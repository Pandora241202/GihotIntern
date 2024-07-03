using Cinemachine;
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

    public void SpawnPlayer(Vector3 position, string id, int gun_id)
    {
        Player player = this.dictPlayers[id]; 
        player.playerTrans = GameObject.Instantiate(characterPrefab, position, Quaternion.identity).transform;
        player.playerTrans.gameObject.GetComponent<CharacterController>().id = id;
        player.playerTrans.gameObject.GetComponent<CharacterController>().gunId = gun_id;
        Debug.Log("Player: " + player.name + " gun: " + gun_id);
        player.playerTrans.gameObject.GetComponent<CharacterController>().SetGunAndBullet();
        if (id == Player_ID.MyPlayerID) 
            player.playerTrans.Find("CM vcam1").gameObject.GetComponent<CinemachineVirtualCamera>().Priority = 11;
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

    public void UpdatePlayerVelocity(string id, Vector3 velocity, Vector3 position,Quaternion rotation)
    {
        Player player = dictPlayers[id];
        player.playerTrans.gameObject.GetComponent<CharacterController>().velocity = velocity;
        player.playerTrans.position = position;
        player.playerTrans.gameObject.GetComponent<CharacterController>().goChar.transform.rotation = rotation;
    }
}