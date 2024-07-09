using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Creep
{
    public Transform creepTrans;
    public CreepConfig config;
    public CreepManager.CreepType type;
    public int hp;
    public float speed;
    public int dmg;
    public float timer;
    public GameObject weaponObj;
    public Animator animator;
    public int sharedId;
    public Dictionary<int, Vector3> collision_plane_normal_dict = new Dictionary<int, Vector3>();

    public Creep(Transform creepTrans, CreepManager.CreepType type, CreepConfig config, int sharedId)
    {
        this.creepTrans = creepTrans;
        this.config = config;
        this.type = type;
        weaponObj = null;
        animator = creepTrans.gameObject.GetComponent<Animator>();
        creepTrans.gameObject.SetActive(false);
        this.sharedId = sharedId;
    }

    public void Set(Vector3 pos, float time)
    {
        hp = config.BaseHp + (int)(time / 60) * 5;
        dmg = config.BaseDmg + (int)(time / 60) * 2;
        speed = config.BaseSpeed;
        creepTrans.position = pos;
        creepTrans.gameObject.SetActive(true);
  
        timer = 0;
    }

    public void UnSet() 
    {
        creepTrans.gameObject.SetActive(false);
    }

    public void Move()
    {
        config.Move(this);
    }

    public void Attack()
    {
        config.Attack(this);
    }

    public void OnDead()
    {
        AllManager.Instance().playerManager.ProcessExpGain(this.config.Exp);
        CreepManager creepManager = AllManager.Instance().creepManager;
        config.OnDead(this);
    }

    public void ProcessDmg(int dmg, string bulletOwnerId)
    {
        hp -= dmg;
        animator.SetTrigger("isTakeDmg");
        if (hp <= 0 && bulletOwnerId == Player_ID.MyPlayerID)
        {
            // Send to server creep destroy
            SendData<CreepDestroyInfo> data = new SendData<CreepDestroyInfo>(new CreepDestroyInfo(this.sharedId));
            SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
        }
    }
}

public class CreepManager
{
    Dictionary<int, Creep> creepActiveDict = new Dictionary<int, Creep>();
    List<Creep>[] creepNotActiveByTypeList = new List<Creep>[7];
    List<int> allCreeps = new List<int>(); // used to store creep id share between players and server and map to instanceId
    private List<int> creepIdsToDeactivate = new List<int>();
    //private List<CreepSpawnInfo> needSpawnCreeps = new List<CreepSpawnInfo>();
    private CreepConfig[] creepConfigs;

    public enum CreepType
    {
        Creep1,
        Creep2, 
        Creep3,
        Creep4,
        Creep5,
        Creep6,
        Creep7
    }

    private void SpawnCreep(CreepType creepType)
    {
        CreepConfig config = GetCreepConfigByType(creepType);
        GameObject creepObj = GameObject.Instantiate(config.CreepPrefab);
        creepObj.layer = LayerMask.NameToLayer("creep");
        Creep creep = new Creep(creepObj.transform, creepType, config, allCreeps.Count);
        creepNotActiveByTypeList[(int) creepType].Add(creep);
        allCreeps.Add(creepObj.GetInstanceID());
    }

    public CreepManager(AllCreepConfig allCreepConfig)
    {
        creepConfigs = allCreepConfig.CreepConfigs;
        
        for (int i = 0; i < creepNotActiveByTypeList.Length; i++) 
        {
            creepNotActiveByTypeList[i] = new List<Creep>();
        }

        for (CreepType type = CreepType.Creep1; type <= CreepType.Creep7; type++)
        {
            for (int i = 0; i < Constants.MaxCreepForEachType; i++)
            {
                SpawnCreep(type);
            }
        }
    }

    public CreepConfig GetCreepConfigByType(CreepType type)
    {
        return creepConfigs[(int)type];
    }

    public void AddToDeactivateList(Creep creep)
    {
        creepIdsToDeactivate.Add(creep.creepTrans.gameObject.GetInstanceID());
    }

    public void ActivateCreep(Vector3 spawnPos, CreepType creepType, float time)
    {
        // All creep of same type have been activated => Spawn new
        if (creepNotActiveByTypeList[(int)creepType].Count == 0)
        {
            SpawnCreep(creepType);
        }

        Creep creepNeedActive = creepNotActiveByTypeList[(int)creepType][0];
            
        creepNeedActive.Set(spawnPos, time/1000f);
        creepActiveDict.Add(creepNeedActive.creepTrans.gameObject.GetInstanceID(), creepNeedActive);
            
        creepNotActiveByTypeList[(int)creepType].RemoveAt(0);
    }

    public Creep GetActiveCreepById(int id)
    {
        Creep creep;
        if (creepActiveDict.TryGetValue(id, out creep))
        {
            return creep;
        }
        return null;
    }

    public int MapSharedIdToInstanceId(int sharedId)
    {
        return allCreeps[sharedId];
    }

    public void SendCreepToDeadBySharedId(int sharedId)
    {
        int id = MapSharedIdToInstanceId(sharedId);
        Creep creep = GetActiveCreepById(id);

        if (creep == null)
        {
            return;
        }

        creep.OnDead();
    }

    public void MyUpdate()
    {
        foreach (var pair in creepActiveDict)
        {
            Creep creep = pair.Value;
            creep.Attack();
            creep.Move();
        }
    }

    private void DeactivateCreep(Creep creep)
    {
        creep.UnSet();
        creepNotActiveByTypeList[(int) creep.type].Add(creep);
        creepActiveDict.Remove(creep.creepTrans.gameObject.GetInstanceID());
    }

    public void LateUpdate()
    {
        for (int i = 0; i < creepIdsToDeactivate.Count; i++)
        {
            Creep creep = GetActiveCreepById(creepIdsToDeactivate[i]);

            if (creep == null)
            {
                creepIdsToDeactivate.RemoveAt(i);
                continue;
            }

            //AllManager.Instance.effectManager.SpawnEffect(EffectManager.EffectType.EXPLOSION, enemyInfo.enemyTrans.position);

            DeactivateCreep(creep);
        }

        creepIdsToDeactivate.Clear();
    }

    public void ProcessCollisionPlayerBullet(int creepId, GameObject colliderObject)
    {
        Creep creep = GetActiveCreepById(creepId);
        BulletInfo bulletInfo = AllManager.Instance().bulletManager.bulletInfoDict[colliderObject.GetInstanceID()];
        creep.ProcessDmg(bulletInfo.damage, bulletInfo.playerId);
        AllManager.Instance().bulletManager.SetDelete(colliderObject.GetInstanceID());
    }

    public void ProcessCollisionMapElement(int creepId, Collider collider)
    {
        Creep creep = GetActiveCreepById(creepId);
        int mapElementId = collider.gameObject.GetInstanceID();
        
        if (creep.collision_plane_normal_dict.ContainsKey(mapElementId)) return;

        Vector3 collidePoint = collider.ClosestPoint(creep.creepTrans.position);
        collidePoint.y = creep.creepTrans.position.y;

        creep.collision_plane_normal_dict.Add(mapElementId, (creep.creepTrans.position - collidePoint).normalized);
    }

    public void MarkTargetCreepById(int creepId)
    {
        Creep creep = GetActiveCreepById(creepId);

        if (creep == null)
        {
            return;
        }
    }

    public void UnmarkTargetCreepById(int creepId)
    {
        Creep creep = GetActiveCreepById(creepId);

        if (creep == null)
        {
            return;
        }

    }
}
