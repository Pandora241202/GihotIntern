using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCollision : MonoBehaviour
{
    public AllDropItemConfig.PowerUpsType powerUpType;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PowerUpManager powerUpManager = AllManager.Instance().powerUpManager;
            if (powerUpManager != null)
            {
                string playerId = other.GetComponent<CharacterControl>().id;
                powerUpManager.ApplyPowerUp(playerId, powerUpType);
                Destroy(gameObject); 
            }
        }
    }
}
