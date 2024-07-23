using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;
using System.Linq;


public class PingData
{
    public static System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
    public static long sum = 0;
    public static int pingCount = 1;
    public static bool pinged = false;
}

public class SocketCommunication
{
    private static SocketCommunication instance;
    public static SocketCommunication GetInstance()
    {
        if (instance == null) instance = new SocketCommunication();
        return instance;
    }
    Socket socket;
    //public string address = "192.168.6.165";
    public string address = "127.0.0.1";
    public int port = 9999;
    private static List<byte> buffer = new List<byte>();

    public SocketCommunication()
    {
        ConnectToServer();
    }

    public async void ConnectToServer()
    {
        socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        await socket.ConnectAsync(address, port);

        //Thread readSocket = new Thread(SocketReadingThread);
        //readSocket.IsBackground = true;
        //readSocket.Start();
        AllManager.Instance().StartCoroutine(StartSocketReading());

        //Thread bufferProcessing = new Thread(ProcessBufferThread);
        //bufferProcessing.IsBackground = true;
        //bufferProcessing.Start();
        AllManager.Instance().StartCoroutine(ProcessBuffer());

        Thread ping = new Thread(PingThread);
        ping.IsBackground = true;
        ping.Start();
        //AllManager.Instance().StartCoroutine(Ping());
    }
    private IEnumerator StartSocketReading()
    {
        while (true)
        {
            SocketReading();
            yield return null;
        }

    }

    private async void SocketReading()
    {
        if (socket.Available == 0)
        {
            return;
        }
        byte[] buf = new byte[socket.Available];
        await socket.ReceiveAsync(buf, SocketFlags.None);
        lock(buffer)
        {
            buffer.AddRange(buf);
            //Debug.Log("buffer after read socket: " +  buffer.Count + "/buf len: " + buf.Length);
        }
    }

    //socket read thread
    private async void SocketReadingThread()
    {
        while (true)
        {
            if (socket.Available == 0)
            {
                continue;
            }
            byte[] buf = new byte[socket.Available];
            await socket.ReceiveAsync(buf, SocketFlags.None);
            lock (buffer)
            {
                buffer.AddRange(buf);
                //Debug.Log("buffer after read socket: " + buffer.Count + "/buf len: " + buf.Length);
            }
        }
    }

    //process buffer thread
    private void ProcessBufferThread()
    {
        while (true)
        {
            if (buffer.Count < 4)
            {
                continue;
            }

            //read first 4 bytes for data length
            int dataLength;
            lock (buffer)
            {
                byte[] lengthField = buffer.Take(4).ToArray();
                dataLength = BitConverter.ToInt32(lengthField, 0);
            }

            while (buffer.Count < dataLength + 5)
            {
                continue;
            }
    
;           //data
            string response;
            byte sum;
            byte[] dataField;
            lock (buffer)
            {
                dataField = buffer.Skip(4).Take(dataLength).ToArray();
                response = Encoding.UTF8.GetString(dataField);
                sum = buffer[dataLength + 4];
                //Debug.Log(response + "/data len: " + dataLength + "/buffer len: " + buffer.Count);
                buffer.RemoveRange(0, 4 + dataLength + 1);
                //Debug.Log("buffer after remove: "+buffer.Count);
            }
           

            if (!CheckSum(dataField, sum))
            {
                continue;
            }


            //Debug.Log(response);
            
            EventName _event = JsonUtility.FromJson<EventName>(response);

            switch (_event.event_name)
            {
                case "provide session id":
                    Dispatcher.EnqueueToMainThread(() => { HandleSessionId(response); });
                    break;

                case "provide id":
                    //set player id in first connect
                    Dispatcher.EnqueueToMainThread(() => { HandleProvideId(response); });
                    break;

                case "rooms":
                    //get available rooms
                    Dispatcher.EnqueueToMainThread(() => { HandleRooms(response); });
                    break;

                case "new player join":
                    //other player join room
                    Dispatcher.EnqueueToMainThread(() =>{ HandleNewPlayerJoin(response); });
                    break;

                case "joined":
                    //join a room
                    Dispatcher.EnqueueToMainThread(() => { HandleJoin(response); });
                    break;

                case "pong":
                    //Debug.Log($"Ping: {PingData.stopwatch.ElapsedMilliseconds} ms");
                    PingData.stopwatch.Stop();
                    Dispatcher.EnqueueToMainThread(() => { HandlePing(response); });                      
                    break;

                case "kick":
                    Dispatcher.EnqueueToMainThread(() => { HandleKick(response); });
                    break;

                case "disband":
                case "kicked":
                    Dispatcher.EnqueueToMainThread(() => { HandleKicked(response); });
                    break;

                case "player leave":
                    Dispatcher.EnqueueToMainThread(() => { HandlePlayerLeave(response); });

                    break;
                case "all player ready":
                    Dispatcher.EnqueueToMainThread(() => { HandleAllPlayerReady(response); });
                    break;

                case "not all player ready":
                    Dispatcher.EnqueueToMainThread(() => { HandleNotAllPlayerReady(response); });
                    break;

                case "start":
                    Dispatcher.EnqueueToMainThread(() => { HandleStart(response); });
                    break;

                case "spawn player":
                    Dispatcher.EnqueueToMainThread(() => { HandleSpawnPlayer(response); });
                    break;

                case "update game state":
                    Dispatcher.EnqueueToMainThread(() => { HandleUpdateGameState(response); });
                    break;

                case "player out":
                    Dispatcher.EnqueueToMainThread(() => { HandlePlayerOut(response); });
                    break;

                case "game end":
                    Dispatcher.EnqueueToMainThread(() => { HandleGameEnd(response); });
                    break;

            }
        }
    }

