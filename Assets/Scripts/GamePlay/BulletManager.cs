using System.Collections.Generic;
using UnityEngine;

public class BulletInfo
{
    public Transform bulletObj;
    private float speed = 5f;
    public bool isNeedDestroy;
    private Vector3 direction;
    public float damage;
    public float timer;
    public bool needDelayActive;
    public float delayActiveTime;
    public string playerId;
    public float timeToLive;

    public BulletInfo(Transform obj, Vector3 targetDirection, float damage, float timeToLive, float bulletSpeed = 5f, bool needDelayActive = false, float delayActiveTime = 0, string playerId = null)
    {
        this.bulletObj = obj;
        this.direction = targetDirection.normalized;
        this.speed = bulletSpeed;
        this.timer = 0;
        this.needDelayActive = needDelayActive;
        this.delayActiveTime = delayActiveTime;
        this.damage = damage;
        this.playerId = playerId;
        this.timeToLive = timeToLive;
        if (needDelayActive)
        {
            bulletObj.gameObject.SetActive(false);
        }
        Setup();
        this.playerId = playerId;
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
        }

        bulletObj.position += direction * speed * Time.deltaTime;

        // Destroy bullet when out of map range or life time >= time to live
        // With time to live
        if (timer >= timeToLive)
        {
            isNeedDestroy = true;
            timer = 0;
        } else
        {
            timer += Time.deltaTime;
        }

        // With map range
        if (bulletObj.position.x < Constants.MapMinX || bulletObj.position.x > Constants.MapMaxX || bulletObj.position.y < Constants.MapMinY || bulletObj.position.z < Constants.MapMinZ  || bulletObj.position.z > Constants.MapMaxZ)
        {
            isNeedDestroy = true;
        }
    }
}
public class BulletManager
{
    public List<BulletInfo> bulletInfoList = new List<BulletInfo>();
    public Dictionary<int, BulletInfo> bulletInfoDict = new Dictionary<int, BulletInfo>();
    public GunConfig gunConfig;
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

        // Transform bullet;
        // for (int i = 0; i < bulletInfoList.Count; i++)
        // {
        //     bullet = bulletInfoList[i].bulletObj;
        //     if (bullet.position.x >= 100 || bullet.position.x <= -100 || bullet.position.z >= 100 || bullet.position.z <= -100)
        //     {
        //         bulletInfoList[i].isNeedDestroy = true;
        //     }
        // }
    }

    public void LateUpdate()
    {
        GameObject bullet;
        for (int i = bulletInfoList.Count - 1; i >= 0; i--)
        {
            if (bulletInfoList[i].isNeedDestroy)
            {
                bullet = bulletInfoList[i].bulletObj.gameObject;
                GameObject.Destroy(bullet);
                bulletInfoList.RemoveAt(i);
                bulletInfoDict.Remove(bullet.GetInstanceID());
            }
        }
        //Debug.Log(bulletInfoDict.Count);
    }

    public void SetDelete(int id)
    {
        BulletInfo in4;
        if(bulletInfoDict.TryGetValue(id, out in4)) 
            in4.isNeedDestroy = true;
    }
}