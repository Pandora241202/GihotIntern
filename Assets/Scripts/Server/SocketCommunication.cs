using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;
using System.Linq;
using System.Threading.Tasks;

public class SocketCommunication
{
    private static SocketCommunication instance;
    public static SocketCommunication GetInstance()
    {
        if (instance == null) instance = new SocketCommunication();
        return instance;
    }
    Socket socket;
    public string address = "127.0.0.1";
    public int port = 9999;
    Thread receiveData;
    public string player_id;
    public List<byte> buffer = new List<byte>();

    public SocketCommunication()
    {
        ConnectToServer();
    }

    public async void ConnectToServer()
    {
        socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        await socket.ConnectAsync(address, port);

        //Thread readSocket = new Thread(SocketReading);
        //readSocket.IsBackground = true;
        //readSocket.Start();
        AllManager.Instance().StartCoroutine(StartSocketReading());

        //Thread bufferProcessing = new Thread(ProcessBuffer);
        //bufferProcessing.IsBackground = true;
        //bufferProcessing.Start();
        AllManager.Instance().StartCoroutine(ProcessBuffer());
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
        buffer.AddRange(buf);
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
                    First_Connect fData = JsonUtility.FromJson<First_Connect>(response);
                    Player_ID.SessionId = fData.id;
                    break;
                case "provide id":
                    //set player id in first connect
                    First_Connect data = JsonUtility.FromJson<First_Connect>(response);
                    Player_ID.MyPlayerID = data.id;

                    AllManager.Instance().playerManager.AddPlayer(data.player_name, data.id, data.gunId, AllManager.Instance().playerConfig);
                    UIManager._instance.OnLogin();
                    break;
                case "rooms":
                    //get available rooms
                    Rooms rooms = JsonUtility.FromJson<Rooms>(response);
                    UIManager._instance.uiOnlineLobby.InitListRoom(rooms.rooms);
                    break;
                case "new player join":
                    //other player join room
                    SimplePlayerInfo playerInfo = JsonUtility.FromJson<SimplePlayerInfo>(response);

                    AllManager.Instance().playerManager.AddPlayer(playerInfo.player_name, playerInfo.player_id, playerInfo.gun_id, AllManager.Instance().playerConfig);
                    UIManager._instance.uiMainMenu.HostChangeLobbyListName(AllManager.Instance().playerManager.dictPlayers);
                    //UIManager._instance.uiMainMenu.JoinCall(0);

                    break;
                case "joined":
                    //join a room
                    SimplePlayerInfoList playerIn4List = JsonUtility.FromJson<SimplePlayerInfoList>(response);
                    for (int i = 0; i < playerIn4List.players.Length; i++)
                    {
                        if (playerIn4List.players[i].player_id == Player_ID.MyPlayerID) continue;
                        AllManager.Instance().playerManager.AddPlayer(playerIn4List.players[i].player_name, playerIn4List.players[i].player_id, playerIn4List.players[i].gun_id, AllManager.Instance().playerConfig);
                    }
                    UIManager._instance.uiOnlineLobby.OnGuessJoin();
                    break;
                case "spawn creep":
                    var creepSpawnInfo = JsonUtility.FromJson<CreepSpawnInfo>(response);
                    //if (AllManager._instance.sceneUpdater == null) break;
                    foreach (Vector3 pos in creepSpawnInfo.spawnPos)
                    {
                        AllManager._instance.creepManager.ActivateCreep(pos, (CreepManager.CreepType)creepSpawnInfo.creepTypeInt, creepSpawnInfo.time);
                    }

                    break;
                case "kick":
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


                    break;
                case "disband":
                case "kicked":

                    Player me = AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID];
                    AllManager.Instance().playerManager.dictPlayers.Clear();
                    AllManager.Instance().playerManager.dictPlayers.Add(me.id, me);
                    UIManager._instance.uiMainMenu.BackShowMain();

                    break;
                case "player leave":
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

                    break;
                case "all player ready":

                    UIManager._instance.uiMainMenu.btnStart.interactable = true;

                    break;
                case "not all player ready":

                    UIManager._instance.uiMainMenu.btnStart.interactable = false;

                    break;
                case "start":

                    AllManager.Instance().LoadSceneAsync("level1");

                    break;
                case "spawn player":
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
                    break;

                case "update game state":
                    GameState gameState = JsonUtility.FromJson<GameState>(response);
                    //Debug.Log(response);
                    AllManager.Instance().UpdateGameState(gameState);
                    break;

                case "player out":
                    SimplePlayerInfo playerOut = JsonUtility.FromJson<SimplePlayerInfo>(response);
                    AllManager.Instance().playerManager.RemovePlayer(playerOut.player_id);
                    break;

                case "destroy creep":
                    CreepDestroyInfo creepDestroyInfo = JsonUtility.FromJson<CreepDestroyInfo>(response);
                    AllManager.Instance().creepManager.SendCreepToDeadBySharedId(creepDestroyInfo.creep_id);
                    break;

                //case "revive":
                //    ReviveEvent revive = JsonUtility.FromJson<ReviveEvent>(response);
                //    AllManager.Instance().playerManager.OnRevive(revive.revive_player_id);
                //    break;

                case "game end":
                    AllManager.Instance().StartCoroutine(Wait());
                    
                    break;

            }

            // Debug.Log(response);
            //remove processed data from buffer
            //Debug.Log(response);
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
}