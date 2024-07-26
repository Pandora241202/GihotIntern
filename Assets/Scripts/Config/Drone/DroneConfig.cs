
using UnityEngine;
[CreateAssetMenu(fileName = "DroneConfig", menuName = "Config/DroneConfig/DroneConfig")]
public class DroneConfig : ScriptableObject
{
    public GameObject prefDrone;
    public int gunId;
    public GunConfig gunConfig;
    public GameObject curCreepTarget = null;
    public float lastFireTime = 0f;
}
