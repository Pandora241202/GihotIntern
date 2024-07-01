using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class AllManager : MonoBehaviour
{
    public static AllManager _instance { get; private set; }
    public BulletManager bulletManager;
    public PlayerManager playerManager;
    public CreepManager creepManager;
    public GunConfig gunConfig;
    [SerializeField] AllCreepConfig allCreepConfig;

    public static AllManager Instance()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindAnyObjectByType<AllManager>();
        }

        return _instance;
    }
    private void Start() {
        bulletManager = new BulletManager();
        playerManager = new PlayerManager();
        creepManager = new CreepManager(allCreepConfig);
        bulletManager.gunConfig = gunConfig;
    }
    private void Update() {
        bulletManager.MyUpdate();
        creepManager.MyUpdate();
    }
    private void LateUpdate() {
        bulletManager.LateUpdate();
        creepManager.LateUpdate();
    }
}
