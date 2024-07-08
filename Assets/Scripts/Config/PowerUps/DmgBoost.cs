using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/PowerUpConfig/DmgBoost")]
public class DmgBoost : PowerUpsConfig
{
    public override void Activate()
    {
        Debug.Log("DMG Boost activated");
        AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].dmgBoostTime=15f;
        //Activate UI show Boost
        
    }
}