    private IEnumerator ProcessBuffer()
    {
        while (true)
        {
            if (buffer.Count < 4)
            {
                yield return null;
                continue;

            }
            //read first 4 bytes for data length
            byte[] lengthField = buffer.Take(4).ToArray();
            int dataLength = BitConverter.ToInt32(lengthField, 0);

            while (buffer.Count < dataLength + 5)
            {
                yield return null;
                continue;
            }

;                //data
            byte[] dataField = buffer.Skip(4).Take(dataLength).ToArray();
            string response = Encoding.UTF8.GetString(dataField);

            byte sum = buffer[dataLength + 4];
            buffer.RemoveRange(0, 4 + dataLength + 1);
           
            if (!CheckSum(dataField, sum))
            {
                continue;
            }
            

            //Debug.Log(response);
            //Debug.Log(response);
            EventName _event = JsonUtility.FromJson<EventName>(response);

            switch (_event.event_name)
            {
                case "provide session id":
                    HandleSessionId(response);
                    break;

                case "provide id":
                    //set player id in first connect
                    HandleProvideId(response);
                    break;

                case "rooms":
                    //get available rooms
                    HandleRooms(response);
                    break;

                case "new player join":
                    //other player join room
                    HandleNewPlayerJoin(response);
                    break;

                case "joined":
                    //join a room
                    HandleJoin(response);
                    break;

                case "pong":
                    //Debug.Log($"Ping: {PingData.stopwatch.ElapsedMilliseconds} ms");
                    HandlePing(response);
                    break;

                case "kick":
                    HandleKick(response);
                    break;

                case "disband":
                case "kicked":
                    HandleKicked(response);
                    break;

                case "player leave":
                    HandlePlayerLeave(response);

                    break;
                case "all player ready":
                    HandleAllPlayerReady(response);
                    break;

                case "not all player ready":
                    HandleNotAllPlayerReady(response);
                    break;

                case "start":
                    HandleStart(response);
                    break;

                case "spawn player":
                    HandleSpawnPlayer(response);
                    break;

                case "update game state":
                    HandleUpdateGameState(response);
                    break;

                case "player out":
                    HandlePlayerOut(response);
                    break;

                case "game end":
                    HandleGameEnd(response);
                    break;

            }

            if (buffer.Count > 4) continue;
            yield return null;
        }
    }

