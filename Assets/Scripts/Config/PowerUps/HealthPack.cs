using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/PowerUpConfig/HealthPack")]
public class HealthPack : PowerUpsConfig
{
    public override void Activate(int playerId)
    {
        Debug.Log("HealthPack activated");
        AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].health++;
        Debug.Log(AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].health);
    }
}
