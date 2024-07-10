using OpenCover.Framework.Model;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Creep7Config", menuName = "Config/CreepConfig/MoveFarPlayer/Creep7")]
public class Creep7Config : MoveFarPlayerCreepConfig
{
    [SerializeField] GameObject healing;
    [SerializeField] float skillCD;
    [SerializeField] int healAmount;

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
        base.Attack(creep);

        if (creep.timer >= skillCD)
        {
            creep.animator.SetTrigger("isAttack");
            
            Creep creepNeedHeal = AllManager.Instance().creepManager.GetRandomHurtCreep();
            
            if (creepNeedHeal != null)
            {
                creep.weaponObj = GameObject.Instantiate(healing, creepNeedHeal.creepTrans.position, Quaternion.identity);
                
                creepNeedHeal.hp += healAmount;
                if (creepNeedHeal.hp > creepNeedHeal.maxHp)
                {
                    creepNeedHeal.hp = creepNeedHeal.maxHp;
                }
            }

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