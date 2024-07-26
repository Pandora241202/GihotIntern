using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/ItemConfig/AllDropItems")]
public class AllDropItemConfig : ScriptableObject
{
    public enum PowerUpsType
    {
        DmgBoost,
        SpeedBoost,
        EXPGainBoost,
        HealthPack,
        // EXPOrb,
        // Bomb,
        Invincibility,
        TimeWarp,
        // PoisonAura
    }   
    [System.Serializable]
    public class PowerUpAttributes
    {
        public PowerUpsType type;
        public PowerUpsConfig powerUpConfig;
    }
    public List<PowerUpAttributes> powerUpAttributesList = new List<PowerUpAttributes>();
    public virtual void Activate()
    {

    }  
}
