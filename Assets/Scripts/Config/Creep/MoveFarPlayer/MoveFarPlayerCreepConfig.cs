using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveFarPlayerCreepConfig : CreepConfig
{
    [SerializeField] float startMoveAwayDistance;

    public override void Move(Transform creepTransform, float speed) 
    {
        Dictionary<string, Player> dictPlayers = AllManager.Instance().playerManager.dictPlayers;

        (string playerId, float minDis) = GetNearestPlayerWithDis(creepTransform);

        if (minDis <= startMoveAwayDistance)
        {
            creepTransform.Translate((creepTransform.position - dictPlayers[playerId].playerTrans.position).normalized * speed * Time.deltaTime);
            //creepTransform.Translate((CharacterController.Instance().transform.position - creepTransform.position).normalized * speed * Time.deltaTime);
        }
    }
}