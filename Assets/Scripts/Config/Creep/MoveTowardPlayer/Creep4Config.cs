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

        if (dis <= startIncreaseSpeedDis && creep.speed <= BaseSpeed)
        {
            creep.animator.SetTrigger("isAttack");
            creep.speed = creep.speed * speedMultiplayer;
        }

        Dictionary<string, Player> dictPlayers = AllManager.Instance().playerManager.dictPlayers;
        Vector3 rotateDir = dictPlayers[playerId].playerTrans.position - creep.creepTrans.position;
        rotateDir.y = 0;
        creep.creepTrans.rotation = Quaternion.LookRotation(rotateDir);
    }
}