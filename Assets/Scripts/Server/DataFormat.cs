using System;
using UnityEngine;
[Serializable]
public class SendData<T>
{
    public string player_id = Player_ID.MyPlayerID;
    public string sessionId = Player_ID.SessionId;
    public T _event;
    public SendData(T _event)
    {
        this._event = _event;
    }
}

[Serializable]
public class FirstConnect
{
    public string event_name = "first connect";
    public string name;
    public FirstConnect(string name)
    {
        this.name = name;
    }
}

[Serializable]
class MoveEvent
{
    public string event_name = "move";
    [field: SerializeField] Vector3 velocity;
    [field: SerializeField] Quaternion rotation;
    public MoveEvent(Vector3 velocity, Quaternion rotation)
    {
        this.velocity = velocity;
        this.rotation = rotation;

    }
}
[Serializable]
class PositionEvent
{
    public string event_name = "position";
    [field: SerializeField] Vector3 position;
    public PositionEvent(Vector3 position)
    {
        this.position = position;
    }
}

[Serializable]
class ChoseGunEvent
{
    public string event_name = "choosegun";
    public int gun_id;
    public ChoseGunEvent(int gunid)
    {
        this.gun_id = gunid;
    }
}

[System.Serializable]
class ItemPlayerEvent
{
    public string event_name;
    public bool value;
    public string player_id;
    public string host_id;
    public ItemPlayerEvent(string event_name, bool value, string id)
    {
        this.event_name = event_name;
        this.value = value;
        this.host_id = Player_ID.MyPlayerID;
        this.player_id = id;
    }
}
[System.Serializable]
class OnlineLobbyEvent
{
    public string event_name;
    public bool value;
    public string name;
    public string game_mode;
    public OnlineLobbyEvent(string event_name, bool value, string name = "", string game_mode = "")
    {
        this.event_name = event_name;
        this.value = value;
        this.name = name;
        this.game_mode = game_mode;
    }
}
[System.Serializable]
class PlayerIdEvent
{
    public string event_name;
    public string player_id = Player_ID.MyPlayerID;
    public PlayerIdEvent(string event_name)
    {
        this.event_name = event_name;
    }
}
[System.Serializable]
public class EventName
{
    public string event_name;
    public EventName(string event_name)
    {
        this.event_name = event_name;
    }
}

[System.Serializable]
class First_Connect
{
    public string id;
    public string player_name;
    public int gunId;
}

[System.Serializable]
class Rooms
{
    public Room[] rooms;
}

[System.Serializable]
class SimplePlayerInfo
{
    public string player_id;
    public string player_name;
    public string host_id;
    public int gun_id;
}

[System.Serializable]
class SimplePlayerInfoList
{
    public SimplePlayerInfo[] players;
}

[System.Serializable]
public class Room
{
    public string id;
    public string name;
    public string game_mode;
}

[System.Serializable]
public class CreepSpawnInfo
{
    public int type_int;
    [field: SerializeField] public Vector3 spawn_pos;
    public float time;
    public int shared_id;
}

[Serializable]
public class PlayerSpawnPos
{
    public string player_id;
    [field: SerializeField] public Vector3 spawn_pos;
    public int gun_id;
}

[Serializable]
public class AllPlayerSpanwPos
{
    public PlayerSpawnPos[] data;
}

[Serializable]
public class PlayerState
{
    public string event_name = "player state";
    public string player_id;
    public int gun;
    public bool isDead;
    [field: SerializeField] public Vector3 position;
    [field: SerializeField] public Vector3 velocity;
    [field: SerializeField] public Quaternion rotation;
    public bool isFire;
    public float speedBoost;
    public float maxHP;
    public PlayerState(Vector3 position, Vector3 velocity, Quaternion rotation, Player player, bool isFire)
    {
        this.position = position;
        this.velocity = velocity;
        this.rotation = rotation;
        this.isDead = player.isDead;
        this.isFire = isFire;
        this.speedBoost = player.GetSpeedBoostByLevelUp();
        this.maxHP = AllManager.Instance().playerManager.GetMaxHealthFromLevel();

    }
}
[Serializable]
public class PlayersState
{
    public long time;
    public PlayerState[] states;
}

