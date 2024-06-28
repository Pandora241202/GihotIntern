using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;

public class SocketCommunication
{
    private static SocketCommunication instance;
    public static SocketCommunication GetInstance()
    {
        if (instance == null) instance = new SocketCommunication();
        return instance;
    }
    UdpClient udpClient;
    public string address = "127.0.0.1";
    public int port = 9999;
    Thread receiveData;
    public string player_id;


    public SocketCommunication()
    {
        ConnectToServer();
    }

    async void ConnectToServer()
    {
        udpClient = new UdpClient();
        udpClient.Client.ReceiveBufferSize = 1024 * 64;

        string message = "{ \"_event\" : {\"event_name\" : \"first connect\"}}";
        byte[] data = Encoding.UTF8.GetBytes(message);
        await udpClient.SendAsync(data, data.Length, address, 9999);

        Debug.Log("Connected to server");
        receiveData = new Thread(new ThreadStart(handleReceivedData));
        receiveData.IsBackground = true;
        receiveData.Start();
    }

    async void handleReceivedData()
    {
        while (true)
        {
            var received = await udpClient.ReceiveAsync();
            var response = Encoding.UTF8.GetString(received.Buffer);

            EventName _event = JsonUtility.FromJson<EventName>(response);

            switch (_event.event_name)
            {
                case "provide id":
                    //set player id in first connect
                    First_Connect data = JsonUtility.FromJson<First_Connect>(response);
                    Player_ID.MyPlayerID = data.id;
                    Dispatcher.EnqueueToMainThread(() =>
                    {
                        AllManager.Instance().playerManager.AddPlayer(data.player_name, data.id);
                    });
                    break;
                case "rooms":
                    //get available rooms
                    Rooms rooms = JsonUtility.FromJson<Rooms>(response);
                    Dispatcher.EnqueueToMainThread(() => UIManager._instance.uiOnlineLobby.InitListRoom(rooms.rooms));
                    break;
                case "new player join":
                    //other player join room
                    SimplePlayerInfo playerInfo = JsonUtility.FromJson<SimplePlayerInfo>(response);
                    Debug.Log(response);
                    Dispatcher.EnqueueToMainThread(() =>
                    {
                        AllManager.Instance().playerManager.AddPlayer(playerInfo.player_name, playerInfo.player_id);
                        UIManager._instance.uiMainMenu.HostChangeLobbyListName(AllManager.Instance().playerManager.dictPlayers);
                        //UIManager._instance.uiMainMenu.JoinCall(0);
                    });
                    break;
                case "joined":
                    //join a room
                    SimplePlayerInfoList playerIn4List = JsonUtility.FromJson<SimplePlayerInfoList>(response);
                    Debug.Log(response);
                    Dispatcher.EnqueueToMainThread(() =>
                    {
                        for (int i = 0; i < playerIn4List.players.Length; i++)
                        {
                            if (playerIn4List.players[i].player_id == Player_ID.MyPlayerID) continue;
                            AllManager.Instance().playerManager.AddPlayer(playerIn4List.players[i].player_name, playerIn4List.players[i].player_id);
                        }
                        UIManager._instance.uiOnlineLobby.OnGuessJoin();
                    });
                    break;
                case "spawn creep":
                    var creepSpawnInfo = JsonUtility.FromJson<CreepSpawnInfo>(response);
                    Dispatcher.EnqueueToMainThread(() =>
                    {
                        foreach (Vector3 pos in creepSpawnInfo.spawnPos)
                        {
                            AllManager._instance.creepManager.ActivateCreep(pos, (CreepManager.CreepType)creepSpawnInfo.creepTypeInt, creepSpawnInfo.time);
                        }
                    });
                    break;
                case "kick":
                    SimplePlayerInfo kickedPlayer = JsonUtility.FromJson<SimplePlayerInfo>(response);
                    Debug.Log(kickedPlayer.player_id);
                    Debug.Log(kickedPlayer.host_id);
                    Dispatcher.EnqueueToMainThread(() =>
                    {
                        Debug.Log("trc remoce"+  AllManager.Instance().playerManager.dictPlayers.Count);
                        AllManager.Instance().playerManager.RemovePlayer(kickedPlayer.player_id);
                        Debug.Log("Sau remoce"+  AllManager.Instance().playerManager.dictPlayers.Count);

                        if (Player_ID.MyPlayerID == kickedPlayer.host_id)
                        {
                            UIManager._instance.uiMainMenu.HostChangeLobbyListName(AllManager.Instance().playerManager.dictPlayers);
                        }
                        else
                        {
                            UIManager._instance.uiMainMenu.ChangeLobbyListName(AllManager.Instance().playerManager.dictPlayers);
                        }
                    });
                    
                    break;
                case "disband":
                case "kicked":
                    Dispatcher.EnqueueToMainThread(() =>
                    {
                        Player me = AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID];
                        AllManager.Instance().playerManager.dictPlayers.Clear();
                        AllManager.Instance().playerManager.dictPlayers.Add(me.id,me);
                        UIManager._instance.uiMainMenu.BackShowMain();
                    });
                    break;
                case "player_leave":
                    SimplePlayerInfo leave_player = JsonUtility.FromJson<SimplePlayerInfo>(response);
                    Dispatcher.EnqueueToMainThread(() =>
                    {
                        
                        AllManager.Instance().playerManager.RemovePlayer(leave_player.player_id);
                        if (Player_ID.MyPlayerID == leave_player.host_id)
                        {
                            UIManager._instance.uiMainMenu.HostChangeLobbyListName(AllManager.Instance().playerManager.dictPlayers);
                        }
                        else
                        {
                            UIManager._instance.uiMainMenu.ChangeLobbyListName(AllManager.Instance().playerManager.dictPlayers);
                        }
                    });
                    break;
                case "all player ready":
                    Debug.Log("Con cac");
                    Dispatcher.EnqueueToMainThread(() =>
                    {
                        UIManager._instance.uiMainMenu.btnStart.interactable = true;
                    });
                    break;
                case "not all player ready":
                    Debug.Log("Con cac");
                    Dispatcher.EnqueueToMainThread(() =>
                    {
                        UIManager._instance.uiMainMenu.btnStart.interactable =false;
                    });
                    break;
            }
            Debug.Log(_event.event_name);
        
        }
    }

    public async void Send(string msg)
    {
        var messageBytes = Encoding.UTF8.GetBytes(msg);
        await udpClient.SendAsync(messageBytes, messageBytes.Length, address, port);
    }

}