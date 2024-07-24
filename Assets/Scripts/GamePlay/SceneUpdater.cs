using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneUpdater : MonoBehaviour
{
    public CreepManager creepManager;
    public BulletManager bulletManager;
    public PowerUpManager powerUpManager;
    public PlayerManager playerManager;
    public GameEventManager gameEventManager;

    public DroneManager droneManager;
    // Start is called before the first frame update
    void Start()
    {
        creepManager = new CreepManager(AllManager.Instance().allCreepConfig);
        bulletManager = new BulletManager(AllManager.Instance().gunConfig);
        powerUpManager = new PowerUpManager(AllManager.Instance().allDropItemConfig);
        gameEventManager = new GameEventManager(AllManager.Instance().allGameEventConfig);
        droneManager = new DroneManager(AllManager.Instance().droneConfig);
        playerManager = AllManager.Instance().playerManager;
        if(playerManager.dictPlayers.Count==1) droneManager.SpawnDrone();
    }

    // Update is called once per frame
    void Update()
    {
        if (AllManager.Instance().isPause) return;
        bulletManager.MyUpdate();
        creepManager.MyUpdate();
        powerUpManager.MyUpdate();
        playerManager.MyUpdate();
        gameEventManager.MyUpdate();
        droneManager.MyUpdate();
    }

    private void FixedUpdate()
    {
        playerManager.MyFixedUpdate();
    }

    private void LateUpdate()
    {
        if (AllManager.Instance().isPause) return;
        bulletManager.LateUpdate();
        creepManager.LateUpdate();
        powerUpManager.LateUpdate();
        playerManager.LateUpdate();
    }
}