    private bool CheckSum(byte[] buffer, byte sum)
    {
        int check = 0;
        foreach(byte b in buffer)
        {
            check = (check + b) % 256;
        }
        return (byte)check == sum;
    }

    public IEnumerator Wait()
    {
        AllManager.Instance().isPause = true;
        yield return new WaitForSecondsRealtime(0f);
        AllManager.Instance().GameEnd();

    }
   
    //public async void Send(string msg)
    //{
    //    var messageBytes = Encoding.UTF8.GetBytes(msg);
    //    await udpClient.SendAsync(messageBytes, messageBytes.Length, address, port);
    //}

    private byte CalSum(byte[] bytes)
    {
        int sum = 0;
        foreach(byte b in bytes)
        {
            sum = (sum + b) % 256;
        }

        return (byte)sum;
    }

    public async void Send(string msg)
    {
        var messageBytes = Encoding.UTF8.GetBytes(msg);
        var messageLength = messageBytes.Length;

        byte[] lengthField = new byte[4];
        Buffer.BlockCopy(BitConverter.GetBytes(messageLength), 0, lengthField, 0, 4);
        Array.Reverse(lengthField);

        byte sum = CalSum(messageBytes);

        byte[] sendData = new byte[4 + messageBytes.Length + 1];
        Buffer.BlockCopy(lengthField, 0, sendData, 0, 4);
        Buffer.BlockCopy(messageBytes, 0, sendData, 4, messageLength);
        sendData[4 + messageLength] = sum;

        await socket.SendAsync(sendData, SocketFlags.None);
    }

    public void Close()
    {
        socket.Disconnect(false);
        socket.Close();
    }

    public IEnumerator Ping()
    {
        while (true)
        {
            if(Player_ID.SessionId != null && !PingData.pinged)
            {
                SendData<PingEvent> data = new SendData<PingEvent>(new PingEvent());
                Send(JsonUtility.ToJson(data));
                PingData.stopwatch.Restart();
                PingData.pinged  = true;
            }
            yield return new WaitForSeconds(1);
        }
    }

    private void PingThread()
    {
        while (true)
        {
            if (Player_ID.SessionId != null && !PingData.pinged)
            {
                SendData<PingEvent> data = new SendData<PingEvent>(new PingEvent());
                Send(JsonUtility.ToJson(data));
                PingData.stopwatch.Restart();
                PingData.pinged = true;
            }
            Thread.Sleep(100);
        }
    }

    private void HandleSessionId(string response)
    {
        First_Connect fData = JsonUtility.FromJson<First_Connect>(response);
        Player_ID.SessionId = fData.id;
    }

    private void HandleProvideId(string response)
    {
        First_Connect data = JsonUtility.FromJson<First_Connect>(response);
        Player_ID.MyPlayerID = data.id;

        AllManager.Instance().playerManager.AddPlayer(data.player_name, data.id, data.gunId, AllManager.Instance().playerConfig);
        UIManager._instance.OnLogin();
    }

    private void HandleRooms(string response)
    {
        Rooms rooms = JsonUtility.FromJson<Rooms>(response);
        UIManager._instance.uiOnlineLobby.InitListRoom(rooms.rooms);
    }

    private void HandleNewPlayerJoin(string response)
    {
        SimplePlayerInfo playerInfo = JsonUtility.FromJson<SimplePlayerInfo>(response);
        AllManager.Instance().playerManager.AddPlayer(playerInfo.player_name, playerInfo.player_id, playerInfo.gun_id, AllManager.Instance().playerConfig);
        UIManager._instance.uiMainMenu.HostChangeLobbyListName(AllManager.Instance().playerManager.dictPlayers);
    }

    private void HandleJoin(string response)
    {
        SimplePlayerInfoList playerIn4List = JsonUtility.FromJson<SimplePlayerInfoList>(response);
        for (int i = 0; i < playerIn4List.players.Length; i++)
        {
            if (playerIn4List.players[i].player_id == Player_ID.MyPlayerID) continue;
            AllManager.Instance().playerManager.AddPlayer(playerIn4List.players[i].player_name, playerIn4List.players[i].player_id, playerIn4List.players[i].gun_id, AllManager.Instance().playerConfig);
        }
        UIManager._instance.uiOnlineLobby.OnGuessJoin();
    }

