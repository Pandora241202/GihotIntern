using UnityEngine;

public class GameEventConfig: ScriptableObject
{
    [SerializeField] private float expGain;
    public float ExpGain => expGain;

    public virtual void Activate(GameEvent gameEvent, GameEventData eventData)  // Assign all need attribute for event in here
    {
        gameEvent.id = eventData.id;
        gameEvent.timeEnd = eventData.timeToEnd;
        UIManager._instance.uiGameplay.OnEventStart(gameEvent.id,(int)gameEvent.timeEnd);
    }

    public virtual void Apply(GameEvent gameEvent)  // Apply the event untill event end
    {
        GameObject item;
        if (UIManager._instance.uiGameplay.lsGOEvent.TryGetValue(gameEvent.id,out item))
        {
            item.GetComponent<ItemEvent>().OnUpdateFill(gameEvent.timeEnd);
        }
        
    }

    public virtual void End(GameEvent gameEvent, bool endState) // End event, do sth???
    {
        Destroy(UIManager._instance.uiGameplay.lsGOEvent[gameEvent.id].gameObject);
        UIManager._instance.uiGameplay.lsGOEvent.Remove(gameEvent.id);
    } 

    public virtual void UpdateState(GameEvent gameEvent, GameEventData eventData) 
    {
        gameEvent.timeEnd = eventData.timeToEnd;
    }

    public virtual void FixedApply(GameEvent gameEvent) { } // Apply the event in fixedUpdate untill event end
}