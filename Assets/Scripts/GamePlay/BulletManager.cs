using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BulletInfo
{
    public Transform bulletObj;
    private float speed = 5f;
    public bool isNeedDestroy;
    private Vector3 direction;

    public BulletInfo(Transform obj, Vector3 targetDirection)
    {
        this.bulletObj = obj;
        this.direction = targetDirection.normalized;
        Setup();
    }

    public void Setup()
    {
        isNeedDestroy = false;
    }

    public void Move()
    {
        bulletObj.position+= direction * speed * Time.deltaTime;
    }
}
public class BulletManager
{
    public List<BulletInfo> bulletInfoList = new List<BulletInfo>();
    public GunConfig bulletConfig;

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

    public void SpawnBullet(Vector3 posSpawn,Vector3 target,int gunId)
    {
        Debug.Log("Spawn Bullet");
        Vector3 direction = (target - posSpawn).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);

        GameObject obj = GameObject.Instantiate(bulletConfig.lsBulletType[gunId].bulletPrefab, posSpawn,Quaternion.identity);
     
        BulletInfo newBullet = new BulletInfo(obj.transform, direction);
        bulletInfoList.Add(newBullet);
        Debug.Log($"Spawned Bullet. Total bullets: {bulletInfoList.Count}");   
    }
}   