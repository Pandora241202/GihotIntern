using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Creep2Config", menuName = "Config/CreepConfig/NotMoving/Creep2")]
public class Creep2Config : NotMovingCreepConfig
{
    [SerializeField] BulletConfig bulletConfig;
    [SerializeField] float fireRate;

    public override void Attack(Creep creep)
    {
        Dictionary<string, Player> dictPlayers = AllManager.Instance().playerManager.dictPlayers;

        (string playerId, float _) = GetNearestPlayerWithDis(creep.creepTrans);

        creep.creepTrans.rotation = Quaternion.LookRotation(dictPlayers[playerId].playerTrans.position - creep.creepTrans.position);

        if (creep.timer >= fireRate)
        {
            bulletConfig.Fire(creep.creepTrans.position, dictPlayers[playerId].playerTrans.position, AllManager.Instance().bulletManager, "EnemyBullet");
            creep.timer = 0;
        }
        else
        {
            creep.timer += Time.deltaTime;
        }
    }
}