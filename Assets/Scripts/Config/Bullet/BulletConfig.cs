using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletConfig : ScriptableObject
{
    public string bulletName;
    public int damage;
    public float speed;
    public int bulletTimeToLive;
    public float bulletBaseCR;
    public float bulletBaseCD;
    public bool destroyOnContact;
    public int bulletMultiplier;
    public GameObject bulletPrefab;
    public virtual void Fire(Vector3 posSpawn, Vector3 target, BulletManager bulletManager, string tagName)
    {

    }

}