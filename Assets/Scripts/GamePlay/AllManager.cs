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

    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        OnSceneLoaded(sceneName);
        Debug.Log("Scene loaded!");

    }

    private void OnSceneLoaded(string sceneName)
    {
        
        if (sceneName == "level1")
        {

            UIManager._instance.uiMainMenu.gameObject.SetActive(false);
            UIManager._instance.uiGameplay.gameObject.SetActive(true);
            sceneUpdater = GameObject.FindObjectOfType<SceneUpdater>();
            //Debug.Log(sceneUpdater);
            creepManager = sceneUpdater.creepManager;
            bulletManager = sceneUpdater.bulletManager;
            powerUpManager = sceneUpdater.powerUpManager;
            SendData<EventName> ev = new SendData<EventName>(new EventName("done loading"));
            SocketCommunication.GetInstance().Send(JsonUtility.ToJson(ev));
        }
        else if(sceneName == "UI")
        {
            UIManager._instance.uiPause.gameObject.SetActive(false);
            UIManager._instance.uiGameplay.gameObject.SetActive(false);
            UIManager._instance.uiMainMenu.gameObject.SetActive(true);
            UIManager._instance.uiMainMenu.BackShowMain();
            UIManager._instance.ResumeGame();
        }
        //SocketCommunication.GetInstance().Send(JsonUtility.ToJson(ev));
        
    }
}