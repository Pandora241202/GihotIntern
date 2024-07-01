using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AllManager : MonoBehaviour
{
    public static AllManager _instance { get; private set; }
    public BulletManager bulletManager;
    public PlayerManager playerManager;
    public CreepManager creepManager;

    public GunConfig gunConfig;
    [SerializeField] AllCreepConfig allCreepConfig;
    [SerializeField] GameObject characterPrefab;

    public static AllManager Instance()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindAnyObjectByType<AllManager>();
        }

        return _instance;
    }
    private void Start() {
        playerManager = new PlayerManager(characterPrefab);
        DontDestroyOnLoad(this);
    }
    private void Update() {
        //bulletManager.MyUpdate();
        //creepManager.MyUpdate();
    }
    private void LateUpdate() {
        //bulletManager.LateUpdate();
        //creepManager.LateUpdate();
    }

    public void LoadGame(string sceneName)
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
        OnSceneLoaded();
        Debug.Log("Scene loaded!");

    }

    private void OnSceneLoaded()
    {
        SendData<EventName> ev = new SendData<EventName>(new EventName("done loading"));
        SocketCommunication.GetInstance().Send(JsonUtility.ToJson(ev));
        UIManager._instance.uiMainMenu.gameObject.SetActive(false);
        creepManager = new CreepManager(allCreepConfig);
        bulletManager = new BulletManager();
    }
}
