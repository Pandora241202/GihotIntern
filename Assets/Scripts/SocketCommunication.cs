using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Text;
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

    [System.Serializable]
    class ReceivedData
    {
        public string event_name;
        public string id;
        public string player_id;
        [field: SerializeField] public Vector3 position;
        [field: SerializeField] public Vector3 direction;
        public Room[] rooms;
    }

    [System.Serializable]
    class Room
    {
        public string id;
        public string name;
        public string game_mode;
    }

    public SocketCommunication()
    {
        ConnectToServer();
    }

    async void ConnectToServer()
    {
        socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        await socket.ConnectAsync(address, port);

        Debug.Log("Connected to server");
        receiveData = new Thread(new ThreadStart(handleReceivedData));
        receiveData.Start();
    }

    async void handleReceivedData()
    {
        while (true)
        {
            var buffer = new byte[1_024];
            var received = await socket.ReceiveAsync(buffer, SocketFlags.None);
            var response = Encoding.UTF8.GetString(buffer, 0, received);

            ReceivedData json = JsonUtility.FromJson<ReceivedData>(response);

            switch (json.event_name)
            {
                case "provide id":
                    //set player id
                    Player_ID.MyPlayerID = json.id;
                    break;
                case "rooms":
                    //do sth
                    Debug.Log(response);
                    Debug.Log(json.rooms[0].name);
                    Debug.Log(json.rooms[0].id);
                    Debug.Log(json.rooms[0].game_mode);
                    break;
            }
            Debug.Log(json.event_name);
        
        }
    }

    public async void Send(string msg)
    {
        var messageBytes = Encoding.UTF8.GetBytes(msg);
        await socket.SendAsync(messageBytes, SocketFlags.None);
    }

}