using UnityEngine;

[CreateAssetMenu(fileName = "Creep7Config", menuName = "Config/CreepConfig/MoveFarPlayer/Creep7")]
public class Creep7Config : MoveFarPlayerCreepConfig
{
    public override void Attack(Creep creep)
    {
        //TODO Regen HP other creep
        base.Attack(creep);
    }
}