using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BulletInfo
{
    public Transform bulletObj;
    private float speed = 5f;
    public bool isNeedDestroy;
    private Vector3 direction;

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
    public GunConfig bulletConfig;
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
        GunType gunType = bulletConfig.lsGunType[gunId];
        // Check if enough time has passed since the last fire time
        if (Time.time - lastFireTime < 1f / gunType.Firerate)
        {
            Debug.Log("Cannot fire yet. Waiting for fire rate cooldown.");
            return;
        }
        Debug.Log("Spawn Bullet");
        Debug.Log("GunList count:  " + bulletConfig.lsGunType.Count);
        Vector3 direction = (target - posSpawn).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);

        GameObject obj = GameObject.Instantiate(gunType.bulletPrefab, posSpawn, Quaternion.identity);

        BulletInfo newBullet = new BulletInfo(obj.transform, direction);
        bulletInfoList.Add(newBullet);

        Debug.Log($"Spawned Bullet. Total bullets: {bulletInfoList.Count}");
    }
}