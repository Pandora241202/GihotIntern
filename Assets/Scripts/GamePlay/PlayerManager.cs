using UnityEngine;

class Player
{
    public Transform playerTrans;
}

class PlayerManager
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