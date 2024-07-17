using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuickTimeEventConfig", menuName = "Config/GameEventConfig/QuickTimeEvent")]
public class QuickTimeEventConfig : GameEventConfig
{
    public string[] quickTimeEvents = {"Kill Enemies", "Level Up"};
    private string randomEvent;

    // All Quick Time Events will have a time limit of 30 seconds
    [SerializeField] private float timeLimit; 
    public float TimeLimit => timeLimit;

    // Event: Kill X enemies in 30 seconds  
    [SerializeField] private int enemiesToKill;
    [SerializeField] private int currentKills;

    public string GetRandomQuickTimeEvent()
    {
        // Choose a random event name from the array
        int randomIndex = UnityEngine.Random.Range(0, quickTimeEvents.Length);
        return quickTimeEvents[randomIndex];
    }
    public override void Activate(GameEvent gameEvent)
    {
        base.Activate(gameEvent);
        randomEvent = GetRandomQuickTimeEvent();
        Debug.Log($"Activated Quick Time Event: {randomEvent}");
        //TODO: time start(?)
        switch (randomEvent)
        {
            case "Kill Enemies":
                currentKills = 0;
                enemiesToKill = UnityEngine.Random.Range(30, 50);
                break;
            case "Level Up":
                break;
            default:
                break;
        }
    }
    public override void End(GameEvent gameEvent)
    {
        base.End(gameEvent);
        switch (randomEvent) 
        {
            case "Kill Enemies":
                if (currentKills >= enemiesToKill)
                {
                    Debug.Log("Quick Time Event: Kill Enemies completed!");
                    //TODO: add exp
                }
                else
                {
                    Debug.Log("Quick Time Event: Kill Enemies failed!");
                }
            break;

            case "Level Up":
            break;
        }
        randomEvent = "";
    }
    public void OnEnemyKilled()
    {
        if (randomEvent == "Kill Enemies")
        {
            currentKills++;
            Debug.Log($"Enemies Killed: {currentKills}/{enemiesToKill}");
        }
    }

}
