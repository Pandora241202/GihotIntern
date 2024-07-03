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

        if (minDis <= startMoveAwayDistance)
        {
            Vector3 rotateDir = creep.creepTrans.position - dictPlayers[playerId].playerTrans.position;
            rotateDir.y = 0;
            creep.creepTrans.rotation = Quaternion.LookRotation(rotateDir);
            creep.speed = BaseSpeed;
        }
        else
        {
            creep.speed = 0;
        }
    }
}