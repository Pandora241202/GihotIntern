using UnityEngine;

public class Player
{
    public Transform playerTrans;
    public string name;
    public string id;
}

public class PlayerManager
{
    Player[] players;
    int controlPlayerId;

    public PlayerManager()
    {

    }

    public Player[] Players => players;


    public Player GetControlPlayer()
    {
        return players[controlPlayerId];
    }
}