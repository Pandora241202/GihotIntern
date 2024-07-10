using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
class JoinRequestEvent
{
    public string event_name = "join_room";
    public string room_id;
    public JoinRequestEvent(string room_id)
    {
        this.room_id = room_id;
    }
}

public class BtnLobby 
{
    public Button button;
    public string room_id;
    public string room_name;
    public string game_mode;

    public BtnLobby(GameObject button, string room_id, string room_name, string game_mode)
    {
        this.button = GameObject.Instantiate(button).GetComponent<Button>();
        ItemLobby item = this.button.gameObject.GetComponent<ItemLobby>();
        item.OnGetLobby(room_name, game_mode);
        this.room_id = room_id;
        this.room_name = room_name;
        this.game_mode = game_mode;
        
        this.button.onClick.AddListener(() =>
        {
            
            SendData<JoinRequestEvent> data = new SendData<JoinRequestEvent>(new JoinRequestEvent(room_id));
            SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
            AllManager._instance.isHost = false;
        });
        
    }
}
