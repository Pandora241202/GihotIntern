using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class ShootNearest : MonoBehaviour
{
    public GunConfig gunConfig;
    public GunType gunType;
    public Player player;
    private float searchRadius = 0f;
    public int maxColliders = 10;
    private ITarget currentTarget;
    public int currentGunId; //TODO: coroutine?
    public GameObject currentGunPrefab;
    public float currentFireRate;

    private void Start()
    {
        // currentGunId = AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].gunId;
        // gunConfig = AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].gunConfig.lsGunType[currentGunId];
        currentGunId = AllManager.Instance().bulletManager.GetGunId();
        Debug.Log("startGunId: " + currentGunId);
        gunConfig = Resources.Load<GunConfig>("Configs/Gun/GunConfig");
        gunType = gunConfig.lsGunType[currentGunId];
        currentGunPrefab = gunType.gunPrefab;
        GameObject gun = Instantiate(currentGunPrefab, transform.position, Quaternion.identity);
        gun.transform.SetParent(transform);
    }
}