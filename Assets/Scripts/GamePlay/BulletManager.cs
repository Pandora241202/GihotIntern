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
    private float localFireRate;
    private float lastFireTime = 0f;
    private int gunId = 0; //TODO: receive gunID from player
    public GameObject target;

    public BulletManager(GunConfig config)
    {
        this.gunConfig = config;
    }

    public void SetGunId(int id)
    {
        gunId = id;
        Debug.Log("SetGunId in BulletManager called, newGunId = " + gunId);
    }
    public int GetGunId(){
        Debug.Log("GetGunId in BulletManager called, gunId = " + gunId);
        return gunId;
    }
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
        //GunType gunType = gunConfig.lsGunType[gunId];
        //localFireRate = gunType.Firerate;
        //if (target && Time.time >= lastFireTime + 1f / localFireRate)
        //{
        //    SpawnBullet(CharacterController.Instance().gunTransform.position, target, gunId);
        //    lastFireTime = Time.time;
        //}
    }

    public void LateUpdate()
    {
        for (int i = bulletInfoList.Count - 1; i >= 0; i--)
        {
            if (bulletInfoList[i].isNeedDestroy)
            {
                GameObject.Destroy(bulletInfoList[i].bulletObj.gameObject);
                Debug.Log("Destroy bullet");
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

    public float SpawnBullet(Vector3 posSpawn, GameObject target, int gunId, float lastFireTime, string tagName)
    {
        GunType gunType = gunConfig.lsGunType[gunId];
        localFireRate = gunType.Firerate;
        if (target && Time.time >= lastFireTime + 1f / localFireRate)
        {
            gunType.bulletConfig.Fire(posSpawn, target.transform.position, this, tagName);
            return Time.time;
        }
        return lastFireTime;
    }
    public void SetTarget(GameObject target)
    {
        Debug.Log("SetTarget");
        this.target = target;
    }
}