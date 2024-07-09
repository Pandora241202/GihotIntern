using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/PowerUpConfig/ExpGainBoost")]
public class ExpGainBoost : PowerUpsConfig
{
    public override void Activate()
    {
        Debug.Log("EXP Gain Boost pickup");
        var playerManager = AllManager.Instance().playerManager;
        playerManager.expBoostAmount = boostAmount;
        playerManager.expBoostTime = 15f;
        //TODO: Activate UI show Boost
    }
}
