using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class AllManager : MonoBehaviour
{
    public static AllManager _instance { get; private set; }
    public BulletManager bulletManager;
    public PlayerManager playerManager;
    [SerializeField] private GunConfig bulletConfig;
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
        bulletManager.bulletConfig = bulletConfig;
    }
    private void Update() {
        bulletManager.MyUpdate();

    }
    private void LateUpdate() {
        bulletManager.LateUpdate();
    }
}
