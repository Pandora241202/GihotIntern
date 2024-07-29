using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
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
    public float maxDis=10f;
    public float maxDisFind;

    public DroneManager(DroneConfig droneConfig)
    {
        this.droneConfig = droneConfig;
    }

    public void MyUpdate()
    {
        if(drone==null)return;
        playerTrans = AllManager._instance.playerManager.dictPlayers[Player_ID.MyPlayerID].playerTrans;
        MoveToTarget();
    }

    public void SpawnDrone()
    {
        Debug.Log("Spawn Drone");
        GameObject goDrone = GameObject.Instantiate(droneConfig.prefDrone);
        goDrone.transform.position = new Vector3(0f, 2.55f, 0);
        drone = new Drone(goDrone.transform, droneConfig);
        posSpawnBullet = goDrone.transform.Find("posShoot").transform.position;
        maxDisFind = maxDis + droneConfig.gunConfig.lsGunType[droneConfig.gunId].FireRange;
    }

    public void MoveToTarget()
    {
        
        GameObject target = GetTargetObj(maxDisFind);
        DistanceToPlayer = Vector3.Distance(drone.droneTrans.position, playerTrans.position);
        if (DistanceToPlayer >=maxDis)
        {
            Vector3 directionToPlayer = (playerTrans.position - drone.droneTrans.position).normalized;
            drone.droneTrans.position += directionToPlayer * 10f * Time.deltaTime;
            if (target == null)
            {
               
                drone.droneTrans.rotation =Quaternion.LookRotation(directionToPlayer);
            }

          
        }
       
        if (target != null)
        {
            // Check if the target is within the firing range
            if (!IsTargetWithinRange(target, droneConfig.gunConfig.lsGunType[droneConfig.gunId].FireRange*.7f))
            {
                // Move the drone towards the target
                Vector3 directionToTarget = (target.transform.position - drone.droneTrans.position).normalized;
                drone.droneTrans.position += directionToTarget * 10f * Time.deltaTime;
                Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                drone.droneTrans.rotation = lookRotation;
                
            }
        }
    }
    
    private bool IsTargetWithinRange(GameObject target, float range)
    {
        float distance = Vector3.Distance(drone.droneTrans.position, target.transform.position);
        return distance <= range;
    }
    GameObject GetTargetObj(float range)
    {
        Debug.Log("Set target");
        GunType gunType = droneConfig.gunConfig.lsGunType[droneConfig.gunId];
        Player me = AllManager._instance.playerManager.dictPlayers[Player_ID.MyPlayerID];
        Collider[] creepColliders = Physics.OverlapSphere(me.playerTrans.position, range, AllManager._instance.playerConfig.CreepLayerMask);

        GameObject targetObj = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider creepCollider in creepColliders)
        {
            float distance = Vector3.Distance(me.playerTrans.position, creepCollider.transform.position);
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
        GameObject targetObj = GetTargetObj(maxDisFind);

        if (targetObj == null)
        {
            return;
        }

        if (targetObj != droneConfig.curCreepTarget)
        {
            droneConfig.curCreepTarget = targetObj;
        }

        // Transform gunTransform = this.GameObject().transform.Find("Gun");
        Vector3 directionToTarget = (targetObj.transform.position - drone.droneTrans.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        drone.droneTrans.rotation = lookRotation;

        float droneDmg = AllManager._instance.playerManager.dictPlayers[Player_ID.MyPlayerID].GetDmg();

        GunType gunType = droneConfig.gunConfig.lsGunType[droneConfig.gunId];

        if (Vector3.Distance(drone.droneTrans.position,targetObj.transform.position)<=gunType.FireRange)
        {
            posSpawnBullet = drone.droneTrans.Find("posShoot").transform.position;
            UIManager._instance.MyPlaySfx(droneConfig.gunId + 1, 0.5f,
                0.15f); //Note: gunId - 1 is VERY temporarily since all audio is in a list in UIManager
            Debug.Log("fire");
            gunType.bulletConfig.Fire(posSpawnBullet, droneConfig.curCreepTarget.transform.position,
                droneDmg, "PlayerBullet", playerId: Player_ID.MyPlayerID);
            droneConfig.lastFireTime = Time.time;
        }
        AllManager._instance.playerManager.ProcessLifeSteal();
    }
}