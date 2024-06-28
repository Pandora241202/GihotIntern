using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
class ItemPlayerEvent
{
    public string event_name;
    public bool value;
    public string player_id;
    public string host_id;
    public ItemPlayerEvent(string event_name,bool value,string id)
    {
        this.event_name = event_name;
        this.value = value;
        this.host_id = Player_ID.MyPlayerID;
        this.player_id = id;
    }
}
public class ItemPlayerList
{
    public TextMeshProUGUI txtName;
    public string id;
    public GameObject goPlayerListItem;
    public GameObject goCrown;
    public Button btnKick;
    public GameObject goBorder;

    public ItemPlayerList(string name,string id,GameObject goListPlayerPrefabs)
    {
        this.id = id;
        goPlayerListItem = GameObject.Instantiate(goListPlayerPrefabs);
        txtName = this.goPlayerListItem.transform.Find("txtNamePlayer").GetComponent<TextMeshProUGUI>();
        
        goCrown = goPlayerListItem.transform.Find("imgIconHost").gameObject;
        btnKick=goPlayerListItem.transform.Find("btnKick").gameObject.GetComponent<Button>();
        goBorder = goPlayerListItem.transform.Find("imgBorder").gameObject;
        goCrown.gameObject.SetActive(false);
        btnKick.gameObject.SetActive(false);
        goBorder.gameObject.SetActive(false);
        txtName.text = name;
        btnKick.onClick.AddListener(() =>
        {
//            Debug.Log(id);
            
            SendData<ItemPlayerEvent> data = new SendData<ItemPlayerEvent>(new ItemPlayerEvent("kick_player", true, id));
            SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
        });
    }

}
