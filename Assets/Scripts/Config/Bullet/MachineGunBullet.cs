using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/BulletConfig/MachineGunBullet")]
public class MachineGunBullet : BulletConfig
{
    public override void Fire(Vector3 posSpawn, Vector3 target, float dmg, string tagName, bool needDelayActive = false, float delayActiveTime = 0, string playerId = null)
    {
        BulletManager bulletManager = AllManager.Instance().bulletManager;
        Vector3 direction = (target - posSpawn).normalized;
        GameObject obj = GameObject.Instantiate(bulletPrefab, posSpawn, Quaternion.identity);
        obj.tag = tagName;
        obj.transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 0, 0);
        BulletInfo newBullet = new BulletInfo(obj.transform, direction, dmg, bulletTimeToLive, speed, needDelayActive, delayActiveTime, playerId);
        bulletManager.bulletInfoList.Add(newBullet);
        bulletManager.bulletInfoDict.Add(obj.GetInstanceID(), newBullet);
    }
}