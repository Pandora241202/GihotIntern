﻿using Cinemachine;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine;

public class Player
{
    public Transform playerTrans;
    public string name;
    public string id;
    public int gunId;
    public GunConfig gunConfig;
    public PlayerConfig playerConfig;
    public GameObject levelUpEffect;
    //Player stat 
    public int health;
    public float lifeSteal;
    public float speed;
    public bool isDead;
    public float dmgBoostTime = 0;
    public float dmgBoostAmount;
    public float speedBoostTime = 0;
    public float speedBoostAmount;

    private Dictionary<string, float> activePowerUps;
    
    public Player(string name, string id, int gunId, PlayerConfig config)
    {
        this.name = name;
        this.id = id;
        this.gunId = gunId;
        this.playerConfig = config;
        this.health = Constants.PlayerBaseMaxHealth;
        this.lifeSteal = Constants.LifeSteal;
        this.speed = Constants.PlayerBaseSpeed;
        isDead = false;
        levelUpEffect = null;
        activePowerUps = new Dictionary<string, float>();
    }

    public void Onstart()
    {
        this.health = Constants.PlayerBaseMaxHealth;
        this.lifeSteal = Constants.LifeSteal;
        this.speed = Constants.PlayerBaseSpeed;
        activePowerUps.Clear();
    }
    public void AddPowerUp(string powerUpName, float duration)
    {
        if (activePowerUps.ContainsKey(powerUpName))
        {
            activePowerUps[powerUpName] = duration; // Reset the duration if already active
            Debug.Log($"Resetting power-up: {powerUpName}");
        }
        else
        {
            activePowerUps.Add(powerUpName, duration);
            Debug.Log($"Activating power-up: {powerUpName}");
        }
    }
    public void UpdatePowerUps()
    {
        List<string> expiredPowerUps = new List<string>();

        foreach (var powerUp in activePowerUps.Keys.ToList())
        {
            activePowerUps[powerUp] -= Time.deltaTime;
            Debug.Log($"Power-up: {powerUp} has {activePowerUps[powerUp]}s left");
            if (activePowerUps[powerUp] <= 0)
            {
                expiredPowerUps.Add(powerUp);
            }
        }

        foreach (var powerUp in expiredPowerUps)
        {
            DeactivatePowerUp(powerUp);
            activePowerUps.Remove(powerUp);
        }
    }

    private void DeactivatePowerUp(string powerUpName)
    {
        Debug.Log($"Deactivating power-up: {powerUpName}");
        AllManager.Instance().powerUpManager.DeactivatePowerUp(powerUpName);
    }
    public void SetDamageBoost(float boostAmount)
    {
        this.dmgBoostAmount = boostAmount;
    }
    public void SetSpeedBoost(float boostAmount)
    {
        this.speed = Constants.PlayerBaseSpeed * (1+boostAmount);
        playerTrans.gameObject.GetComponent<CharacterControl>().speed = this.speed;
        
    }
    public void ProcessDmg(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            health = 0;
            //died
            isDead = true;
        }
        UIManager._instance.uiGameplay.UpdateHealthSlider(health);
    }
}

public class PlayerManager
{
    public Dictionary<string, Player> dictPlayers = new Dictionary<string, Player>();
    GameObject characterPrefab;
    public PlayerManager(GameObject characterPrefab)
    {
        this.characterPrefab = characterPrefab;
    }
    public int exp = 0;
    public int level = Constants.PlayerBaseLevel;
    public int expRequire = Constants.PlayerBaseExp;
    public float expBoostTime = 0;
    public float expBoostAmount = 1.5f;
    public void MyUpdate()
    {
        foreach (var player in dictPlayers.Values)
        {
            if (player.dmgBoostTime > 0)
            {
                player.dmgBoostTime -= Time.deltaTime;
                if (player.dmgBoostTime < 0)
                {
                    player.dmgBoostTime = 0;
                }
            }
            if (player.speedBoostTime > 0)
            {
                player.speedBoostTime -= Time.deltaTime;
                if (player.speedBoostTime < 0)
                {
                    player.speedBoostTime = 0;
                    player.speed = Constants.PlayerBaseSpeed;
                }
            }
            if (expBoostTime > 0){
                expBoostTime -= Time.deltaTime;
                if (expBoostTime < 0)
                {
                    expBoostTime = 0;
                    expBoostAmount = 1f;
                }
            }
        }
    }
    public void LateUpdate(){}
    public void ProcessExpGain(int expGain)
    {
        expGain = (int)(expGain * (1 + expBoostAmount));
        Debug.Log(expGain);
        exp += expGain;
        UIManager._instance.uiGameplay.UpdateLevelSlider(exp);
        if (exp >= expRequire)
        {
            expRequire = Constants.PlayerBaseExp + (level - 1) * Constants.ExpIncrement + Constants.ScalingMultiplierExp * (level - 1) * (level - 1);
            level++;
            UIManager._instance.uiGameplay.LevelUpdateSlider(expRequire);
            exp = 0;
            foreach (var pair in  dictPlayers)
            {
                Player player = pair.Value;

                player.levelUpEffect = GameObject.Instantiate(player.playerConfig.levelUpEffect, player.playerTrans.position, Quaternion.identity);
            }
        }
            
    }

