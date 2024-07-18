using System.Collections.Generic;
using UnityEngine;

public class GameEvent
{
    public GameEventConfig config;

    public Vector3 anchorPos;

    public GameEvent(GameEventConfig config)
    {
        this.config = config;
        config.Activate(this); // Assign all need attribute for event in here
    }

    public void Apply()
    {
        config.Apply(this);
    }

    public void End()
    {
        config.End(this);
    }
}

public class GameEventManager
{
    private GameEventConfig[] gameEventConfigs;
    private Dictionary<int, GameEvent> gameEventDict = new Dictionary<int, GameEvent>(); // key is the sharedId generate and manage by server

    public enum GameEventType
    {
        Chain,
        LimitedVision,
        SharedAttributes,
        OnePermadeath,
        QuickTimeEvent,
        RaidBoss
    }

    public GameEventManager(AllGameEventConfig allGameEventConfig)
    {
        gameEventConfigs = allGameEventConfig.GameEventConfigs;
    }

    public void ActivateEventByType(GameEventType type, int sharedId)
    {
        GameEventConfig config = gameEventConfigs[(int) type];
        GameEvent gameEvent = new GameEvent(config);
        gameEventDict.Add(sharedId, gameEvent);
    }

    public void MyUpdate()
    {
        foreach (var pair in gameEventDict) 
        {
            GameEvent gameEvent = pair.Value;
            gameEvent.Apply();
        }
    }

    public void ClearEventBySharedId(int sharedId)
    {
        GameEvent gameEvent = gameEventDict[sharedId];
        gameEvent.End();
    }

    public void UpdateEventState(EventsInfo info)
    {
        //process info
        // Debug.Log("Time to next event: " + info.timeToNextEvent);
        foreach(var ev in info.event_info)
        {
            GameEventType id = (GameEventType)ev.id;
            Debug.Log("Event is: " + id + "/" + ev.id);
            switch(id)
            {
                case GameEventType.Chain:
                    break;
                case GameEventType.LimitedVision:
                    break;
                case GameEventType.SharedAttributes:
                    ShareAttrEventData sharedAttrData = ev.share;
                    if (sharedAttrData != null) Debug.Log("Shared Attribute Data is not null");
                    Debug.Log(sharedAttrData.curHP + " / " +  sharedAttrData.maxHP);
                    break;
                case GameEventType.QuickTimeEvent:
                    Debug.Log("Quick Time Event quick: " + ev);
                    QuickTimeEventData quickTimeData = ev.quick;
                    Debug.Log("Current Event: " + quickTimeData.currentEvent);
                    Debug.Log("Current Goal: " + quickTimeData.goal);
                    Debug.Log("Current Starting Score: " + quickTimeData.startingScore);
                    //_____________________________//
                    Debug.Log("Enemies killed in the qte: " + quickTimeData.enemyKill);
                    break;
                case GameEventType.OnePermadeath:
                    break;
            }
        }
    }
}
