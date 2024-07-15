using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUpInfo
{
    public Transform powerUpObj;
    public bool isNeedDestroy;
    public int sharedId;

    public float spawnTime;
    public PowerUpsConfig config;
    public AllDropItemConfig.PowerUpsType type;

    public PowerUpInfo(Transform obj, PowerUpsConfig config, AllDropItemConfig.PowerUpsType type, int sharedId) //15f for ALL power-up
    {
        this.powerUpObj = obj;
        this.spawnTime = Time.time;
        this.config = config;
        this.type = type;
        this.sharedId = sharedId;
        Setup();
    }

    public void Setup()
    {
        isNeedDestroy = false;
    }

    public void Update()
    {
        if (Time.time >= spawnTime + config.timeToLive)
        {
            isNeedDestroy = true;
        }
    }

    public void ProcessPickedUpByPlayer(string playerId)
    {
        // Send to server player pick up powerUp
        if (playerId == Player_ID.MyPlayerID)
        {
            SendData<PowerUpPickInfo> data = new SendData<PowerUpPickInfo>(new PowerUpPickInfo(playerId, sharedId));
            SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
        }
    }
}
public class PowerUpManager 
{
    //public List<PowerUpInfo> powerUpInfoList = new List<PowerUpInfo>();
    private Dictionary<int, PowerUpInfo> powerUpInfoDict = new Dictionary<int, PowerUpInfo>();
    Dictionary<int, int> powerUpSharedIdDict = new Dictionary<int, int>(); // used to store powerUp id share between players and server and map to instanceId
    private AllDropItemConfig allDropItemConfig;
    public PowerUpManager(AllDropItemConfig allDropItemConfig)
    {
        this.allDropItemConfig = allDropItemConfig;
    }
    public void MyUpdate()
    {
        foreach (var pair in powerUpInfoDict)
        {
            PowerUpInfo powerUpInfo = pair.Value;
            powerUpInfo.Update();
        }
        UpdatePlayerPowerUps();
    }

    public void LateUpdate()
    {
        List<int> destroyList = new List<int>();

        foreach (var pair in powerUpInfoDict)
        {
            PowerUpInfo powerUpInfo = pair.Value;

            if (powerUpInfo.isNeedDestroy)
            {
                destroyList.Add(pair.Key);
            }
        }

        foreach (int id in destroyList)
        {
            GameObject.Destroy(powerUpInfoDict[id].powerUpObj.gameObject);
            powerUpInfoDict.Remove(id);
        }
    }
    public void SpawnPowerUp(Vector3 posSpawn, AllDropItemConfig.PowerUpsType powerUpType, int shared_id)
    {
        var powerUpAttr = allDropItemConfig.powerUpAttributesList.Find(attr => attr.type == powerUpType);
        var powerUpPrefab = powerUpAttr.powerUpConfig.powerUpPrefab;
        Transform powerUpObj = GameObject.Instantiate(powerUpPrefab, posSpawn, Quaternion.identity).transform;
        PowerUpInfo newPowerUp = new PowerUpInfo(powerUpObj, powerUpAttr.powerUpConfig, powerUpType, shared_id);
        powerUpInfoDict.Add(powerUpObj.gameObject.GetInstanceID(), newPowerUp);
        powerUpSharedIdDict.Add(shared_id, powerUpObj.gameObject.GetInstanceID());
    }
    // public void ActivatePowerUp(AllDropItemConfig.PowerUpsType powerUpType)
    // {
    //     var powerUpAttr = allDropItemConfig.powerUpAttributesList.Find(attr => attr.type == powerUpType);
    //     powerUpAttr.powerUpConfig.Activate();
    // }
    public void ApplyPowerUpBySharedId(string playerId, int sharedId)
    {
        Player player = AllManager.Instance().playerManager.dictPlayers[playerId];

        int powerUpId = powerUpSharedIdDict[sharedId];
        PowerUpInfo powerUpInfo = powerUpInfoDict[powerUpId];

        powerUpInfo.config.Activate();
        player.AddPowerUp(powerUpInfo.type, powerUpInfo.config.duration);
    }

    public void DeactivatePowerUpByType(AllDropItemConfig.PowerUpsType type)
    {
        var powerUpAttr = allDropItemConfig.powerUpAttributesList.Find(attr => attr.type == type);
        powerUpAttr?.powerUpConfig.Deactivate();
    }

    public void UpdatePlayerPowerUps()
    {
        foreach (var player in AllManager.Instance().playerManager.dictPlayers.Values)
        {
            player.UpdatePowerUps();
        }
    }

    public void SetDeletePowerUpBySharedId(int sharedId)
    {
        int powerUpId = powerUpSharedIdDict[sharedId];
        powerUpInfoDict[powerUpId].isNeedDestroy = true;
    }

    public void ProcessCollisionPlayer(int powerUpId, string playerId)
    {
        powerUpInfoDict[powerUpId].ProcessPickedUpByPlayer(playerId);
    }

    public void UpdatePowerUpsState(PowerUpPickInfo[] powerUpPickInfos)
    {
        if (powerUpPickInfos != null)
        {
            foreach (PowerUpPickInfo powerUpPickInfo in powerUpPickInfos)
            {
                ApplyPowerUpBySharedId(powerUpPickInfo.player_id, powerUpPickInfo.shared_id);
                SetDeletePowerUpBySharedId(powerUpPickInfo.shared_id);
            }
        }
    }
}
