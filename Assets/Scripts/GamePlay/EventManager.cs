using System.Collections.Generic;
using UnityEngine;

public class GameEvent
{
    public GameEventConfig config;

    // Chain Event
    public Transform anchorTrans;
    public Dictionary<string, Transform> connectLineTransDict = new Dictionary<string, Transform>();
    public float speed;

    public GameEvent(GameEventConfig config)
    {
        this.config = config;
    }

    public void Activate()
    {
        config.Activate(this);
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

    public void ActivateEventByType(GameEventType type)
    {
        GameEventConfig config = gameEventConfigs[(int) type];
        GameEvent gameEvent = new GameEvent(config);
        gameEvent.Activate();
        gameEventDict.Add((int)type, gameEvent);
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

    public GameEvent GetActiveGameEventByType(GameEventType type)
    {
        GameEvent gameEvent = null;
        gameEventDict.TryGetValue((int) type, out gameEvent);
        return gameEvent;
    }
}
