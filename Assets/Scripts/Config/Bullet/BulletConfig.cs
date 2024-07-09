using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletConfig : ScriptableObject
{
    public string bulletName;
    public float speed;
    public int bulletTimeToLive;
    public float bulletBaseCR;
    public float bulletBaseCD;
    public bool destroyOnContact;
    public GameObject bulletPrefab;
    public virtual void Fire(Vector3 posSpawn, Vector3 target, int dmg, BulletManager bulletManager, string tagName, bool needDelayActive = false, float delayActiveTime = 0, string playerId = null)
    {

    }

}