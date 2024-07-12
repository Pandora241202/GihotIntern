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
        player.AddPowerUp("SpeedBoost", duration);
        //TODO: Activate UI show Boost
    }
    public override void ApplyEffect()
    {
        Debug.Log("SpeedBoost Boost apply effect");
    }
    public override void Deactivate()
    {
        Debug.Log("Speed Boost deactivate");
        var player = AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID];
        player.SetSpeedBoost(0);
    }
}
