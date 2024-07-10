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
    public virtual void Activate()
    {

    }
    public virtual void ApplyEffect()
    {

    }
    public virtual void Deactivate()
    {

    }
}
