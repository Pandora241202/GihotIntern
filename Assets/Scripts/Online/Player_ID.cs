using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ID : MonoBehaviour
{
    public static string MyPlayerID { get; set; }
    public static string SessionId { get; set; }
    public static Player_ID instance;
    private void Start()
    {
        DontDestroyOnLoad(this);
        SocketCommunication.GetInstance();
        if (instance == null) instance = this;
        else GameObject.Destroy(this);
        
    }
}