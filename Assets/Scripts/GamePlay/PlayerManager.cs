using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player
{
    public Transform playerTrans;
    public string name;
    public string id;
}

public class PlayerManager
{
    public Dictionary<string, Player> dictPlayers = new Dictionary<string, Player>();
    public PlayerManager()
    {

    }


    public void RemovePlayer(string id)
    {
        this.dictPlayers.Remove(id);
    }
    public void AddPlayer(string name, string id)
    {
        Player newplayer = new Player();
        newplayer.name = name;
        newplayer.id = id;
        dictPlayers.Add(id,newplayer);
    }
}