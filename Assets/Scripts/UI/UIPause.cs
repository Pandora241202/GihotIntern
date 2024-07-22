using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPause : MonoBehaviour
{
    public List<Button> lsBtnPause;

    public void OnBtn_Clicked(int index)
    {
        UIManager._instance.PlaySfx(0);
        switch (index)
        {
            case 0:
                //Setting
                break;
            case 1:
                //Resume
                SendData<ResumeEvent> dt = new SendData<ResumeEvent>(new ResumeEvent());
                SocketCommunication.GetInstance().Send(JsonUtility.ToJson(dt));
                break;
            case 2:
                //Leave
                SendData<PlayerOutEvent> ev = new SendData<PlayerOutEvent>(new PlayerOutEvent());
                SocketCommunication.GetInstance().Send(JsonUtility.ToJson(ev));
                break;
        }
    }
}
