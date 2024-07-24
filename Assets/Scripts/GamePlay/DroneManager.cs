using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone
{
    public Transform droneTrans;
    public DroneConfig droneConfig;

    public Drone(Transform droneTrans, DroneConfig droneConfig)
    {
        this.droneTrans = droneTrans;
        this.droneConfig = droneConfig;
    }
}
