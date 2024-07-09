using UnityEngine;

[CreateAssetMenu(fileName = "Creep1Config", menuName = "Config/CreepConfig/MoveTowardPlayer/Creep1")]
public class Creep1Config : MoveTowardPlayerCreepConfig
{
    public override void Attack(Creep creep)
    {
        base.RotateTowardPlayer(creep);
    }
}