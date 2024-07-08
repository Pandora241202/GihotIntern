using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsConfig : ScriptableObject
{
    public GameObject powerUpPrefab;
    public int dropNumber;
    public float timeToLive;
    public float duration;
    public float boostAmount;
    public bool isStackable;
    public float dropChance;
    public virtual void Activate()
    {

    }
}
