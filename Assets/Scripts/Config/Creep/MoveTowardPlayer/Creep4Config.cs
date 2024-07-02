using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Creep4Config", menuName = "Config/CreepConfig/MoveTowardPlayer/Creep4")]
public class Creep4Config : MoveTowardPlayerCreepConfig
{
    [SerializeField] float startIncreaseSpeedDis;
    [SerializeField] float speedMultiplayer;

    public override void Attack(Creep creep)
    {
        (string playerId, float dis) = GetNearestPlayerWithDis(creep.creepTrans);
        //float dis = Vector3.Distance(CharacterController.Instance().transform.position, creep.creepTrans.position);

        if (dis <= startIncreaseSpeedDis && creep.speed <= BaseSpeed)
        {
            creep.speed = creep.speed * speedMultiplayer;
        }

        Dictionary<string, Player> dictPlayers = AllManager.Instance().playerManager.dictPlayers;
        creep.creepTrans.rotation = Quaternion.LookRotation(dictPlayers[playerId].playerTrans.position - creep.creepTrans.position);
        //creep.creepTrans.rotation = Quaternion.LookRotation(CharacterController.Instance().transform.position - creep.creepTrans.position);
    }
}