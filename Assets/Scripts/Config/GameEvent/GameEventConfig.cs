using UnityEngine;

public class GameEventConfig: ScriptableObject
{
    [SerializeField] private float expGain;
    public float ExpGain => expGain;

    public virtual void Activate(GameEvent gameEvent) { } // Assign all need attribute for event in here

    public virtual void Apply(GameEvent gameEvent) { } // Apply the event untill event end

    public virtual void End(GameEvent gameEvent) { } // End event, do sth???
}