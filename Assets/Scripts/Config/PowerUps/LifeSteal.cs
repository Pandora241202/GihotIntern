using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/PowerUpConfig/LifeSteal")]
public class LifeSteal : PowerUpsConfig
{
    public override void Activate()
    {
        AllManager._instance.playerManager.dictPlayers[Player_ID.MyPlayerID].lifesteal+=(int)this.boostAmount;
    }
}
