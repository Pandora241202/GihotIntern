using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AllManager : MonoBehaviour
{
    public static AllManager _instance { get; private set; }
    public PlayerManager playerManager;
    public GunConfig gunConfig;
    [SerializeField] public AllCreepConfig allCreepConfig;
    [SerializeField] public AllDropItemConfig allDropItemConfig;
    [SerializeField] public PlayerConfig playerConfig;
    [SerializeField] GameObject characterPrefab;
    public SceneUpdater sceneUpdater;
    public BulletManager bulletManager;
    public CreepManager creepManager;
    public PowerUpManager powerUpManager;
    public bool isPause = false;
    public bool isHost=false;
    public static AllManager Instance()
    {
        return _instance;
    }

    private void Awake()
    {
        if(_instance == null) _instance = this;
        SocketCommunication.GetInstance();
    }

    private void Start()
    {
        playerManager = new PlayerManager(characterPrefab);
    }
    private void Update()
    {

    }
    private void LateUpdate()
    {

    }

    public void LoadSceneAsync(string sceneName, string mode = "")
    {
        StartCoroutine(LoadScene(sceneName, mode));
    }

    private IEnumerator LoadScene(string sceneName, string mode)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        OnSceneLoaded(sceneName, mode);
        Debug.Log("Scene loaded!");

    }

    private void OnSceneLoaded(string sceneName, string mode)
    {
        //sceneUpdater = null;
        if (sceneName == "level1")
        {
            UIManager._instance.uiMainMenu.gameObject.SetActive(false);
            UIManager._instance.uiGameplay.gameObject.SetActive(true);
            sceneUpdater = GameObject.FindObjectOfType<SceneUpdater>();
            playerManager.FreshStart();
            //Debug.Log(sceneUpdater);
            creepManager = sceneUpdater.creepManager;
            bulletManager = sceneUpdater.bulletManager;
            powerUpManager = sceneUpdater.powerUpManager;
            SendData<EventName> ev = new SendData<EventName>(new EventName("done loading"));
            SocketCommunication.GetInstance().Send(JsonUtility.ToJson(ev));
        }
        else if(sceneName == "UI")
        {
            if(mode == "Main Menu")
            {
                UIManager._instance.uiPause.gameObject.SetActive(false);
                UIManager._instance.uiGameplay.gameObject.SetActive(false);
                UIManager._instance.uiMainMenu.gameObject.SetActive(true);
                UIManager._instance.uiMainMenu.BackShowMain();
                UIManager._instance.ResumeGame();
            }
            else if(mode == "Room")
            {
                UIManager._instance.uiPause.gameObject.SetActive(false);
                UIManager._instance.uiGameplay.gameObject.SetActive(false);
                UIManager._instance.uiMainMenu.gameObject.SetActive(true);
                if (isHost)
                {
                    UIManager._instance.uiMainMenu.BackShowRoom(1);
                }
                else
                {
                    UIManager._instance.uiMainMenu.BackShowRoom(0);
                }
            }
            
        }
        //SocketCommunication.GetInstance().Send(JsonUtility.ToJson(ev));
        
    }

    private void OnApplicationQuit()
    {
        SendData<QuitEvent> data = new SendData<QuitEvent>(new QuitEvent());
        SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
        SocketCommunication.GetInstance().Close();
    }
    
    public void GameEnd()
    {
        foreach(var player in playerManager.dictPlayers)
        {
            GameObject.Destroy(player.Value.playerTrans.gameObject);
        }
       LoadSceneAsync("UI", "Room");
    }
    
}