using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField ] List<Button> lsBtnShowPlayer=new List<Button>();
    [SerializeField] private List<GameObject> lsGOPlayer = new List<GameObject>();
    [SerializeField] private GameObject goOnline;
    [SerializeField] public Button btnOnline;
    [SerializeField] public Button btnChoseGun;
    [SerializeField] private List<GameObject> lsBtnForPlayer = new List<GameObject>();
    [SerializeField] private List<ItemPlayerList> goPlayerList = new List<ItemPlayerList>();
    [SerializeField] public GameObject prefabListItem;
    [SerializeField] private GameObject goContentList;
    [SerializeField] private Button btnLeave;
    [SerializeField] private List<Button> lsBtnReady = new List<Button>();
    public Button btnStart;
    public void OnSetUp()
    {
        
        lsBtnForPlayer[0].SetActive(true);
        lsBtnForPlayer[1].SetActive(false);
        goOnline.SetActive(false);
        lsGOPlayer[0].SetActive(false);
        lsGOPlayer[1].SetActive(false);
        btnStart.onClick.AddListener(() =>
        {
            SendData<EventName> ev = new SendData<EventName>(new EventName("start"));
            SocketCommunication.GetInstance().Send(JsonUtility.ToJson(ev));
        });
    }

    public void OnChoseGun_Clicked()
    {
        UIManager._instance.uiChoseGun.gameObject.SetActive(true);
    }
    public void ShowPlayerBtn()
    {
        lsBtnForPlayer[1].SetActive(true);
        lsBtnForPlayer[0].SetActive(false);
        lsBtnReady[0].gameObject.SetActive(true);
        lsBtnReady[1].gameObject.SetActive(false);
        
    }

    public void HostChangeLobbyListName(Dictionary<string,Player> players)
    {

        for (int i = goPlayerList.Count-1; i >=0; i--)
        {
            Destroy(goPlayerList[i].goPlayerListItem);
            goPlayerList.RemoveAt(i);
        }
        int index = 0;
        foreach (var pair in players)
        {
            ItemPlayerList item = new ItemPlayerList(pair.Value.name,pair.Key,prefabListItem);
            goPlayerList.Add(item);
            Debug.Log(pair.Value.name);
            item.goPlayerListItem.transform.SetParent(goContentList.transform);
            if (index == 0)
            {
                item.goCrown.SetActive(true);
                item.goBorder.SetActive(true);
            }
            else
            {
                item.btnKick.gameObject.SetActive(true);
            }
            index++;
        }
    }

    public void OnReady_Click(int i)
    { 
        SendData<PlayerIdEvent> data = new SendData<PlayerIdEvent>(new PlayerIdEvent("ready")); 
        SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
        if (i == 0)
        {
           
            lsBtnReady[0].gameObject.SetActive(false);
            lsBtnReady[1].gameObject.SetActive(true);

        }
        else
        {
            lsBtnReady[1].gameObject.SetActive(false);
            lsBtnReady[0].gameObject.SetActive(true);

        }
        
    }
    public void OnLeave_Clicked()
    {
        SendData<PlayerIdEvent> data = new SendData<PlayerIdEvent>(new PlayerIdEvent("leave"));
        SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
        
        //Reset List Player
        Player me = AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID];
        AllManager.Instance().playerManager.dictPlayers.Clear();
        AllManager.Instance().playerManager.dictPlayers.Add(me.id,me);
        UIManager._instance.uiMainMenu.BackShowMain();
        BackShowMain();
    }
    public void ChangeLobbyListName(Dictionary<string, Player> players)
    {
        for (int i = goPlayerList.Count-1; i >= 0; i--)
        {
            Destroy(goPlayerList[i].goPlayerListItem);
            goPlayerList.RemoveAt(i);
        }

        int index = 0;
        foreach (var pair in players)
        {
            if (index == 1)
            {
                ItemPlayerList item = new ItemPlayerList(pair.Value.name, pair.Key, prefabListItem);
                goPlayerList.Add(item);
                item.goCrown.SetActive(true);
                item.goPlayerListItem.transform.SetParent(goContentList.transform);
            }
            else if (index > 1)
            {
                ItemPlayerList item = new ItemPlayerList(pair.Value.name, pair.Key, prefabListItem);
                goPlayerList.Add(item);
                item.goPlayerListItem.transform.SetParent(goContentList.transform);
            }
            index++;
        }
        ItemPlayerList itemMe = new ItemPlayerList(players[Player_ID.MyPlayerID].name, Player_ID.MyPlayerID, prefabListItem);
        goPlayerList.Add(itemMe);
        itemMe.goPlayerListItem.transform.SetParent(goContentList.transform);
        itemMe.goBorder.SetActive(true);
       
    }
    public void AfterCreate()
    {
        btnOnline.gameObject.SetActive(false);
        lsBtnForPlayer[0].SetActive(true);
        lsBtnForPlayer[1].SetActive(false);
        lsGOPlayer[0].SetActive(true);
    }
    public void AfterCreateGuess()
    {
        btnOnline.gameObject.SetActive(false);
        lsBtnForPlayer[1].SetActive(true);
        lsBtnForPlayer[0].SetActive(false);
        lsGOPlayer[0].SetActive(true);
    }
    
    public void OnBtnClick(int index)
    {
        if (index == 0)
        {
            lsGOPlayer[1].SetActive(true);
            lsGOPlayer[0].SetActive(false);
        }
        else
        {
            lsGOPlayer[0].SetActive(true);
            lsGOPlayer[1].SetActive(false);
        }
    }

    public void BackShowMain()
    {
        btnOnline.gameObject.SetActive(true);
        lsBtnForPlayer[0].SetActive(true);
        lsBtnForPlayer[1].SetActive(false);
        lsGOPlayer[0].SetActive(false);
        lsGOPlayer[1].SetActive(false);
    }
    public void OnOnline_Clicked()
    {
        goOnline.SetActive(true);
        UIManager._instance.uiOnlineLobby.btnGetRooms.onClick.Invoke();
    }
    
}
