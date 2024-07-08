using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Creep6Config", menuName = "Config/CreepConfig/MoveTowardPlayer/Creep6")]
public class Creep6Config : MoveTowardPlayerCreepConfig
{
    [SerializeField] float skillCD;

    public override void Attack(Creep creep)
    {
        Dictionary<string, Player> dictPlayers = AllManager.Instance().playerManager.dictPlayers;

        (string playerId, float _) = GetNearestPlayerWithDis(creep.creepTrans);

        Vector3 rotateDir = dictPlayers[playerId].playerTrans.position - creep.creepTrans.position;
        rotateDir.y = 0;
        creep.creepTrans.rotation = Quaternion.LookRotation(rotateDir);

        if (creep.timer >= skillCD)
        {
            creep.animator.SetTrigger("isAttack");
            // drop meteor
            creep.timer = 0;
        }
        else
        {
            creep.timer += Time.deltaTime;
        }
    }
}