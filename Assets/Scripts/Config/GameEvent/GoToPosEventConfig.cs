using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "GoToPosEventConfig", menuName = "Config/GameEventConfig/GoToPosEvent")]
public class GoToPosEventConfig : GameEventConfig
{
    public override void Activate(GameEvent gameEvent, GameEventData eventData)
    {
        base.Activate(gameEvent, eventData);
        AllManager._instance.SpawnGoToPosEvent(eventData.goToPos.target1,eventData.goToPos.target2);
        
    }

    public override void Apply(GameEvent gameEvent)
    {
        base.Apply(gameEvent);
        
    }

    public override void End(GameEvent gameEvent, bool endState)
    {
        base.End(gameEvent, endState);
        foreach (var item in AllManager._instance.lsGoToEvent)
        {
            Destroy(item);
        }
        AllManager._instance.lsGoToEvent.Clear();
        if (endState) AllManager._instance.playerManager.ProcessExpGain(AllManager._instance.playerManager.expRequire);
    }

    public override void UpdateState(GameEvent gameEvent, GameEventData eventData)
    {
        base.UpdateState(gameEvent, eventData);
        
    }
    
}
