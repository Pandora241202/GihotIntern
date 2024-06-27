using System.Collections.Generic;
using UnityEngine;

public class MoveTowardPlayerCreepConfig : CreepConfig
{
    public override void Move(Transform creepTransform, float speed) 
    {
        Dictionary<string, Player> dictPlayers = AllManager._instance.playerManager.dictPlayers;

        (string playerId, float _) = GetNearestPlayerWithDis(creepTransform);

        creepTransform.Translate((dictPlayers[playerId].playerTrans.position - creepTransform.position).normalized * speed * Time.deltaTime);
    }
}