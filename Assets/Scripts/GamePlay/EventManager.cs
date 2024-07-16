using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using static CreepManager;

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
}
