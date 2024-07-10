using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CreepConfig : ScriptableObject
{
    [SerializeField] GameObject creepPrefab;

    [SerializeField] int baseHp;

    [SerializeField] float baseSpeed;

    [SerializeField] int baseDmg;

    [SerializeField] int exp;

    [SerializeField] AllDropItemConfig.PowerUpsType[] dropItemTypes;

    public GameObject CreepPrefab => creepPrefab;

    public int BaseHp => baseHp;

    public float BaseSpeed => baseSpeed;

    public int BaseDmg => baseDmg;

    //public float StartSpawnTime => startSpawnTime;

    //public float SpawnRate => spawnRate;

    //public float MinSpawnIntervalTime => minSpawnIntervalTime;

    //public float MaxSpawnIntervalTime => maxSpawnIntervalTime;

    public int Exp => exp;

    public AllDropItemConfig.PowerUpsType[] DropItemTypes => dropItemTypes;

    public virtual void Move(Creep creep) 
    {
        Vector3 normal = Vector3.zero;
        foreach (var pair in creep.collision_plane_normal_dict)
        {
            normal += pair.Value;
        }

        creep.creepTrans.Translate((creep.creepTrans.forward + normal).normalized * creep.speed * Time.deltaTime, Space.World);

        //creepTransform.Translate(creepTransform.forward * creep.speed * Time.deltaTime, Space.World);
    }

    public virtual void Attack(Creep creep) { }

    public virtual void OnDead(Creep creep) 
    {
        AllManager.Instance().creepManager.AddToDeactivateList(creep);
        Debug.Log($"{creep.type} dropped item of type {dropItemTypes[0]}");
        AllManager.Instance().powerUpManager.SpawnPowerUp(creep.creepTrans.position, DropItemTypes[0]);//, AllManager.Instance().powerUpConfig.GetPowerUpPrefab(PowerUpsType.HealthPack));
    }

    float DistanceBetween(Vector3 pos1, Vector3 pos2)
    {
        Vector3 disVector = pos1 - pos2;
        disVector.y = 0;
        return disVector.magnitude;
    }

    protected (string, float) GetNearestPlayerWithDis(Transform creepTransform)
    {
        Dictionary<string, Player> dictPlayers = AllManager.Instance().playerManager.dictPlayers;

        float minDis = DistanceBetween(dictPlayers.First().Value.playerTrans.position, creepTransform.position);
        string playerIdToTarget = dictPlayers.First().Key;

        foreach (var pair in dictPlayers)
        {
            Vector3 playerPos = pair.Value.playerTrans.position;
            float dis = DistanceBetween(playerPos, creepTransform.position);
            if (dis < minDis)
            {
                playerIdToTarget = pair.Key;
                minDis = dis;
            }
        }

        return (playerIdToTarget, minDis);
    }
}