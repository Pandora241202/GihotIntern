using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "DroneConfig", menuName = "Config/DroneConfig/DroneConfig")]
public class DroneConfig : ScriptableObject
{
    public GameObject prefDrone;
    public int gunId;
    public GunConfig gunConfig;
    private GameObject curCreepTarget = null;
    public float lastFireTime = 0f;
    GameObject GetTagetObj()
    {
        GunType gunType = gunConfig.lsGunType[gunId];

        Collider[] creepColliders = Physics.OverlapSphere(this.GameObject().transform.position, gunType.FireRange,AllManager._instance.playerConfig.CreepLayerMask);

        GameObject targetObj = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider creepCollider in creepColliders)
        {
            float distance = Vector3.Distance(this.GameObject().transform.position, creepCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                targetObj = creepCollider.gameObject;
            }
        }

        return targetObj;
    }

    public void Shoot()
    {
        GameObject targetObj = GetTagetObj();

        if (targetObj == null)
        {
            return;
        }

        if (targetObj != curCreepTarget)
        {
            curCreepTarget = targetObj;
        }

        // Transform gunTransform = this.GameObject().transform.Find("Gun");
        // Vector3 directionToTarget = (targetObj.transform.position - gunTransform.position).normalized;
        // Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        // gunTransform.rotation = lookRotation;

        float droneDmg = AllManager._instance.playerManager.dictPlayers[Player_ID.MyPlayerID].GetDmg();

        GunType gunType = gunConfig.lsGunType[gunId];

        if (Time.time >= lastFireTime + 1f / gunType.Firerate)
        {
            UIManager._instance.MyPlaySfx(gunId + 1 , 0.5f, 0.15f); //Note: gunId - 1 is VERY temporarily since all audio is in a list in UIManager
            gunType.bulletConfig.Fire(this.GameObject().transform.position, curCreepTarget.transform.position, droneDmg, "PlayerBullet", 
                playerId: Player_ID.MyPlayerID);
            lastFireTime = Time.time;
        }
        
    }

}
