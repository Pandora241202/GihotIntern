using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/BulletConfig/LightPistolBullet")]
public class LightPistolBullet : BulletConfig
{
    public override void Fire(Vector3 posSpawn, Vector3 target, int dmg, BulletManager bulletManager, string tagName, bool needDelayActive = false, float delayActiveTime = 0, string playerId = null)
    {
        Vector3 direction = (target - posSpawn).normalized;
        GameObject obj = GameObject.Instantiate(bulletPrefab, posSpawn, Quaternion.identity);
        obj.tag = tagName;
        // obj.transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
        obj.transform.Translate(direction*0.5f);
        BulletInfo newBullet = new BulletInfo(obj.transform, direction, dmg, speed, needDelayActive, delayActiveTime, playerId);
        bulletManager.bulletInfoList.Add(newBullet);
        bulletManager.bulletInfoDict.Add(obj.GetInstanceID(), newBullet);
    }
}