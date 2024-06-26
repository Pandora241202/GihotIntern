using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Config",fileName = "Bullet")]
public class BulletConfig : ScriptableObject
{
   public List<BulletType> lsBulletType = new List<BulletType>();
}

[System.Serializable]
public class BulletType
{
   public int data;
}