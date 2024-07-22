using System.Collections.Generic;
using UnityEngine;

public class GameEvent
{
    public GameEventConfig config;
    public int id;
    public float timeEnd;

    // ShareAttribute Event
    public int curHP;
    public int maxHP;

    // Chain Event
    public Transform anchorTrans;
    public Dictionary<string, Transform> connectLineTransDict = new Dictionary<string, Transform>();
    public float speed;

    public GameEvent(GameEventConfig config)
    {
        this.config = config;
        
        //config.Activate(this); // Assign all need attribute for event in here
    }

    public void Activate()
    {
        config.Activate(this);
    }

    public void Apply()
    {
        config.Apply(this);
    }

    public void End(bool endState)
    {
        config.End(this, endState);
    }

    public void FixedApply()
    {
        config.FixedApply(this);
    }

    public void FixedApply()
    {
        config.FixedApply(this);
    }
}

public class GameEventManager
{
    private GameEventConfig[] gameEventConfigs;

    private Dictionary<int, GameEvent>
        gameEventDict = new Dictionary<int, GameEvent>();

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

    public void ActivateEventByType(GameEventType type, GameEventData ev)
    {
        GameEventConfig config = gameEventConfigs[(int)type];
        GameEvent gameEvent = new GameEvent(config);
        gameEvent = new GameEvent(gameEventConfigs[ev.id]);
        gameEvent.config.Activate(gameEvent, ev);
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

    public void ClearEventByType(int type, bool endState)
    {
        GameEvent gameEvent = gameEventDict[type];
        gameEventDict.Remove(type);
        gameEvent.End(endState);
    }

    public void UpdateEventState(EventsInfo info)
    {
        //process info
        //Debug.Log(info.timeToNextEvent);
        if (info.timeToNextEvent<=2f)
        {
            //todo
            UIManager._instance.uiGameplay.ShowText();
        }

        foreach (var ev in info.event_info)
        {
            //GameEventType id = (GameEventType)ev.id;
            if (!gameEventDict.ContainsKey(ev.id))
            {
               ActivateEventByType((GameEventType)ev.id, ev);
            }
            else
            {
                if (ev.end)
                {
                    ClearEventByType(ev.id, ev.endState);
                }
                else
                {
                    GameEvent gameEvent = gameEventDict[ev.id];
                    gameEvent.config.UpdateState(gameEvent, ev);
                }
            }
         
        }
    }

    public void FixedUpdate()
    {
        foreach (var pair in gameEventDict)
        {
            GameEvent gameEvent = pair.Value;
            gameEvent.FixedApply();
        }
    }

    public GameEvent GetActiveGameEventByType(GameEventType type)
    {
        GameEvent gameEvent = null;
        gameEventDict.TryGetValue((int)type, out gameEvent);
        return gameEvent;
    }
}