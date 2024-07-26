using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "GoToPosEventConfig", menuName = "Config/GameEventConfig/GoToPosEvent")]
public class GoToPosEventConfig : GameEventConfig
{
    public GameObject prefArrow;
   
    public override void Activate(GameEvent gameEvent, GameEventData eventData)
    {
        base.Activate(gameEvent, eventData);
        AllManager._instance.lsArrow.Clear();
        AllManager._instance.lsGoToEvent.Clear();
        AllManager._instance.SpawnGoToPosEvent(eventData.goToPos.target1,eventData.goToPos.target2);
        GameObject goArrow = Instantiate(prefArrow);
       
       
        GameObject goArrow1 = Instantiate(prefArrow);
        
        
        AllManager._instance.lsArrow.Add(goArrow);
        AllManager._instance.lsArrow.Add(goArrow1);
       
        goArrow.transform.SetParent(AllManager._instance.playerManager.dictPlayers[Player_ID.MyPlayerID].playerTrans);
        goArrow1.transform.SetParent(AllManager._instance.playerManager.dictPlayers[Player_ID.MyPlayerID].playerTrans);
        goArrow.transform.localPosition = new Vector3(-1f, 2.7f, 0);
        goArrow1.transform.localPosition = new Vector3(1f, 2.7f, 0);
    }

    public override void Apply(GameEvent gameEvent)
    {
        base.Apply(gameEvent);
        for (int i = 0; i < 2; i++)
        {
           //lsArrow[i].transform.position = new Vector3(0, 2.55f, 0);
            Vector3 directionToTarget = (AllManager._instance.lsGoToEvent[i].transform.position - AllManager._instance.lsArrow[i].transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            AllManager._instance.lsArrow[i].transform.rotation = lookRotation;
        }
        
    }

    public override void End(GameEvent gameEvent, bool endState)
    {
        base.End(gameEvent, endState);
        int i = 0;
        foreach (var item in AllManager._instance.lsGoToEvent)
        {
            Destroy(item);
            Destroy( AllManager._instance.lsArrow[i]);
            i++;
        }
        AllManager._instance.lsGoToEvent.Clear();
        AllManager._instance.lsArrow.Clear();
        if (endState) AllManager._instance.playerManager.ProcessExpGain(AllManager._instance.playerManager.expRequire);
    }

    public override void UpdateState(GameEvent gameEvent, GameEventData eventData)
    {
        base.UpdateState(gameEvent, eventData);
        
    }
    
}