    private void HandlePing(string response)
    {
        PingData.sum += PingData.stopwatch.ElapsedMilliseconds;
        PingData.pingCount += 2;
        PingData.pinged = false;
    }

    private void HandleKick(string response)
    {
        SimplePlayerInfo kickedPlayer = JsonUtility.FromJson<SimplePlayerInfo>(response);
        AllManager.Instance().playerManager.RemovePlayer(kickedPlayer.player_id);

        if (Player_ID.MyPlayerID == kickedPlayer.host_id)
        {
            UIManager._instance.uiMainMenu.HostChangeLobbyListName(AllManager.Instance().playerManager.dictPlayers);
        }
        else
        {
            UIManager._instance.uiMainMenu.ChangeLobbyListName(AllManager.Instance().playerManager.dictPlayers);
        }
    }

    private void HandleKicked(string response)
    {
        Player me = AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID];
        AllManager.Instance().playerManager.dictPlayers.Clear();
        AllManager.Instance().playerManager.dictPlayers.Add(me.id, me);
        UIManager._instance.uiMainMenu.BackShowMain();
    }

    private void HandlePlayerLeave(string response)
    {
        SimplePlayerInfo leave_player = JsonUtility.FromJson<SimplePlayerInfo>(response);

        AllManager.Instance().playerManager.RemovePlayer(leave_player.player_id);
        if (Player_ID.MyPlayerID == leave_player.host_id)
        {
            UIManager._instance.uiMainMenu.HostChangeLobbyListName(AllManager.Instance().playerManager.dictPlayers);
        }
        else
        {
            UIManager._instance.uiMainMenu.ChangeLobbyListName(AllManager.Instance().playerManager.dictPlayers);
        }
    }

    private void HandleAllPlayerReady(string response)
    {
        UIManager._instance.uiMainMenu.btnStart.interactable = true;
    }

    private void HandleNotAllPlayerReady(string response)
    {
        UIManager._instance.uiMainMenu.btnStart.interactable = false;
    }

    private void HandleStart(string response)
    {
        AllManager.Instance().LoadSceneAsync("level1");
    }

    private void HandleSpawnPlayer(string response)
    {
        AllPlayerSpanwPos all = JsonUtility.FromJson<AllPlayerSpanwPos>(response);

        for (int i = 0; i < all.data.Length; i++)
        {
            AllManager.Instance().playerManager.SpawnPlayer(all.data[i].spawn_pos, all.data[i].player_id, all.data[i].gun_id);
        }

        AllManager.Instance().isPause = false;
        SendData<EventName> dataDoneSpawn = new SendData<EventName>(new EventName("spawn done"));
        Send(JsonUtility.ToJson(dataDoneSpawn));

        UIManager._instance.uiGameplay.OnSetUp(AllManager._instance.playerManager.GetMaxHealthFromLevel(), AllManager._instance.playerManager.expRequire);
        UIManager._instance.uiGameplay.gameObject.SetActive(true);
        UIManager._instance._fjoystick.gameObject.SetActive(true);
    }

    private void HandleUpdateGameState(string response)
    {
        GameState gameState = JsonUtility.FromJson<GameState>(response);
        //Debug.Log(response);
        AllManager.Instance().UpdateGameState(gameState);
    }

    private void HandlePlayerOut(string response)
    {
        SimplePlayerInfo playerOut = JsonUtility.FromJson<SimplePlayerInfo>(response);
        AllManager.Instance().playerManager.RemovePlayer(playerOut.player_id);
    }

    private void HandleGameEnd(string response)
    {
        GameEnd end = JsonUtility.FromJson<GameEnd>(response);

        //foreach (var sc in end.result)
        //{
        //    Debug.Log($"Player id: {sc.player_id}, score: {sc.enemy_kill}");
        //}
        UIManager._instance.uiDefeat.OnSetUp(end);

    }
}