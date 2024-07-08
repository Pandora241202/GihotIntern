using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/PowerUpConfig/DmgBoost")]
public class DmgBoost : PowerUpsConfig
{
    public override void Activate()
    {
        AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].dmgBoostTime=15f;
        //Acctive UI show Boost
        
    }
}
