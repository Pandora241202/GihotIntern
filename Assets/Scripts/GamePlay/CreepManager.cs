using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
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

    //public void Move()
    //{
    //    config.Move(this);
    //}

    //public void ProcessDmg(float dmg)
    //{
    //    health -= dmg;
    //    if (health <= 0)
    //    {
    //        EnemyManager enemyManager = AllManager.Instance.enemyManager;
    //        enemyManager.AddToDestroyList(this);
    //    }
    //    enemyTrans.gameObject.transform.localScale = Vector3.one * health;
    //}
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
        creepObj.layer = LayerMask.NameToLayer("Enemy");
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
            //creep.Move();
        }

    }

    //public void LateUpdate()
    //{
    //    for (int i = 0; i < enemyIdsToDestroy.Count; i++)
    //    {
    //        EnemyInfo enemyInfo = GetEnemyInfoById(enemyIdsToDestroy[i]);

    //        if (enemyInfo == null)
    //        {
    //            enemyIdsToDestroy.RemoveAt(i);
    //            continue;
    //        }

    //        AllManager.Instance.effectManager.SpawnEffect(EffectManager.EffectType.EXPLOSION, enemyInfo.enemyTrans.position);
    //        GameObject.Destroy(enemyInfo.enemyTrans.gameObject);
    //        enemyInfoDict.Remove(enemyIdsToDestroy[i]);
    //    }
    //    enemyIdsToDestroy.Clear();
    //}

    //public void ProcessCollision(int enemyId, GameObject colliderObject)
    //{
    //    EnemyInfo enemyInfo = GetEnemyInfoById(enemyId);
    //    if (colliderObject == null)
    //    {
    //        enemyInfo.ProcessDmg(enemyInfo.health);
    //        return;
    //    }
    //    BulletManager bulletManager = AllManager.Instance.bulletManager;
    //    BulletInfo bulletInfo = bulletManager.GetBulletInfoById(colliderObject.GetInstanceID());
    //    enemyInfo.ProcessDmg(bulletInfo.config.Dmg);
    //}

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
