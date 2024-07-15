using System.Collections.Generic;
using UnityEngine;

public class Creep
{
    public Transform creepTrans;
    public CreepConfig config;
    public CreepManager.CreepType type;
    public int sharedId;

    public int hp;
    public float speed;
    public int dmg;

    public int maxHp;

    public float timer;
    public GameObject weaponObj;
    public Animator animator;
 
    public Dictionary<int, Vector3> collision_plane_normal_dict = new Dictionary<int, Vector3>();

    public Creep(Transform creepTrans, CreepManager.CreepType type, CreepConfig config)
    {
        this.creepTrans = creepTrans;
        this.config = config;
        this.type = type;
        weaponObj = null;
        animator = creepTrans.gameObject.GetComponent<Animator>();
        creepTrans.gameObject.SetActive(false);
        this.sharedId = -1;
    }

    public void Set(Vector3 pos, float time, int shared_id)
    {
        hp = config.BaseHp + (int)(time / 60) * 5;
        dmg = config.BaseDmg + (int)(time / 60) * 2;
        speed = config.BaseSpeed;
        maxHp = config.BaseHp + (int)(time / 60) * 5;

        this.sharedId = shared_id;

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
        this.sharedId = -1;
        config.OnDead(this);
    }

    public void ProcessDmg(int dmg, string bulletOwnerId)
    {
        hp -= dmg;
        animator.SetTrigger("isTakeDmg");
        if (hp <= 0 && bulletOwnerId == Player_ID.MyPlayerID)
        {
            // Determine item drop
            AllDropItemConfig.PowerUpsType? droppedPowerUp = config.DetermineDrop();
            PowerUpSpawnInfo powerUpSpawnInfo = null;
            if (droppedPowerUp != null)
            {
                powerUpSpawnInfo = new PowerUpSpawnInfo((AllDropItemConfig.PowerUpsType) droppedPowerUp, creepTrans.position);
            }

            // Send to server creep destroy
            SendData<CreepDestroyInfo> data = new SendData<CreepDestroyInfo>(new CreepDestroyInfo(this.sharedId, powerUpSpawnInfo));
            SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
        }
    }
}

public class CreepManager
{
    Dictionary<int, Creep> creepActiveDict = new Dictionary<int, Creep>();
    List<Creep>[] creepNotActiveByTypeList = new List<Creep>[7];
    Dictionary<int, int> creepSharedIdDict = new Dictionary<int, int>(); // used to store creep id share between players and server and map to instanceId
    private List<int> creepIdsToDeactivate = new List<int>();
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
    public Dictionary<int, Creep> GetCreepActiveDict()
    {
        return creepActiveDict;
    }
    private void SpawnCreep(CreepType creepType)
    {
        CreepConfig config = GetCreepConfigByType(creepType);
        GameObject creepObj = GameObject.Instantiate(config.CreepPrefab);
        creepObj.layer = LayerMask.NameToLayer("creep");
        Creep creep = new Creep(creepObj.transform, creepType, config);
        creepNotActiveByTypeList[(int) creepType].Add(creep);
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

    public Creep GetRandomHurtCreep()
    {
        foreach (var pair in creepActiveDict)
        {
            Creep creep = pair.Value;
            if (creep.hp < creep.maxHp)
            {
                return creep;
            }
        }

        return null;
    }

    public CreepConfig GetCreepConfigByType(CreepType type)
    {
        return creepConfigs[(int)type];
    }

    public void AddToDeactivateList(Creep creep)
    {
        creepIdsToDeactivate.Add(creep.creepTrans.gameObject.GetInstanceID());
    }

    public void ActivateCreep(Vector3 spawnPos, CreepType creepType, float time, int sharedId)
    {
        // All creep of same type have been activated => Spawn new
        if (creepNotActiveByTypeList[(int)creepType].Count == 0)
        {
            SpawnCreep(creepType);
        }

        Creep creepNeedActive = creepNotActiveByTypeList[(int)creepType][0];
            
        creepNeedActive.Set(spawnPos, time/1000f, sharedId);
        creepActiveDict.Add(creepNeedActive.creepTrans.gameObject.GetInstanceID(), creepNeedActive);
        creepSharedIdDict.Add(sharedId, creepNeedActive.creepTrans.gameObject.GetInstanceID());
            
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
        return creepSharedIdDict[sharedId];
    }

    public void SendCreepToDeadBySharedId(int sharedId)
    {
        int id = MapSharedIdToInstanceId(sharedId);
        Creep creep = GetActiveCreepById(id);

        if (creep == null)
        {
            return;
        }

        creepSharedIdDict.Remove(sharedId);

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

    //public void MarkTargetCreepById(int creepId)
    //{
    //    Creep creep = GetActiveCreepById(creepId);

    //    if (creep == null)
    //    {
    //        return;
    //    }
    //}

    //public void UnmarkTargetCreepById(int creepId)
    //{
    //    Creep creep = GetActiveCreepById(creepId);

    //    if (creep == null)
    //    {
    //        return;
    //    }
    //}

    public void UpdateCreepsState(CreepSpawnInfo[] creepSpawnInfos, CreepDestroyInfo[] creepDestroyInfos)
    {
        if (creepSpawnInfos != null)
        {
            foreach (CreepSpawnInfo creepSpawnInfo in creepSpawnInfos)
            {
                ActivateCreep(creepSpawnInfo.spawn_pos, (CreepManager.CreepType)creepSpawnInfo.type_int, creepSpawnInfo.time, creepSpawnInfo.shared_id);
            }
        }

        if (creepDestroyInfos != null)
        {
            Debug.Log("creepDestroyInfos: " + creepDestroyInfos.ToString());
            foreach (CreepDestroyInfo creepDestroyInfo in creepDestroyInfos)
            {
                SendCreepToDeadBySharedId(creepDestroyInfo.shared_id);
                
                PowerUpSpawnInfo powerUpSpawnInfo = creepDestroyInfo.power_up_spawn_info;
                if (powerUpSpawnInfo != null)
                {
                    AllManager.Instance().powerUpManager.SpawnPowerUp(powerUpSpawnInfo.spawn_pos, (AllDropItemConfig.PowerUpsType)powerUpSpawnInfo.type_int, powerUpSpawnInfo.shared_id);
                }
            }
        }
    }
}
