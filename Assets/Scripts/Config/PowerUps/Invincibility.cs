using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/PowerUpConfig/Invincibility")]
public class Invincibility : PowerUpsConfig
{
    public override void Activate()
    {
        Debug.Log("Invincibility Power-Up Activated");
        var player = AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID];
        player.playerTrans.GetComponent<CharacterControl>().EnableInvincibility(boostAmount);
    }
}
