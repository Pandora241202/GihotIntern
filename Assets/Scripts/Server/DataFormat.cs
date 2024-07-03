using System;
using UnityEngine;
[Serializable]
public class SendData<T>
{
    public string player_id = Player_ID.MyPlayerID;
    public T _event;
    public SendData(T _event)
    {
        this._event = _event;
    }
}

[Serializable]
class MoveEvent
{
    public string event_name = "move";
    [field: SerializeField] Vector3 velocity;
    [field: SerializeField] Vector3 position;
    [field: SerializeField] Quaternion rotation;
    public MoveEvent(Vector3 velocity, Vector3 position,Quaternion rotation)
    {
        this.velocity = velocity;
        this.position = position;
        this.rotation = rotation;

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
    public ItemPlayerEvent(string event_name,bool value,string id)
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
    public int creepTypeInt;
    [field: SerializeField] public Vector3[] spawnPos;
    public float time;
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
public class PlayerMove
{
    public string player_id;
    [field: SerializeField] public Vector3 velocity;
    [field: SerializeField] public Vector3 position;
    [field: SerializeField] public Quaternion rotation;
}