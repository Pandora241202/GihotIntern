using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuickTimeEventConfig", menuName = "Config/GameEventConfig/QuickTimeEvent")]
public class QuickTimeEventConfig : GameEventConfig
{
    private int score;
    private int goal;
    public override void Activate(GameEvent gameEvent, GameEventData eventData)
    {
        base.Activate(gameEvent, eventData);
        score = eventData.quick.currentScore;
        goal = eventData.quick.goal;

        //render current and target score
    }


    public override void Apply(GameEvent gameEvent)
    {
        base.Apply(gameEvent);
        //update current and target score
    }

    public override void End(GameEvent gameEvent, bool endState)
    {
        base.End(gameEvent, endState);
        if (endState) AllManager._instance.playerManager.ProcessExpGain(AllManager._instance.playerManager.expRequire);
        //else do nothing cause event failed
    }

    public override void UpdateState(GameEvent gameEvent, GameEventData eventData)
    {
        base.UpdateState(gameEvent, eventData);
        gameEvent.timeEnd = eventData.timeToEnd;
        score = eventData.quick.currentScore;
        Debug.Log(score);
    }
}
