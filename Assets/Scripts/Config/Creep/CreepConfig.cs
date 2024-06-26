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

    public virtual void Move(Transform creepTransform, float speed) { }

    public virtual void Attack() { }
}