using System.Collections;
using System.Collections.Generic;using System.Security.Claims;
using UnityEngine;

public class Drone
{
    public Transform droneTrans;
    public DroneConfig droneConfig;

    public Drone(Transform droneTrans, DroneConfig droneConfig)
    {
        this.droneTrans = droneTrans;
        this.droneConfig = droneConfig;
    }
}

public class DroneManager
{
    public Drone drone;
    public DroneConfig droneConfig;
    public Vector3 posSpawnBullet;
    public Transform playerTrans;
    public float DistanceToPlayer;
    public DroneManager(DroneConfig droneConfig)
    {
        this.droneConfig = droneConfig;
    }

    public void MyUpdate()
    {
        if (AllManager._instance.playerManager.dictPlayers.TryGetValue(Player_ID.MyPlayerID, out var player))
        {
            playerTrans = player.playerTrans;
            if (drone != null && playerTrans != null)
            {
                DistanceToPlayer = Vector3.Distance(drone.droneTrans.position, playerTrans.position);
            }
        }
    }
    public void SpawnDrone()
    {
        Debug.Log("Spawn Drone");
        GameObject goDrone = GameObject.Instantiate(droneConfig.prefDrone);
        goDrone.transform.position = new Vector3(0f, 2.55f, 0);
        drone = new Drone(goDrone.transform, droneConfig);
        posSpawnBullet = drone.droneTrans.position + new Vector3(0, 0.133f, 0.881f);
    }
    GameObject GetTagetObj()
    {
        Debug.Log("Set target");
        GunType gunType = droneConfig.gunConfig.lsGunType[droneConfig.gunId];
        Debug.Log(drone.droneTrans.position);
        Debug.Log(gunType.FireRange);
        Debug.Log(AllManager._instance.playerConfig.CreepLayerMask);
        Collider[] creepColliders = Physics.OverlapSphere(drone.droneTrans.position, gunType.FireRange,AllManager._instance.playerConfig.CreepLayerMask);

        GameObject targetObj = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider creepCollider in creepColliders)
        {
            float distance = Vector3.Distance(drone.droneTrans.position, creepCollider.transform.position);
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
        Debug.Log("Shoot");
        GameObject targetObj = GetTagetObj();

        if (targetObj == null)
        {
            return;
        }

        if (targetObj !=droneConfig.curCreepTarget)
        {
            droneConfig.curCreepTarget = targetObj;
        }

        // Transform gunTransform = this.GameObject().transform.Find("Gun");
        Vector3 directionToTarget = (targetObj.transform.position - drone.droneTrans.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        drone.droneTrans.rotation = lookRotation;

        float droneDmg = AllManager._instance.playerManager.dictPlayers[Player_ID.MyPlayerID].GetDmg();

        GunType gunType = droneConfig.gunConfig.lsGunType[droneConfig.gunId];

        if (Time.time >= droneConfig.lastFireTime + 1f / gunType.Firerate)
        {
            UIManager._instance.MyPlaySfx(droneConfig.gunId + 1 , 0.5f, 0.15f); //Note: gunId - 1 is VERY temporarily since all audio is in a list in UIManager
            gunType.bulletConfig.Fire(posSpawnBullet, droneConfig.curCreepTarget.transform.position, 
                droneDmg, "PlayerBullet", playerId: Player_ID.MyPlayerID);
            droneConfig.lastFireTime = Time.time;
        }
        
    }
}
