using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Config/Gun")]
public class GunConfig : ScriptableObject
{
   public List<GunType> lsBulletType = new List<GunType>();
}

[System.Serializable]
public class GunType
{
   public int Damage;
   public float Firerate;
   public float BulletSpeed;
   public int BulletPerShot;

   public GameObject gunPrefab;
   public GameObject bulletPrefab;
}