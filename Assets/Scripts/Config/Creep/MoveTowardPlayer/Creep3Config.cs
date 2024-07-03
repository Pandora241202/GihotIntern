using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Creep3Config", menuName = "Config/CreepConfig/MoveTowardPlayer/Creep3")]
public class Creep3Config : MoveTowardPlayerCreepConfig
{
    [SerializeField] BulletConfig bulletConfig;
    [SerializeField] float fireRate;

    public override void Attack(Creep creep)
    {
        Dictionary<string, Player> dictPlayers = AllManager.Instance().playerManager.dictPlayers;

        (string playerId, float _) = GetNearestPlayerWithDis(creep.creepTrans);

        Vector3 rotateDir = dictPlayers[playerId].playerTrans.position - creep.creepTrans.position;
        rotateDir.y = 0;
        creep.creepTrans.rotation = Quaternion.LookRotation(rotateDir);

        if (creep.timer >= fireRate)
        {
            creep.animator.SetTrigger("isAttack");
            bulletConfig.Fire(creep.creepTrans.position, dictPlayers[playerId].playerTrans.position, AllManager.Instance().bulletManager, "EnemyBullet", true, 0.25f);
            creep.timer = 0;
        } else
        {
            creep.timer += Time.deltaTime;
        }
    }
}