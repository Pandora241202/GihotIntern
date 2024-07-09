using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/PowerUpConfig/SpeedBoost")]
public class SpeedBoost : PowerUpsConfig
{
    public override void Activate()
    {
        Debug.Log("Speed Boost pickup");
        var player = AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID];
        player.SetSpeedBoost(boostAmount);
        player.speedBoostTime = 15f;
        //TODO: Activate UI show Boost
    }
}
