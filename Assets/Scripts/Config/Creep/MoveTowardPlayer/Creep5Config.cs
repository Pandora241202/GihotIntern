using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Creep5Config", menuName = "Config/CreepConfig/MoveTowardPlayer/Creep5")]
public class Creep5Config : MoveTowardPlayerCreepConfig
{
    [SerializeField] GameObject explosion;

    public override void OnDead(Creep creep)
    {
        GameObject explosionObj = GameObject.Instantiate(explosion, creep.creepTrans.position, Quaternion.identity);
        creep.weaponObj = explosionObj;
        AllManager.Instance().effectManager.AddEffect(creep.weaponObj);
        creep.UnSet();
    }

    public override void Attack(Creep creep)
    {
        if (creep.weaponObj == null) 
        {
            base.RotateTowardPlayer(creep);
            return;
        }

        if (creep.weaponObj != null)
        {
            ParticleSystem ps = creep.weaponObj.GetComponent<ParticleSystem>();

            if (ps != null)
            {
                if (ps.time >= 0.8 && ps.time <= 1)
                {
                    Dictionary<string, Player> dictPlayers = AllManager.Instance().playerManager.dictPlayers;

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
                    AllManager.Instance().effectManager.RemoveEffectById(creep.weaponObj.GetInstanceID());
                    Destroy(creep.weaponObj);
                    creep.weaponObj = null;
                    AllManager.Instance().creepManager.AddToDeactivateList(creep);
                }
            }
        }
    }
}