    public void FreshStart()
    {
        exp = 0;
        level = Constants.PlayerBaseLevel;
        expRequire = Constants.PlayerBaseExp;
        foreach (var item in dictPlayers)
        {
            item.Value.Onstart();
        }

    }
    public int GetMaxHealthFromLevel()
    {
        return Constants.PlayerBaseMaxHealth + (level - 1) * 3;
    }

    public int GetBaseSpeedFromLevel()
    {
        return Constants.PlayerBaseSpeed + (level - 1) / 2;
    }

    public int GetPlayerDmg(string playerId)
    {
        Player player = dictPlayers[playerId];
        GunType gunType = AllManager.Instance().gunConfig.lsGunType[player.gunId];
        float boostMultiplier = player.dmgBoostTime > 0 ? player.dmgBoostAmount : 1f;
        return (int)(gunType.baseDamage + (level - 1) * boostMultiplier * gunType.bulletMultiplier);
    }

    public void SpawnPlayer(Vector3 position, string id, int gun_id)
    {
        Player player = this.dictPlayers[id];
        player.playerTrans = GameObject.Instantiate(characterPrefab, position, Quaternion.identity).transform;
        player.playerTrans.gameObject.GetComponent<CharacterControl>().id = id;
        player.playerTrans.gameObject.GetComponent<CharacterControl>().gunId = gun_id;
        player.playerTrans.gameObject.GetComponent<CharacterControl>().speed = player.speed;
        //Debug.Log("Player: " + player.name + " gun: " + gun_id);
        player.playerTrans.gameObject.GetComponent<CharacterControl>().SetGunAndBullet();
        if (id == Player_ID.MyPlayerID)
            player.playerTrans.Find("CM vcam1").gameObject.GetComponent<CinemachineVirtualCamera>().Priority = 11;
    }

    public void RemovePlayer(string id)
    {
        if(id == Player_ID.MyPlayerID)
        {
            Player pl = dictPlayers.ElementAt(0).Value;
            this.dictPlayers.Clear();
            this.dictPlayers.Add(pl.id, pl);
            AllManager.Instance().LoadSceneAsync("UI", "Main Menu");
            return;
        }

        Player player = this.dictPlayers[id];
        if (player.playerTrans != null)
        {
            Debug.Log("Player destroyed");
            GameObject.Destroy(player.playerTrans.gameObject);
        }
        this.dictPlayers.Remove(id);
    }


    public void AddPlayer(string name, string id, int gunId, PlayerConfig playerConfig)
    {
        Player newPlayer = new Player(name, id, gunId, playerConfig);
        dictPlayers.Add(id, newPlayer);
    }

    public void UpdatePlayersState(PlayersState playersState)
    {
        Player player;
        PlayerState state;
        for (int i = 0; i < playersState.states.Length; i++)
        {
            state = playersState.states[i];
            player = dictPlayers[state.player_id];
            player.isDead = state.isDead;
            CharacterControl c_Controller = player.playerTrans.gameObject.GetComponent<CharacterControl>();
            c_Controller.input_velocity = state.velocity;
            //c_Controller.goChar.transform.rotation = state.rotation;
            if (!c_Controller.isColliding && c_Controller.id != Player_ID.MyPlayerID)
            {
                if ((player.playerTrans.position - state.position).magnitude <= Time.fixedDeltaTime * c_Controller.speed)
                {
                    c_Controller.lerp = false;
                    c_Controller.transform.position = state.position;
                }
                else
                {
                    c_Controller.lerp = true;
                    c_Controller.elapseFrame = 0;
                    c_Controller.lerpPosition = state.position;
                    c_Controller.lerpVertor = (c_Controller.lerpPosition - player.playerTrans.position);
                }
            }

            c_Controller.correctPositionTime = 0;
            if (state.isFire) c_Controller.Shoot();
            if (state.isDead)
            {
                c_Controller.charAnim.SetBool("isDead",true);
            }
            //player.playerTrans.position = state.position;
        }
    }

    public void ProcessCollisionCreep(string playerId, int creepId)
    {
        if(playerId!=Player_ID.MyPlayerID)return;
        Creep creep = AllManager.Instance().creepManager.GetActiveCreepById(creepId);
        
        dictPlayers[playerId].ProcessDmg(creep.dmg);
    }

    public void ProcessCollisionEnemyBullet(string playerId, int bulletId)
    {
        if(playerId!=Player_ID.MyPlayerID) return;
        BulletInfo bullet = AllManager.Instance().bulletManager.bulletInfoDict[bulletId];
        dictPlayers[playerId].ProcessDmg(bullet.damage);
        AllManager.Instance().bulletManager.SetDelete(bulletId);
    }

    public void ProcessLifeSteal()
    {
        int lifeSteal = Random.Range(0, 100);
        if (lifeSteal <= AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].lifeSteal)
        {
            Debug.Log("Hut dc 1 mau nha may em yeu");
            AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].health++;
        }
    }
}