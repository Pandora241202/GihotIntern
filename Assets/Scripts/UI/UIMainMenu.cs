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
    [SerializeField] private List<GameObject> lsBtnForPlayer = new List<GameObject>();
    [SerializeField] private List<TextMeshProUGUI> lsTxtName = new List<TextMeshProUGUI>();
    [SerializeField] private List<ItemPlayerList> goPlayerList = new List<ItemPlayerList>();
    [SerializeField] public GameObject prefabListItem;
    [SerializeField] private GameObject goContentList;
    public void OnSetUp()
    {
        
        lsBtnForPlayer[0].SetActive(true);
        lsBtnForPlayer[1].SetActive(false);
        goOnline.SetActive(false);
        lsGOPlayer[0].SetActive(false);
        lsGOPlayer[1].SetActive(false);
    }

    public void ShowPlayerBtn()
    {
        lsBtnForPlayer[1].SetActive(true);
        lsBtnForPlayer[0].SetActive(false);
    }

    public void HostChangeLobbyListName(Dictionary<string,Player> players)
    {

        for (int i = players.Count-1; i >= 0; i--)
        {
            if (goPlayerList.Count == 0) break;
            Destroy(goPlayerList[i].goPlayerListItem);
            goPlayerList.RemoveAt(i);
        }
        int index = 0;
        foreach (var pair in players)
        {
            ItemPlayerList item = new ItemPlayerList(pair.Value.name,pair.Key,prefabListItem);
            goPlayerList.Add(item);
            item.goPlayerListItem.transform.SetParent(goContentList.transform);
            if (index == 0)
            {
                item.goCrown.SetActive(true);
            }
            else
            {
                item.btnKick.gameObject.SetActive(true);
            }
            index++;
        }
    }

    public void ChangeLobbyListName(Dictionary<string, Player> players)
    {
        for (int i = players.Count-1; i >= 0; i--)
        {
            if (goPlayerList.Count == 0) break;
            Destroy(goPlayerList[i].goPlayerListItem);
            goPlayerList.RemoveAt(i);
        }
        foreach (var pair in players)
        {
            ItemPlayerList item = new ItemPlayerList(pair.Value.name, pair.Key, prefabListItem);
            goPlayerList.Add(item);
            item.goPlayerListItem.transform.SetParent(goContentList.transform);
        }
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

    // public void JoinCall(int i)
    // {
    //     if (i == 0)
    //     {
    //         goBorderMe[i].SetActive(true);
    //         btnKickForHost.gameObject.SetActive(true);
    //     }
    //     else
    //     {
    //         goBorderMe[i].SetActive(true);
    //         btnKickForHost.gameObject.SetActive(false);
    //     }
    // }
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

    public void OnOnline_Clicked()
    {
        goOnline.SetActive(true);
    }
    
}
