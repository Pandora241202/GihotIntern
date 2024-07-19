using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChoseGun : MonoBehaviour
{
    [SerializeField] private List<Button> lsBtnGun =new List<Button>();

    public void OnSetUp()
    {
        for (int i = 0; i < lsBtnGun.Count; i++)
        {
           
            lsBtnGun[i].interactable = true;
            
        }
    }
    public void OnGun_Clicked(int index)
    {
        SendData<ChoseGunEvent> ev = new SendData<ChoseGunEvent>(new ChoseGunEvent(index));
        SocketCommunication.GetInstance().Send(JsonUtility.ToJson(ev));
        Debug.Log(index);
        UIManager._instance.PlaySfx(0);
        this.gameObject.SetActive(false);
    }
}
