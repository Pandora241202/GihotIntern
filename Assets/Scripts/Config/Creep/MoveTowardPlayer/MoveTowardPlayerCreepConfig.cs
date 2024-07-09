using System.Collections.Generic;
using UnityEngine;

public class MoveTowardPlayerCreepConfig : CreepConfig
{
    public virtual string RotateTowardPlayer(Creep creep) 
    {
        Dictionary<string, Player> dictPlayers = AllManager.Instance().playerManager.dictPlayers;

        (string playerId, float _) = GetNearestPlayerWithDis(creep.creepTrans);

        Vector3 rotateDir = dictPlayers[playerId].playerTrans.position - creep.creepTrans.position;
        rotateDir.y = 0;
        creep.creepTrans.rotation = Quaternion.LookRotation(rotateDir);

        return playerId;
    }
}