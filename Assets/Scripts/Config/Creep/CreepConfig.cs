using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreepConfig : ScriptableObject
{
    [SerializeField] GameObject creepPrefab;

    [SerializeField] int baseHp;

    [SerializeField] float baseSpeed;

    [SerializeField] int baseDmg;

    //[SerializeField] float startSpawnTime;

    //[SerializeField] float spawnRate;

    //[SerializeField] float minSpawnIntervalTime;

    //[SerializeField] float maxSpawnIntervalTime;

    [SerializeField] float exp;

    [SerializeField]  AllItemConfig.DropItemType[] dropItemTypes;

    public GameObject CreepPrefab => creepPrefab;

    public int BaseHp => baseHp;

    public float BaseSpeed => baseSpeed;

    public int BaseDmg => baseDmg;

    //public float StartSpawnTime => startSpawnTime;

    //public float SpawnRate => spawnRate;

    //public float MinSpawnIntervalTime => minSpawnIntervalTime;

    //public float MaxSpawnIntervalTime => maxSpawnIntervalTime;

    public float Exp => exp;

    public AllItemConfig.DropItemType[] DropItemTypes => dropItemTypes;

    public virtual void Move(Transform creepTransform, float speed) 
    {
        creepTransform.Translate(creepTransform.forward * speed * Time.deltaTime, Space.World);
    }

    public virtual void Attack(Creep creep) { }

    public virtual void OnDead(Creep creep) 
    {
        AllManager.Instance().creepManager.AddToDeactivateList(creep);
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