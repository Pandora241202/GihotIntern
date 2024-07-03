using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveFarPlayerCreepConfig : CreepConfig
{
    [SerializeField] float startMoveAwayDistance;

    public override void Attack(Creep creep) 
    {
        Dictionary<string, Player> dictPlayers = AllManager.Instance().playerManager.dictPlayers;

        (string playerId, float minDis) = GetNearestPlayerWithDis(creep.creepTrans);
        //float minDis = Vector3.Distance(creep.creepTrans.position, CharacterControl.Instance().transform.position);

        if (minDis <= startMoveAwayDistance)
        {
            creep.creepTrans.rotation = Quaternion.LookRotation(creep.creepTrans.position - dictPlayers[playerId].playerTrans.position);
            //creep.creepTrans.rotation = Quaternion.LookRotation(creep.creepTrans.position - CharacterControl.Instance().transform.position);
            creep.speed = BaseSpeed;
        }
        else
        {
            creep.speed = 0;
        }
    }
}