using UnityEngine;

public class GameEventConfig: ScriptableObject
{
    [SerializeField] private float expGain;
    public float ExpGain => expGain;

    public virtual void Activate(GameEvent gameEvent, GameEventData eventData)
    {
        UIManager._instance.uiGameplay.OnEventStart(gameEvent.id,(int)gameEvent.timeEnd);
    } // Assign all need attribute for event in here

    public virtual void Apply(GameEvent gameEvent)
    {
        GameObject item;
        if (UIManager._instance.uiGameplay.lsGOEvent.TryGetValue(gameEvent.id,out item))
        {
            item.GetComponent<ItemEvent>().OnUpdateFill(gameEvent.timeEnd);
        }
        
    } // Apply the event untill event end

    public virtual void End(GameEvent gameEvent, bool endState)
    {
        Destroy(UIManager._instance.uiGameplay.lsGOEvent[gameEvent.id].gameObject);
        UIManager._instance.uiGameplay.lsGOEvent.Remove(gameEvent.id);
    } // End event, do sth???

    public virtual void UpdateState(GameEvent gameEvent, GameEventData eventData) { }
}