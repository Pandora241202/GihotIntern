using System.Collections.Generic;
using UnityEngine;

public class MoveTowardPlayerCreepConfig : CreepConfig
{
    public override void Attack(Creep creep) 
    {
        Dictionary<string, Player> dictPlayers = AllManager.Instance().playerManager.dictPlayers;

        //(string playerId, float _) = GetNearestPlayerWithDis(creep.creepTrans);

        //creep.creepTrans.rotation = Quaternion.LookRotation(dictPlayers[playerId].playerTrans.position - creep.creepTrans.position);

        //creep.creepTrans.rotation = Quaternion.LookRotation(CharacterController.Instance().transform.position - creep.creepTrans.position);
    }

    public override void Move(Transform creepTransform, float speed)
    {
        Dictionary<string, Player> dictPlayers = AllManager.Instance().playerManager.dictPlayers;
        (string playerId, float _) = GetNearestPlayerWithDis(creepTransform);
        Vector3 direction =dictPlayers[playerId].playerTrans.position - creepTransform.position;
        creepTransform.position += direction * speed * Time.deltaTime;
        
    }
}