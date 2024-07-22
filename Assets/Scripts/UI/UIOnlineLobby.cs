using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIOnlineLobby : MonoBehaviour
{
    [Header("================Button For Online UI===============")]
    public Button btnCreateRoom;
    public Button btnGetRooms;
    [SerializeField] private GameObject prefabBtnRoom;
    public GameObject scrollViewContent;
    [SerializeField] private Button btnCloseOnline;
    
    
    [Header("==================PopUpCreate========================")]
    private List<BtnLobby> lsBtn = new List<BtnLobby>();
    [SerializeField] private GameObject popupCreate;
    [SerializeField] private Button btnPopUpCreate;
    [SerializeField] private GameObject inputRoomName;
    [SerializeField] private TMP_InputField inRoomName;
    [SerializeField] private TMP_Dropdown ddGameMode;
    [SerializeField] private Button btnCloseCreatePU;
 
   

    public void OnClose()
    {
        UIManager._instance.PlaySfx(0);
        Debug.Log(gameObject.name);
        gameObject.SetActive(false);
    }
    public void OnPopUpCreate_Clicked()
    {
        UIManager._instance.PlaySfx(0);
        popupCreate.SetActive(true);
    }
    public void OnSetUp()
    {
        popupCreate.SetActive(false);
        btnCloseCreatePU.onClick.AddListener(() =>
        {
            UIManager._instance.PlaySfx(0);
            popupCreate.SetActive(false);
        });
    }
    

    public void OnGetRoom_Clicked()
    {
        UIManager._instance.PlaySfx(0);
        SendData<OnlineLobbyEvent> data = new SendData<OnlineLobbyEvent>(new OnlineLobbyEvent("get_rooms", true));
        
        SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
        
    }
    public void OnClickCreateRoom_Clicked()
    {
        UIManager._instance.PlaySfx(0);
        SendData<OnlineLobbyEvent> data = new SendData<OnlineLobbyEvent>(new OnlineLobbyEvent("create_rooms", true, inRoomName.text, ddGameMode.options[ddGameMode.value].text));
        SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
        popupCreate.SetActive(false);
        //SocketCommunication.GetInstance().CreateRoom();
        Debug.Log(AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].name);
        UIManager._instance.uiMainMenu.HostChangeLobbyListName(AllManager.Instance().playerManager.dictPlayers);
        UIManager._instance.uiMainMenu.AfterCreate();
        //UIManager._instance.uiMainMenu.btnStart.interactable = false;
        AllManager._instance.isHost = true;
        gameObject.SetActive(false);
    }

    public void OnGuessJoin()
    {
        UIManager._instance.uiMainMenu.ChangeLobbyListName(AllManager.Instance().playerManager.dictPlayers);
        UIManager._instance.uiMainMenu.AfterCreateGuess();
        gameObject.SetActive(false);
    }

    public void InitListRoom(Room[] rooms)
    {
        for (int i = lsBtn.Count-1; i >= 0; i--)
        {
            Destroy(lsBtn[i].button.gameObject);
            lsBtn.RemoveAt(i);
        }
        //Debug.Log("Length"+lsBtn.Count);
        for (int i = 0; i < rooms.Length; i++)
        {
            //Debug.Log(rooms[i].game_mode+" "+rooms[i].name);
            BtnLobby btn = new BtnLobby(prefabBtnRoom, rooms[i].id, rooms[i].name, rooms[i].game_mode);
            GameObject btnContent = btn.button.gameObject;
            btnContent.transform.SetParent(scrollViewContent.transform);
            lsBtn.Add(btn);
        }
        //Debug.Log("Length"+lsBtn.Count);
    }
}
