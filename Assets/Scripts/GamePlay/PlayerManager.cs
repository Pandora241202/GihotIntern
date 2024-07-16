using Cinemachine;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public float dmgBoostAmount;
    public float speedBoostAmount;

    private Dictionary<AllDropItemConfig.PowerUpsType, float> activePowerUps;
    
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
        activePowerUps = new Dictionary<AllDropItemConfig.PowerUpsType, float>();
    }

    public void Onstart()
    {
        this.health = Constants.PlayerBaseMaxHealth;
        this.lifeSteal = Constants.LifeSteal;
        this.speed = Constants.PlayerBaseSpeed;
        activePowerUps.Clear();
        isDead = false;
    }
    public void AddPowerUp(AllDropItemConfig.PowerUpsType powerUpsType, float duration)
    {
        if (activePowerUps.ContainsKey(powerUpsType))
        {
            activePowerUps[powerUpsType] = duration; // Reset the duration if already active
            Debug.Log($"Resetting power-up: {powerUpsType}");
        }
        else
        {
            activePowerUps.Add(powerUpsType, duration);
            Debug.Log($"Activating power-up: {powerUpsType}");
        }
    }
    public void UpdatePowerUps()
    {
        List<AllDropItemConfig.PowerUpsType> expiredPowerUps = new List<AllDropItemConfig.PowerUpsType>();

        foreach (var powerUp in activePowerUps.Keys.ToList())
        {
            activePowerUps[powerUp] -= Time.deltaTime;
            // Debug.Log($"Power-up: {powerUp} has {activePowerUps[powerUp]}s left");
            if (activePowerUps[powerUp] <= 0)
            {
                expiredPowerUps.Add(powerUp);
            }
        }

        foreach (var powerUp in expiredPowerUps)
        {
            AllManager.Instance().powerUpManager.DeactivatePowerUpByType(powerUp);
            activePowerUps.Remove(powerUp);
            Debug.Log($"Deactivating power-up: {powerUp}");
        }
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
        if(id == Player_ID.MyPlayerID)
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
}

public class PlayerManager
{
    public Dictionary<string, Player> dictPlayers = new Dictionary<string, Player>();
    GameObject characterPrefab;
    public LevelUpConfig levelUpConfig;
    public PlayerManager(GameObject characterPrefab, LevelUpConfig levelUpConfig)
    {
        this.characterPrefab = characterPrefab;
        this.levelUpConfig = levelUpConfig;
    }
    public int exp = 0;
    public int level = Constants.PlayerBaseLevel;
    public int expRequire = Constants.PlayerBaseExp;
    public float expBoostTime = 0;
    public float expBoostAmount; // Since all player share a single EXP bar, we use the variable here instead of in each player's class
    public void MyUpdate()
    {
        // foreach (var player in dictPlayers.Values)
        // {
            if (expBoostTime > 0){
                expBoostTime -= Time.deltaTime;
                if (expBoostTime < 0)
                {
                    expBoostTime = 0;
                    expBoostAmount = 1f;
                }
            }
        // }
    }
    public void LateUpdate(){}
    public void ProcessExpGain(int expGain)
    {
        expGain = (int)(expGain * (1 + expBoostAmount));
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
                player.health = GetMaxHealthFromLevel();
                player.levelUpEffect = GameObject.Instantiate(player.playerConfig.levelUpEffect, player.playerTrans.position, Quaternion.identity);
                levelUpConfig.OpenMenu();
                Debug.Log("final options real: " + string.Join(", ", levelUpConfig.finalOptions.ToArray()));
                
                SendData<PauseEvent> data = new SendData<PauseEvent>(new PauseEvent());
                SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
                
                
                UIManager._instance.uiLevelUp.OnSetUp(levelUpConfig.finalOptions);
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

        UIManager._instance._fjoystick.input = Vector2.zero;
        UIManager._instance._fjoystick.background.gameObject.SetActive(false);


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
        float boostMultiplier = player.dmgBoostAmount;
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
            if (state.isDead && !c_Controller.goCircleRes.activeSelf) OnDead(player.id);
            //if (player.id == state.player_id) Debug.Log(state.isDead + "/" + player.isDead);
            else if (!state.isDead && c_Controller.goCircleRes.activeSelf) OnRevive(player.id);

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

    public void OnRevive(string id)
    {
        Player player = dictPlayers[id];
        player.isDead = false;
        CharacterControl c_Controller = dictPlayers[id].playerTrans.gameObject.GetComponent<CharacterControl>();
        c_Controller.goCircleRes.SetActive(false);
        c_Controller.charAnim.SetBool("isDead", false);
        if (id == Player_ID.MyPlayerID)
        {
            player.health = (int)(GetMaxHealthFromLevel() * 0.3f);
            UIManager._instance.uiGameplay.UpdateHealthSlider(player.health);
        } 
    }

    public void OnDead(string id)
    {
        CharacterControl c_Controller = dictPlayers[id].playerTrans.gameObject.GetComponent<CharacterControl>();
        c_Controller.goCircleRes.SetActive(true);
        c_Controller.charAnim.SetBool("isDead", true);
    }

    public void ProcessLifeSteal()
    {
        int lifeSteal = Random.Range(0, 100);
        if (lifeSteal <= AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].lifeSteal)
        {
            Debug.Log("Hut dc 1 mau nha may em yeu");
            //AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].health++;
            //if(  AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].health>GetMaxHealthFromLevel())
        }
    }

}