using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Player
{
    public Transform playerTrans;
    public string name;
    public string id;
    public int gunId;
    public GunConfig gunConfig;
    public PlayerConfig config;
    public GameObject levelUpEffect;
    public float lastFireTime = 0f;
    public float lifeSteal;
    public int dmgRecieved = 0;
    public int hpGain = 0;

    // Player stat 
    private float health;
    private float dmgBoostAmount;
    private float speedBoostAmount;
    private float speedBoostByLevelUp;
    //Perm Update
    //public List<int> lsPermUpgrade = new List<int>() { 0, 2, 3, 4, 5,1 };
    
    // Player state
    private Dictionary<AllDropItemConfig.PowerUpsType, float> activePowerUps;
    private GameObject curCreepTarget = null;
    public bool isDead;

    public Vector3 input_velocity = Vector3.zero;
    public Vector3 final_velocity = Vector3.zero;
    public bool collision = false;
    Vector3 normal = Vector3.zero;
    public bool isColliding = false;
    public bool lerp = false;
    public int frameLerp = 16;
    public int elapseFrame = 0;
    public Vector3 lerpVertor = Vector3.zero;
    public Vector3 lerpPosition = Vector3.zero;
    public bool cantMove = false;
    public Dictionary<int, Vector3> collision_plane_normal_dict = new Dictionary<int, Vector3>();
    private int frame = 0;
    public PermUpdateInfo info;
    public Player(string name, string id, int gunId, PlayerConfig config, PermUpdateInfo info)
    {
        this.name = name;
        this.id = id;
        this.gunId = gunId;
        this.config = config;
        health = config.BaseMaxHealth;
        lifeSteal = config.BaseMaxHealth;
        isDead = false;
        levelUpEffect = null;
        activePowerUps = new Dictionary<AllDropItemConfig.PowerUpsType, float>();
        gunConfig = AllManager.Instance().gunConfig;
        this.info = info;
    }
    public float GetSpeedBoost()
    {
        return speedBoostAmount;
    }

    public void Onstart()
    {
        this.health = config.BaseMaxHealth;
        this.lifeSteal = config.BaseMaxHealth;
        activePowerUps.Clear();
        isDead = false;
    }

    public float GetSpeedBoostByLevelUp()
    {
        return speedBoostByLevelUp;
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
            //Debug.Log($"Deactivating power-up: {powerUp}");
        }
    }

    public void SetDamageBoost(float boostAmount)
    {
        this.dmgBoostAmount = boostAmount;
    }

    public void SetSpeedBoost(float boostAmount)
    {
        this.speedBoostAmount = boostAmount;
    }

    public void SetSpeedBoostByLevelUp(float boost)
    {
        this.speedBoostByLevelUp = boost;
    }

    public float GetHealth()
    {
        return this.health;
    }

    public float GetSpeed()
    {
        GameEvent gameEvent = AllManager.Instance().gameEventManager.GetActiveGameEventByType(GameEventManager.GameEventType.Chain);
        if (gameEvent != null)
        {
            return gameEvent.speed;
        }

        return AllManager.Instance().playerManager.GetSpeedFromLevel(config.BaseSpeed) * (1 + this.speedBoostAmount) * (1 + this.speedBoostByLevelUp) ;
    }

    public void ChangeHealth(float healthChangeAmount)
    {
        hpGain = (int)healthChangeAmount;
        health += healthChangeAmount;
        health = Mathf.Min(health, AllManager.Instance().playerManager.GetMaxHealthFromLevel());
        UIManager._instance.uiGameplay.UpdateHealthSlider(health);
        Debug.Log(AllManager.Instance().playerManager.GetMaxHealthFromLevel()+" "+health);
    }

    public float GetDmg()
    {
        float critRate = GetCritRate();
        // Debug.Log("Normal CR: " + critRate);
        critRate += AllManager.Instance().levelUpConfig.critRateIncrements;
        // Debug.Log("CR after buff: " + critRate);
        GunType gunType = gunConfig.lsGunType[gunId];
        float dmg = AllManager.Instance().playerManager.GetDmgFromLevel(gunType.baseDamage, gunType.bulletMultiplier) * (dmgBoostAmount + 1);

        if (critRate >= 1 || Random.Range(0f, 1f) <= critRate) 
        {
            return dmg * (1 + GetCritDmg() +AllManager.Instance().levelUpConfig.critDamageIncrements);
        }

        return dmg;
    }

    public float GetCritRate()
    {
        GunType gunType = gunConfig.lsGunType[gunId];
        int level = AllManager.Instance().playerManager.level;
        return gunType.baseCritRateMultiplier + (level - 1) * 0.033f;
    }

    public float GetCritDmg()
    {
        GunType gunType = gunConfig.lsGunType[gunId];
        int level = AllManager.Instance().playerManager.level;
        return gunType.baseCritDMGMultiplier + (level - 1) * 0.066f;
    }

    public void ProcessDmg(float dmg)
    {
        Debug.Log("Damdge: "+dmg+" Health: "+health);
        if(id == Player_ID.MyPlayerID)
        {
            dmgRecieved = (int)dmg;
            health -= dmg;
            if (health <= 0)
            {
                health = 0;
                isDead = true;
            }
            UIManager._instance.uiGameplay.UpdateHealthSlider(health);
        }
        Debug.Log("Damdge: "+dmg+" Health: "+health);
    }

    GameObject GetTagetObj()
    {
        GunType gunType = gunConfig.lsGunType[gunId];

        Collider[] creepColliders = Physics.OverlapSphere(playerTrans.position, gunType.FireRange, config.CreepLayerMask);

        GameObject targetObj = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider creepCollider in creepColliders)
        {
            float distance = Vector3.Distance(playerTrans.position, creepCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                targetObj = creepCollider.gameObject;
            }
        }

        return targetObj;
    }

    public void Shoot()
    {
        GameObject targetObj = GetTagetObj();

        if (targetObj == null)
        {
            return;
        }

        if (targetObj != curCreepTarget)
        {
            curCreepTarget = targetObj;
        }

        Transform gunTransform = playerTrans.Find("Gun");
        Vector3 directionToTarget = (targetObj.transform.position - gunTransform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        gunTransform.rotation = lookRotation;

        float playerDmg = GetDmg();

        GunType gunType = gunConfig.lsGunType[gunId];
        float Firerate = gunType.Firerate + AllManager.Instance().levelUpConfig.getLevelUpFireRate();
        if (Time.time >= lastFireTime + 1f / Firerate)
        {
            UIManager._instance.MyPlaySfx(gunId + 1 , 0.5f, 0.15f); //Note: gunId - 1 is VERY temporarily since all audio is in a list in UIManager
            gunType.bulletConfig.Fire(gunTransform.position, curCreepTarget.transform.position, playerDmg, "PlayerBullet", playerId: id);
            lastFireTime = Time.time;
        }
        
        //Life Steal
        AllManager._instance.playerManager.ProcessLifeSteal();

        //float angle = Vector3.Angle(gunTransform.forward, directionToTarget);
        //if (angle < 10f)
        //{
        //    AllManager.Instance().bulletManager.SpawnBullet(gunTransform.position, curCreepTarget, gunId);
        //}
    }

    public void SetGunAndBullet()
    {
        GunType gunType = gunConfig.lsGunType[gunId];
        GameObject gun = GameObject.Instantiate(gunType.gunPrefab, playerTrans.position, Quaternion.identity);
        gun.transform.SetParent(playerTrans.Find("Gun"));
    }

    public void Lerp()
    {
        float speed = GetSpeed();

        if ((lerpPosition - playerTrans.position).magnitude <= speed * Time.fixedDeltaTime)
        {
            lerp = false;
            playerTrans.position = lerpPosition;
            final_velocity = input_velocity;
            return;
        }

        lerpPosition += input_velocity * Time.fixedDeltaTime;
        final_velocity = (lerpVertor + input_velocity).normalized * speed;
    }

    public void CalculateVelocity()
    {
        if (lerp)
        {
            Lerp();
        }
        else
        {
            final_velocity = input_velocity;
        }

        normal = Vector3.zero;

        float speed = AllManager.Instance().playerManager.dictPlayers[id].GetSpeed();

        if (collision)
        {
            for (int i = 0; i < collision_plane_normal_dict.Count; i++)
            {
                normal += collision_plane_normal_dict.ElementAt(i).Value;
            }

            final_velocity = (normal + final_velocity.normalized).normalized * speed;
        }

        CharacterControl c = playerTrans.GetComponent<CharacterControl>();

        if (input_velocity != Vector3.zero)
        {
            c.charAnim.SetBool("isRun", true);
            c.goChar.transform.rotation = Quaternion.LookRotation(input_velocity);
        }
        else
        {
            c.charAnim.SetBool("isRun", false);
        }


        if (id != Player_ID.MyPlayerID) return;

        if (frame % 3 == 0)
        {
            float horizontal = c.joystick.Horizontal;
            float vertical = c.joystick.Vertical;

            Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

            Vector3 v = direction * speed;

            SendData<PlayerState> data = new SendData<PlayerState>(new PlayerState(playerTrans.position, v,
                Quaternion.LookRotation(direction), AllManager._instance.playerManager.dictPlayers[id], true));
            SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
        }

        frame++;
    }
}