[Serializable]
public class PlayerPosition
{
    public string event_name = "player position";
    public string player_id;
    [field: SerializeField] public Vector3 position;
    public PlayerPosition(Vector3 position)
    {
        this.position = position;
    }
}

[Serializable]
public class PlayersPosition
{
    public PlayerPosition[] players_position;
}

[Serializable]
public class PowerUpSpawnInfo
{
    public int type_int;
    [field: SerializeField] public Vector3 spawn_pos;
    public int shared_id;

    public PowerUpSpawnInfo(AllDropItemConfig.PowerUpsType type, Vector3 spawn_pos)
    {
        this.type_int = (int) type;
        this.spawn_pos = spawn_pos;
    }
}

[Serializable]
public class PowerUpPickInfo
{
    public string event_name = "power up pick";
    public string player_id;
    public int shared_id;

    public PowerUpPickInfo(string player_id, int shared_id)
    {
        this.player_id = player_id;
        this.shared_id = shared_id;
    }
}

[Serializable]
public class CreepDestroyInfo
{
    public string event_name = "creep destroy";
    public int shared_id;
    public PowerUpSpawnInfo power_up_spawn_info;

    public CreepDestroyInfo(int shared_id, PowerUpSpawnInfo power_up_spawn_info)
    {
        this.shared_id = shared_id;
        this.power_up_spawn_info = power_up_spawn_info;
    }
}

[Serializable]
public class PlayerOutEvent
{
    public string event_name = "player out";
}

[Serializable]
public class QuitEvent
{
    public string event_name = "quit";
}

[Serializable]
public class PauseEvent
{
    public string event_name = "pause";
}
[Serializable]
public class LevelUpEvent
{
    public string event_name = "level up";
}
[Serializable]
public class ChooseLevelUpEvent
{
    public string event_name = "choose level up";
}

[Serializable]
public class ResumeEvent
{
    public string event_name = "resume";
}

[Serializable]
public class ReviveEvent
{
    public string event_name = "revive";
    public string revive_player_id;
    public ReviveEvent(string id)
    {
        this.revive_player_id = id;
    }
}

[Serializable]
public class GameEnd
{
    public Score[] result;
}

[Serializable]
public class Score
{
    public string player_id;
    public int enemy_kill;
}
[Serializable]
public class PingEvent
{
    public string event_name = "ping";
}

[Serializable]
public class ResumeInfo
{
    public bool isResume;
    public float time;
}


[Serializable]
public class GameEventData
{
    public int id;
    public float timeToEnd;
    public bool endState;
    public bool end;
    public ShareAttrEventData share;
    public ChainEventData chain;
    public LimitedVisionEventData limited;
    public RaidBossEventData raid;
    public QuickTimeEventData quick;
    public GoToPosEvent goToPos;
}

[Serializable]
public class EventsInfo
{
    public GameEventData[] event_info;
    public float timeToNextEvent;
}

[Serializable]
public class ShareAttrEventData
{
    public float curHP;
    public float maxHP;
}

[Serializable]
public class ShareAttrEventDamaged
{
    public int id = 2;
    public string event_name = "game event";
    public int damage;
    public ShareAttrEventDamaged(int dmg)
    {
        this.damage = dmg;
    }
}
[Serializable]
public class GoToPosEvent
{
    [field: SerializeField] public Vector3 target1;
    [field: SerializeField] public Vector3 target2;
}

[Serializable]
public class ChainEventData 
{

}

[Serializable]
public class LimitedVisionEventData 
{

}

[Serializable]
public class RaidBossEventData 
{

}

[Serializable]
public class QuickTimeEventData 
{
    public int currentScore;
    public int goal;
}


[Serializable]
public class GameStateData
{
    public PlayersState player_states;
    public bool isPause;
    public bool isLevelUp;
    public ResumeInfo resume;
    public CreepSpawnInfo[] creep_spawn_infos;
    public CreepDestroyInfo[] creep_destroy_infos;
    public PowerUpPickInfo[] power_up_pick_infos;
    public EventsInfo game_event;
}


[Serializable]
public class GameState
{
    public GameStateData state;
}