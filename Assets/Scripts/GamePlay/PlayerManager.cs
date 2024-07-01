using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player
{
    public Transform playerTrans;
    public string name;
    public string id;
    public int gunId;
    public GunConfig gunConfig;
    public Player(string name, string id, int gunId)
    {
        this.name = name;
        this.id = id;
        this.gunId = gunId;

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

    public void SpawnPlayer(Vector3 position, string id)
    {
        Player player = this.dictPlayers[id];
        player.playerTrans = GameObject.Instantiate(characterPrefab, position, Quaternion.identity).transform;
        player.playerTrans.gameObject.GetComponent<CharacterController>().id = id;
    }

    public void RemovePlayer(string id)
    {
        this.dictPlayers.Remove(id);
    }
    public void AddPlayer(string name, string id, int gunId)
    {
        Player newPlayer = new Player(name, id, gunId);
        // newplayer.name = name;
        // newplayer.id = id;
        dictPlayers.Add(id,newPlayer);
    }


}