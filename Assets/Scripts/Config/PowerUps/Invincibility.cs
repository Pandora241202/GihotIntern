using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/PowerUpConfig/Invincibility")]
public class Invincibility : PowerUpsConfig
{
    public override void Activate()
    {
        Debug.Log("Invincibility Power-Up Activated");
        
    }
    public override void ApplyEffect()
    {
        Debug.Log("Invincibility Power-Up Apply Effect");
        var player = AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID];
        player.playerTrans.GetComponent<CharacterControl>().EnableInvincibility(duration);
        
    }
    public override void Deactivate()
    {
        Debug.Log("Invincibility Power-Up Deactivated");
    }
}
