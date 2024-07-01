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
    public int currentGunId = AllManager.Instance().bulletManager.GetGunId(); //TODO: coroutine?
    public GameObject currentGunPrefab;
    public float currentFireRate;

    private void Start()
    {
        // currentGunId = AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].gunId;
        // gunConfig = AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].gunConfig.lsGunType[currentGunId];
        Debug.Log("startGunId: " + currentGunId);
        gunConfig = Resources.Load<GunConfig>("Configs/Gun/GunConfig");
        gunType = gunConfig.lsGunType[currentGunId];
        currentGunPrefab = gunType.gunPrefab;
        GameObject gun = Instantiate(currentGunPrefab, transform.position, Quaternion.identity);
        gun.transform.SetParent(transform);
    }
    
    private void Update()
    {
        // currentGunId = AllManager.Instance().bulletManager.GetGunId();
        Debug.Log("currentGunId after update in shootnearest: " + currentGunId);
        gunType = gunConfig.lsGunType[currentGunId];
        searchRadius = gunType.FireRange;
        currentFireRate = gunType.Firerate;
        FindInRadius();
    }

    void FindInRadius()
    {
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, searchRadius, hitColliders);
        float closestDistance = Mathf.Infinity;
        ITarget closestTarget = null;

        for (int i = 0; i < numColliders; i++)
        {
            if (hitColliders[i].TryGetComponent<ITarget>(out ITarget target))
            {
                float distance = Vector3.Distance(transform.position, hitColliders[i].transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }
        }

        if (currentTarget != closestTarget)
        {
            if (currentTarget != null)
            {
                currentTarget.UnTarget();
            }
            currentTarget = closestTarget;
            if (currentTarget != null)
            {
                currentTarget.Target();
                Debug.Log("Targeting: " + currentTarget);
            }
        }
    }
}