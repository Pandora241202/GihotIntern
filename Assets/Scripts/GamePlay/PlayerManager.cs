using Cinemachine;
using JetBrains.Annotations;
using System.Buffers.Text;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class Player
{
    public Transform playerTrans;
    public string name;
    public string id;
    public int gunId;
    public GunConfig gunConfig;
    public PlayerConfig playerConfig;
    
    //Player stat 

    public int health; 
    public float speed;
    public Player(string name, string id, int gunId, PlayerConfig config)
    {
        this.name = name;
        this.id = id;
        this.gunId = gunId;
        this.playerConfig = config;
        this.health = Constants.PlayerBaseMaxHealth;
        this.speed = Constants.PlayerBaseSpeed;
    }

    public void ProcessDmg(int dmg)
    {
        health -= dmg;
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


    public void ProcessExpGain(int expGain)
    {
        exp += expGain;
        if (exp >= expRequire)
        {
            expRequire = Constants.PlayerBaseExp + (level - 1) * Constants.ExpIncrement + Constants.ScalingMultiplierExp * (level-1) * (level-1);
            level++;
            exp -= expRequire;
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

        return gunType.baseDamage + (level - 1) * gunType.bulletMultiplier;
    }

    public void SpawnPlayer(Vector3 position, string id, int gun_id)
    {
        Player player = this.dictPlayers[id]; 
        player.playerTrans = GameObject.Instantiate(characterPrefab, position, Quaternion.identity).transform;
        player.playerTrans.gameObject.GetComponent<CharacterControl>().id = id;
        player.playerTrans.gameObject.GetComponent<CharacterControl>().gunId = gun_id;
        //Debug.Log("Player: " + player.name + " gun: " + gun_id);
        player.playerTrans.gameObject.GetComponent<CharacterControl>().SetGunAndBullet();
        if (id == Player_ID.MyPlayerID) 
            player.playerTrans.Find("CM vcam1").gameObject.GetComponent<CinemachineVirtualCamera>().Priority = 11;
    }

    public void RemovePlayer(string id)
    {
        this.dictPlayers.Remove(id);
    }
    public void AddPlayer(string name, string id, int gunId,PlayerConfig playerConfig)
    {
        Player newPlayer = new Player(name, id, gunId, playerConfig);
        dictPlayers.Add(id,newPlayer);
    }

    public void UpdatePlayersState(PlayersState playersState)
    {
        Player player;
        PlayerState state;
        for (int i = 0; i < playersState.states.Length; i++)
        {
            state = playersState.states[i];
            player = dictPlayers[state.player_id];
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
                }
            }
                
            c_Controller.correctPositionTime = 0;
            //player.playerTrans.position = state.position;
        }
    }

    public void UpdatePlayerVelocity(PlayerState playersState)
    {
        //CharacterControl player = dictPlayers[playersState.player_id].playerTrans.gameObject.GetComponent<CharacterControl>();
        //player.velocity = playersState.velocity;
        //player.goChar.transform.rotation = playersState.rotation;
        //if(!player.isColliding) if((player.transform.position - playersState.position).magnitude > 0.2) player.transform.position = playersState.position;
        //player.newPosition = playersState.position;
    }

    public void UpdatePlayerPosition(PlayerPosition playerPosition)
    {
        //CharacterControl player = dictPlayers[playerPosition.player_id].playerTrans.gameObject.GetComponent<CharacterControl>();
        //if ((player.transform.position - playerPosition.position).magnitude > 0.5) player.transform.position = playerPosition.position;
        //player.correctPositionTime = 0;
    }

    public void ProcessCollisionCreep(string playerId, int creepId)
    {
        Creep creep = AllManager.Instance().creepManager.GetActiveCreepById(creepId);
        dictPlayers[playerId].ProcessDmg(creep.dmg);
    }

    public void ProcessCollisionEnemyBullet(string playerId, int bulletId)
    {
        BulletInfo bullet = AllManager.Instance().bulletManager.bulletInfoDict[bulletId];
        dictPlayers[playerId].ProcessDmg(bullet.damage);
        AllManager.Instance().bulletManager.SetDelete(bulletId);
    }
}