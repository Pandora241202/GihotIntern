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
            creep.animator.SetTrigger("isAttack");
            bulletConfig.Fire(creep.creepTrans.position + new Vector3(0,1.5f,0), dictPlayers[playerId].playerTrans.position + new Vector3(0, 1.5f, 0), creep.dmg, AllManager.Instance().bulletManager, "EnemyBullet", true, 0.5f);
            creep.timer = 0;
        }
        else
        {
            creep.timer += Time.deltaTime;
        }
    }
}