using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class AllManager : MonoBehaviour
{
    public static AllManager _instance { get; private set; }
    public PlayerManager playerManager;
    public GunConfig gunConfig;
    [SerializeField] public AllCreepConfig allCreepConfig;
    [SerializeField] public AllDropItemConfig allDropItemConfig;
    [SerializeField] public PlayerConfig playerConfig;
    [SerializeField] public AllGameEventConfig allGameEventConfig;
    [SerializeField] GameObject characterPrefab;
    [SerializeField] public LevelUpConfig levelUpConfig;

    [SerializeField] public DroneConfig droneConfig;
    [SerializeField] public UpgradesConfig upgradeConfig;
    [SerializeField] public LayerMask treeLayerMask;

    public SceneUpdater sceneUpdater;
    public BulletManager bulletManager;
    public CreepManager creepManager;
    public PowerUpManager powerUpManager;
    public GameEventManager gameEventManager;

    public DroneManager droneManager;

    public RenderManager renderManager;
    public bool isPause = false;
    public bool isHost=false;
    public bool isLevelUp = false;
    public List<AudioClip> lsAudioClip = new List<AudioClip>();
    public AudioSource audioSource;
    
    public GameObject goEventGoTo;
    public List<GameObject> lsGoToEvent = new List<GameObject>();
    public List<GameObject> lsArrow = new List<GameObject>();

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
        playerManager = new PlayerManager(characterPrefab, levelUpConfig);
        StartCoroutine(UpdatePing());
    }
    private void Update()
    {
        
    }
    private void LateUpdate()
    {

    }

    public void SpawnGoToPosEvent(Vector3 posA,Vector3 posB)
    {
        GameObject gotoposA = Instantiate(goEventGoTo);
        gotoposA.transform.position = posA;
        GameObject gotoposB = Instantiate(goEventGoTo);
        gotoposB.transform.position = posB;
        lsGoToEvent.Add(gotoposA);
        lsGoToEvent.Add(gotoposB);
    }
    public void LoadSceneAsync(string sceneName, string mode = "")
    {
        StartCoroutine(LoadScene(sceneName, mode));
    }

    private IEnumerator LoadScene(string sceneName, string mode)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        UIManager._instance.uiLoading.gameObject.SetActive(true);
        UIManager._instance.uiLoading.OnSetUp();
        while (!asyncLoad.isDone)
        {
            UIManager._instance.uiLoading.OnProgress(asyncLoad.progress);
            yield return null;
        }
        UIManager._instance.uiLoading.gameObject.SetActive(false);
        OnSceneLoaded(sceneName, mode);
        Debug.Log("Scene loaded!");

    }

    private void OnSceneLoaded(string sceneName, string mode)
    {
        //sceneUpdater = null;
        //bulletManager = null;
        //creepManager = null;
        //powerUpManager = null;
        UIManager._instance.ClearUI();
        if (sceneName == "level1")
        { 
            sceneUpdater = GameObject.FindObjectOfType<SceneUpdater>();
            //Debug.Log(sceneUpdater);
            creepManager = sceneUpdater.creepManager;
            bulletManager = sceneUpdater.bulletManager;
            powerUpManager = sceneUpdater.powerUpManager;
            gameEventManager = sceneUpdater.gameEventManager;
            droneManager = sceneUpdater.droneManager;
            renderManager = sceneUpdater.renderManager;
            UIManager._instance.OnLoadGameScene();
            playerManager.FreshStart();
      
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
    
    public IEnumerator GameEnd()
    {
        isPause = true;
        yield return new WaitForEndOfFrame();
        foreach(var player in playerManager.dictPlayers)
        {
            GameObject.Destroy(player.Value.playerTrans.gameObject);
        }
       LoadSceneAsync("UI", "Room");
    }
    
    public void UpdateGameState(GameState gameState)
    {
        GameStateData state = gameState.state;

        if(state.resume.isResume)
        {
            //show resume coutdown
            Debug.Log(state.resume.time);
        }

        if(isPause != state.isPause&&!state.isLevelUp)
        {
            isPause = state.isPause;
            if (isPause) UIManager._instance.PauseGame();
            else UIManager._instance.ResumeGame();
        }

        if (isLevelUp != state.isLevelUp)
        {
            isLevelUp = state.isLevelUp;
            if (isLevelUp) isPause = true;
            else
            {
                isPause = false;
                UIManager._instance.uiGameplay.goWaiting.SetActive(false);
                
            }
        }
    
        playerManager.UpdatePlayersState(state.player_states);

        creepManager.UpdateCreepsState(state.creep_spawn_infos, state.creep_destroy_infos);

        powerUpManager.UpdatePowerUpsState(state.power_up_pick_infos);

        gameEventManager.UpdateEventState(state.game_event);
        
    }

    public IEnumerator UpdatePing()
    {
        while(true)
        {
            yield return new WaitForSeconds(2);
            //Debug.Log(PingData.sum + "/" + PingData.pingCount);
            UIManager._instance.uiGameplay.UpdatePingText(PingData.sum / PingData.pingCount);
            PingData.sum = 0;
            PingData.pingCount = 1;
        }
    }
}