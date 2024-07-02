using UnityEngine;

[CreateAssetMenu(fileName = "Creep5Config", menuName = "Config/CreepConfig/MoveTowardPlayer/Creep5")]
public class Creep5Config : MoveTowardPlayerCreepConfig
{
    [SerializeField] GameObject bombPrefab;
    [SerializeField] float timeToExplode;

    public override void OnDead(Creep creep)
    {
        GameObject bombObj = GameObject.Instantiate(bombPrefab, creep.creepTrans.position, Quaternion.identity);
        creep.bombObj = bombObj;
        creep.UnSet();
    }

    public override void Attack(Creep creep)
    {
        if (creep.bombObj == null) 
        {
            base.Attack(creep);
            return;
        }

        if (creep.timer >= timeToExplode)
        {
            GameObject.Destroy(creep.bombObj);
            creep.bombObj = null;
            AllManager.Instance().creepManager.AddToDeactivateList(creep);
        } else
        {
            creep.timer += Time.deltaTime;
        }
    }
}