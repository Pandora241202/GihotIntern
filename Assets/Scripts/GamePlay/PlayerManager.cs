using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public Transform playerTrans;
    public string name;
    public string id;
}

public class PlayerManager
{
    public List<Player>lsPlayers=new List<Player>();
    int controlPlayerId;

    public PlayerManager()
    {

    }
    
    public Player GetControlPlayer()
    {
        return lsPlayers[controlPlayerId];
    }

    public void AddPlayer(string name, string id)
    {
        Player newplayer = new Player();
        newplayer.name = name;
        newplayer.id = id;
        lsPlayers.Add(newplayer);
    }
}