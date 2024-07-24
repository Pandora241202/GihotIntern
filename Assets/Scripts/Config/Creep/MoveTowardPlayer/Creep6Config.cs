using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Creep6Config", menuName = "Config/CreepConfig/MoveTowardPlayer/Creep6")]
public class Creep6Config : MoveTowardPlayerCreepConfig
{
    [SerializeField] float skillCD;
    [SerializeField] GameObject meteor;

    public override void OnDead(Creep creep)
    {
        if (creep.weaponObj != null)
        {
            creep.UnSet();
        }
        else
        {
            base.OnDead(creep);
        }
    }

    public override void Attack(Creep creep)
    {
        Dictionary<string, Player> dictPlayers = AllManager.Instance().playerManager.dictPlayers;

        string playerId = base.RotateTowardPlayer(creep);

        if (playerId != null)
        {
            return;
        }

        if (creep.timer >= skillCD)
        {
            creep.animator.SetTrigger("isAttack");
            creep.weaponObj = GameObject.Instantiate(meteor, dictPlayers[playerId].playerTrans.position, Quaternion.identity);
            creep.timer = 0;
        }
        else
        {
            creep.timer += Time.deltaTime;
        }

        if (creep.weaponObj != null)
        {
            ParticleSystem ps = creep.weaponObj.GetComponent<ParticleSystem>();
            if (ps)
            {
                if (ps.time >= 0.5 && ps.time <= 1)
                {
                    foreach (var pair in dictPlayers)
                    {
                        Player player = pair.Value;
                        if (Vector3.Magnitude(player.playerTrans.position - ps.transform.position) <= 3)
                        {
                            if(player.id == Player_ID.MyPlayerID) player.ProcessDmg(creep.dmg);
                        }
                    }
                }

                if (ps.isStopped)
                {
                    Destroy(creep.weaponObj);
                    creep.weaponObj = null;
                    if (creep.creepTrans.gameObject.activeInHierarchy == false)
                    {
                        AllManager.Instance().creepManager.AddToDeactivateList(creep);
                    }
                }
            }   
        }
    }
}