public class PlayerManager
{
    public Dictionary<string, Player> dictPlayers = new Dictionary<string, Player>();
    private GameObject characterPrefab;
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
    public bool isLevelUp;
    public float correctPositionTime = 0;

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

    public void MyFixedUpdate()
    {
        if (AllManager.Instance().isPause) return;

        foreach (Player player in dictPlayers.Values)
        {
            if (player.playerTrans == null)
            {
                return;
            }
            player.CalculateVelocity();
        }

        if (correctPositionTime < Time.fixedDeltaTime * 10)
        {
            GameEvent gameEvent = AllManager.Instance().gameEventManager.GetActiveGameEventByType(GameEventManager.GameEventType.Chain);
            if (gameEvent != null)
            {
                gameEvent.FixedApply();
            }

            foreach (Player player in dictPlayers.Values)
            {
                player.playerTrans.position += player.final_velocity * Time.fixedDeltaTime;
            }
            correctPositionTime += Time.fixedDeltaTime;
        }
    }

    public void LateUpdate(){}
    
    public void ProcessExpGain(int expGain)
    {
        expGain = (int)(expGain * (1 + expBoostAmount));
        exp += expGain;
        UIManager._instance.uiGameplay.UpdateLevelSlider(exp);
        if (exp >= expRequire)
        {
            isLevelUp = true;
            expRequire = Constants.PlayerBaseExp + (level - 1) * Constants.ExpIncrement + Constants.ScalingMultiplierExp * (level - 1) * (level - 1);
            level++;
            UIManager._instance.uiGameplay.LevelUpdateSlider(expRequire);
            exp = 0;
            
            SendData<LevelUpEvent> data = new SendData<LevelUpEvent>(new LevelUpEvent());
            SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
            levelUpConfig.OpenMenu();
            // UIManager._instance.uiLevelUp.OnSetUp(levelUpConfig.finalOptions);
            UIManager._instance.OnInstanceLevel(levelUpConfig.finalOptions);
            
            foreach (var pair in  dictPlayers)
            {
                Player player = pair.Value;
                if(player.id == Player_ID.MyPlayerID) player.ChangeHealth(GetMaxHealthFromLevel());
                player.levelUpEffect = GameObject.Instantiate(player.config.LevelUpEffect, player.playerTrans.position, Quaternion.identity);
               
                //Debug.Log("final options real: " + string.Join(", ", levelUpConfig.finalOptions.ToArray()));
               
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

    public float GetSpeedFromLevel(float baseSpeed)
    {
        return baseSpeed + (level - 1) / 2;
    }

    public float GetDmgFromLevel(float baseDmg, int bulletMultiplier)
    {
        return baseDmg + (level - 1) * bulletMultiplier;
    }

    public void SpawnPlayer(Vector3 position, string id, int gun_id)
    {
        Player player = this.dictPlayers[id];
        player.playerTrans = GameObject.Instantiate(characterPrefab, position, Quaternion.identity).transform;
        player.playerTrans.gameObject.GetComponent<CharacterControl>().id = id;
        player.gunId = gun_id;
        player.playerTrans.gameObject.GetComponent<CharacterControl>().txtName.text=
            player.name;
     
        player.SetGunAndBullet();
        
        if (id == Player_ID.MyPlayerID)
        {
            player.playerTrans.Find("CM vcam1").gameObject.GetComponent<CinemachineVirtualCamera>().Priority = 11;
        }   
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


    public void AddPlayer(string name, string id, int gunId, PlayerConfig playerConfig, PermUpdateInfo info = null)
    {
        Player newPlayer = new Player(name, id, gunId, playerConfig, info);
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
            float speedBoost = state.speedBoost;
            if(player.id != Player_ID.MyPlayerID) player.SetSpeedBoostByLevelUp(speedBoost);
            float speed = player.GetSpeed();
            player.isDead = state.isDead;
            CharacterControl c_Controller = player.playerTrans.gameObject.GetComponent<CharacterControl>();
            player.input_velocity = state.velocity;
            //c_Controller.goChar.transform.rotation = state.rotation;
            if (!player.isColliding && player.id != Player_ID.MyPlayerID)
            {
                if ((player.playerTrans.position - state.position).magnitude <= Time.fixedDeltaTime * speed)
                {
                    player.lerp = false;
                    player.playerTrans.position = state.position;
                }
                else
                {
                    player.lerp = true;
                    player.elapseFrame = 0;
                    player.lerpPosition = state.position;
                    player.lerpVertor = (player.lerpPosition - player.playerTrans.position);
                }
            }

            correctPositionTime = 0;
            if (state.isFire)
            {
                player.Shoot();
                if (dictPlayers.Count==1&&AllManager.Instance().droneManager.drone!=null)
                {
                    Debug.Log("Drone");
                    AllManager.Instance().droneManager.Shoot();
                }
               
            }
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
            player.ChangeHealth(GetMaxHealthFromLevel() * 0.3f);
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
        if (lifeSteal <= dictPlayers[Player_ID.MyPlayerID].lifeSteal)
        {
            Debug.Log("Hut dc 1 mau nha may em yeu");
            dictPlayers[Player_ID.MyPlayerID].ChangeHealth(1);
        }
    }

}