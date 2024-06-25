using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOnlineLobby : MonoBehaviour
{
    public Button btnCreateRoom;
    public Button btnGetRooms;

    public void OnGetRoom_Clicked()
    {
        string msg = "{" + Player_ID.MyPlayerID + ":{get_rooms:true}}";
        SocketCommunication.GetInstance().Send(msg);
    }
    public void OnClickCreateRoom_Clicked()
    {
        Debug.Log("OnClickCreateRoom");
        //SocketCommunication.GetInstance().CreateRoom();
    }
}
