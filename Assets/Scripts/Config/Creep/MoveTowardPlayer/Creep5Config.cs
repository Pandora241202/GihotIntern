using UnityEngine;

[CreateAssetMenu(fileName = "Creep5Config", menuName = "Config/CreepConfig/MoveTowardPlayer/Creep5")]
public class Creep5Config : MoveTowardPlayerCreepConfig
{
    [SerializeField] float bombDmg;

    public override void OnDead(Transform creepTransform)
    {
        //TODO spawn bomb
    }
}