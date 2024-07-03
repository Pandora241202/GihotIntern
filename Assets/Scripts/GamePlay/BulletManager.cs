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
    public float timer;
    public bool needDelayActive;
    public float delayActiveTime;

    public BulletInfo(Transform obj, Vector3 targetDirection, float bulletSpeed = 5f, bool needDelayActive = false, float delayActiveTime = 0)
    {
        this.bulletObj = obj;
        this.direction = targetDirection.normalized;
        this.speed = bulletSpeed;
        this.timer = 0;
        this.needDelayActive = needDelayActive;
        this.delayActiveTime = delayActiveTime;
        if (needDelayActive)
        {
            bulletObj.gameObject.SetActive(false);
        }
        Setup();
    }

    public void Setup()
    {
        isNeedDestroy = false;
    }

    public void Move()
    {
        if (needDelayActive)
        {
            if (timer >= delayActiveTime)
            {
                bulletObj.gameObject.SetActive(true);
                needDelayActive = false;
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        bulletObj.position += direction * speed * Time.deltaTime;
    }
}
public class BulletManager
{
    public List<BulletInfo> bulletInfoList = new List<BulletInfo>();
    public Dictionary<int, BulletInfo> bulletInfoDict = new Dictionary<int, BulletInfo>();
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
        GameObject bullet;
        for (int i = bulletInfoList.Count - 1; i >= 0; i--)
        {
            if (bulletInfoList[i].isNeedDestroy)
            {
                bullet = bulletInfoList[i].bulletObj.gameObject;
                bulletInfoList.RemoveAt(i);
                bulletInfoDict.Remove(bullet.GetInstanceID());
                GameObject.Destroy(bulletInfoList[i].bulletObj.gameObject);
            }
        }
    }

    public void SetDelete(int id)
    {
        BulletInfo in4;
        if(bulletInfoDict.TryGetValue(id, out in4)) in4.isNeedDestroy = true;
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
}