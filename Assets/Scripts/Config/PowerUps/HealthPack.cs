using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/PowerUpConfig/HealthPack")]
public class HealthPack : PowerUpsConfig
{
    public override void Activate()
    {
        Debug.Log("HealthPack activated");
        int healAmount=(int)(AllManager.Instance().playerManager.GetMaxHealthFromLevel()*this.boostAmount);
        AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].ChangeHealth(healAmount);
        
    }
}
