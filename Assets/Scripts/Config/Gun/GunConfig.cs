using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Config/Gun")]
public class GunConfig : ScriptableObject
{
    public List<GunType> lsGunType = new List<GunType>();
}

[System.Serializable]
public class GunType
{
    public int numberOfBullet;
    public float Firerate;
    public int baseDamage;
    public int bulletMultiplier;
    public BulletConfig bulletConfig;
    public float FireRange;
    public GameObject gunPrefab;
    public GameObject bulletPrefab;
    public float baseCritRateMultiplier;
    public float baseCritDMGMultiplier;
}