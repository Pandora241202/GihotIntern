using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ID : MonoBehaviour
{
    public static string MyPlayerID { get; set; }

    private void Start()
    {
        SocketCommunication.GetInstance();
        DontDestroyOnLoad(this);
    }
}
