using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/PowerUpConfig/DmgBoost")]
public class DmgBoost : PowerUpsConfig
{
    public override void Activate()
    {
        Debug.Log("DMG Boost pickup");
        var player = AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID];
        player.SetDamageBoost(boostAmount);
        player.dmgBoostTime = duration;
        //TODO: Activate UI show Boost
    }
    public override void ApplyEffect()
    {
        Debug.Log("DMG Boost apply effect");
    }
    public override void Deactivate()
    {
        Debug.Log("DMG Boost deactivate");
        var player = AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID];
        player.SetDamageBoost(1);
    }
}
