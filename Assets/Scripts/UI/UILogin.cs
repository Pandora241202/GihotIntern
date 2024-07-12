using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UILogin : MonoBehaviour
{
    [SerializeField] private TMP_InputField txtName;
    [SerializeField] private Button btnLogin;

    public void OnSetUp()
    {
        btnLogin.onClick.AddListener(OnLogin_Clicked);
    }

    public void OnLogin_Clicked()
    {
        Debug.Log("login");
        SendData<FirstConnect> data = new SendData<FirstConnect>(new FirstConnect(txtName.text));
        SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
    }
}