using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/PowerUpConfig/TimeWarp")]
public class TimeWarp : PowerUpsConfig
{
    public override void Activate()
    {
        foreach (var creep in AllManager.Instance().creepManager.GetCreepActiveDict())
        {
            creep.Value.speed *= 1 + boostAmount;
        }
    }

    public override void Deactivate()
    {
        foreach (var creep in AllManager.Instance().creepManager.GetCreepActiveDict())
        {
            creep.Value.speed *= 1;
        }
    }
}
