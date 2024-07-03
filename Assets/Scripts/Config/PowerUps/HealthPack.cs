using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/PowerUpConfig/HealthPack")]
public class HealthPack : PowerUpsConfig
{
    public override void Activate(Player player)
    {
        Debug.Log("HealthPack activated");
    }
}
