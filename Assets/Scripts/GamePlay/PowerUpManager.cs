using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpInfo
{
    public Transform powerUpObj;
    public bool isNeedDestroy;
    private float timeToLive;
    private float spawnTime;
    private float duration;

    public PowerUpInfo(Transform obj, float timeToLive = 15f) //15f for ALL power-up
    {
        this.powerUpObj = obj;
        this.timeToLive = timeToLive;
        this.spawnTime = Time.time;
        Setup();
    }

    public void Setup()
    {
        isNeedDestroy = false;
    }

    public void Update()
    {
        if (Time.time >= spawnTime + timeToLive)
        {
            isNeedDestroy = true;
        }
    }
}
public class PowerUpManager 
{
    public List<PowerUpInfo> powerUpInfoList = new List<PowerUpInfo>();
    private AllDropItemConfig allDropItemConfig;
    public PowerUpManager(AllDropItemConfig allDropItemConfig)
    {
        this.allDropItemConfig = allDropItemConfig;
    }
    public void MyUpdate()
    {
        for (int i = 0; i < powerUpInfoList.Count; i++)
        {
            powerUpInfoList[i].Update();
        }
        UpdatePlayerPowerUps();
    }

    public void LateUpdate()
    {
        for (int i = powerUpInfoList.Count - 1; i >= 0; i--)
        {
            if (powerUpInfoList[i].isNeedDestroy)
            {
                if (powerUpInfoList[i].powerUpObj != null)
                {
                    GameObject.Destroy(powerUpInfoList[i].powerUpObj.gameObject);
                    Debug.Log("Destroy power-up");
                    powerUpInfoList.RemoveAt(i);
                }
            }
        }
    }
    public void SpawnPowerUp(Vector3 posSpawn,  AllDropItemConfig.PowerUpsType powerUpType)
    {
        var powerUpAttr = allDropItemConfig.powerUpAttributesList.Find(attr => attr.type == powerUpType);
        var powerUpPrefab = powerUpAttr.powerUpConfig.powerUpPrefab;
        Transform powerUpObj = GameObject.Instantiate(powerUpPrefab, posSpawn, Quaternion.identity).transform;
        PowerUpInfo newPowerUp = new PowerUpInfo(powerUpObj, powerUpAttr.powerUpConfig.timeToLive);
        powerUpInfoList.Add(newPowerUp);
    }
    // public void ActivatePowerUp(AllDropItemConfig.PowerUpsType powerUpType)
    // {
    //     var powerUpAttr = allDropItemConfig.powerUpAttributesList.Find(attr => attr.type == powerUpType);
    //     powerUpAttr.powerUpConfig.Activate();
    // }
    public void ApplyPowerUp(string playerId, AllDropItemConfig.PowerUpsType powerUpType)
    {
        var powerUpAttr = allDropItemConfig.powerUpAttributesList.Find(attr => attr.type == powerUpType);
        Player player = AllManager.Instance().playerManager.dictPlayers[playerId];
        
        powerUpAttr.powerUpConfig.Activate();
        player.AddPowerUp(powerUpType.ToString(), powerUpAttr.powerUpConfig.duration);
    }

    public void DeactivatePowerUp(string powerUpName)
    {
        var powerUpAttr = allDropItemConfig.powerUpAttributesList.Find(attr => attr.type.ToString() == powerUpName);
        powerUpAttr?.powerUpConfig.Deactivate();
    }

    public void UpdatePlayerPowerUps()
    {
        foreach (var player in AllManager.Instance().playerManager.dictPlayers.Values)
        {
            player.UpdatePowerUps();
        }
    }
    public void setDeletePowerUp(int index)
    {
        powerUpInfoList[index].isNeedDestroy = true;
    }
}
