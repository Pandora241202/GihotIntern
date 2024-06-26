using System.Collections.Generic;
using UnityEngine;

public class Creep
{
    public Transform creepTrans;
    public CreepConfig config;
    public int hp;
    public float speed;
    public int dmg;

    public Creep(Transform creepTrans, CreepConfig config, float time)
    {
        this.creepTrans = creepTrans;
        this.config = config;
        hp = config.BaseHp + (int) (time / 60) * 5;
        dmg = config.BaseDmg + (int)(time / 60) * 2;
        speed = config.BaseSpeed;
    }

    public void Move()
    {
        config.Move(creepTrans, speed);
    }

    public void Attack()
    {
        config.Attack(this);
    }

    public void OnDead()
    {
        config.OnDead(creepTrans);
    }

    public void ProcessDmg(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            CreepManager creepManager = AllManager._instance.creepManager;
            creepManager.AddToDestroyList(this);
        }
    }
}

public class CreepManager
{
    private Dictionary<int, Creep> creepDict = new Dictionary<int, Creep>();
    private List<int> creepIdsToDestroy = new List<int>();
    private CreepConfig[] creepConfigs;

    public CreepManager(AllCreepConfig allCreepConfig)
    {
        creepConfigs = allCreepConfig.CreepConfigs;
    }

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

    public CreepConfig GetCreepConfigByType(CreepType type)
    {
        return creepConfigs[(int)type];
    }

    public void AddToDestroyList(Creep creep)
    {
        creepIdsToDestroy.Add(creep.creepTrans.gameObject.GetInstanceID());
    }

    public void SpawnCreep(Vector3 spawnPos, CreepConfig config, float time)
    {
        GameObject creepObj = GameObject.Instantiate(config.CreepPrefab, spawnPos, Quaternion.identity);
        Creep creep = new Creep(creepObj.transform, config, time);
        creepDict.Add(creepObj.GetInstanceID(), creep);
    }

    public Creep GetCreepById(int id)
    {
        Creep creep;
        if (creepDict.TryGetValue(id, out creep))
        {
            return creep;
        }
        return null;
    }

    public void MyUpdate()
    {
        foreach (var pair in creepDict)
        {
            Creep creep = pair.Value;
            creep.Attack();
            creep.Move();
        }

    }

    public void LateUpdate()
    {
        for (int i = 0; i < creepIdsToDestroy.Count; i++)
        {
            Creep creep = GetCreepById(creepIdsToDestroy[i]);

            if (creep == null)
            {
                creepIdsToDestroy.RemoveAt(i);
                continue;
            }

            //AllManager.Instance.effectManager.SpawnEffect(EffectManager.EffectType.EXPLOSION, enemyInfo.enemyTrans.position);
            creep.OnDead();
            GameObject.Destroy(creep.creepTrans.gameObject);
            creepDict.Remove(creepIdsToDestroy[i]);
        }
        creepIdsToDestroy.Clear();
    }

    public void ProcessCollision(int creepId, GameObject colliderobject)
    {
        //TODO: Bullet Dmg
        //TODO: Sync 
        int dmg = 5;
        Creep creep = GetCreepById(creepId);
        creep.ProcessDmg(dmg);
    }

    //public void Reset()
    //{
    //    enemyIdsToDestroy.Clear();
    //    enemyInfoDict.Clear();
    //}

    //public void DeleteAllObj()
    //{
    //    foreach (var pair in enemyInfoDict)
    //    {
    //        AddToDestroyList(pair.Value);
    //        for (int i = 0; i < enemyConfigs.Length; i++)
    //        {
    //            timeCountForSpawnEnemys[i] = 0;
    //        }
    //    }
    //}

    //public bool isClean()
    //{
    //    return needSpawnedNums.All(n => n == 0) && enemyInfoDict.Count == 0;
    //}
}
