using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BulletInfo
{
    public Transform bulletObj;
    private float speed = 5f;
    public bool isNeedDestroy;
    private Vector3 direction;
    public int damage;

    public BulletInfo(Transform obj, Vector3 targetDirection, float bulletSpeed = 5f)
    {
        this.bulletObj = obj;
        this.direction = targetDirection.normalized;
        this.speed = bulletSpeed;
        Setup();
    }

    public void Setup()
    {
        isNeedDestroy = false;
    }

    public void Move()
    {
        bulletObj.position += direction * speed * Time.deltaTime;
    }
}
public class BulletManager
{
    public List<BulletInfo> bulletInfoList = new List<BulletInfo>();
    public GunConfig gunConfig;
    private float lastFireTime = 0f;

    public void MyUpdate()
    {
        for (int i = 0; i < bulletInfoList.Count; i++)
        {
            bulletInfoList[i].Move();
        }

        for (int i = 0; i < bulletInfoList.Count; i++)
        {
            if (bulletInfoList[i].bulletObj.position.y >= 6)
            {
                bulletInfoList[i].isNeedDestroy = true;
            }
        }
    }

    public void LateUpdate()
    {
        for (int i = bulletInfoList.Count - 1; i >= 0; i--)
        {
            if (bulletInfoList[i].isNeedDestroy)
            {
                //GameObject.Destroy(bulletInfoList[i].bulletObj.gameObject);
                bulletInfoList.RemoveAt(i);
            }
        }
    }

    public void SetDelete(int id)
    {
        foreach (var check in bulletInfoList)
        {
            if (check.bulletObj.gameObject.GetInstanceID() == id)
            {
                check.isNeedDestroy = true;
            }
        }
    }

    public void SpawnBullet(Vector3 posSpawn, Vector3 target, int gunId)
    {
        if (gunConfig == null)
        {
            Debug.LogError("gunConfig is null!");
            return;
        }

        if (gunConfig.lsGunType == null)
        {
            Debug.LogError("gunConfig.lsGunType is null!");
            return;
        }

        if (gunId < 0 || gunId >= gunConfig.lsGunType.Count)
        {
            Debug.LogError($"gunId {gunId} is out of range!");
            return;
        }

        GunType gunType = gunConfig.lsGunType[gunId];

        if (gunType == null)
        {
            Debug.LogError($"GunType at index {gunId} is null!");
            return;
        }

        if (gunType.bulletConfig == null)
        {
            Debug.LogError($"bulletConfig for gunType at index {gunId} is null!");
            return;
        }

        // Check if enough time has passed since the last fire time
        if (Time.time - lastFireTime < 1f / gunType.Firerate)
        {
            Debug.Log("Cannot fire yet. Waiting for fire rate cooldown.");
            return;
        }

        gunType.bulletConfig.Fire(posSpawn, target, this);
        lastFireTime = Time.time;

        Debug.Log($"Spawned Bullet. Total bullets: {bulletInfoList.Count}");
    }
}