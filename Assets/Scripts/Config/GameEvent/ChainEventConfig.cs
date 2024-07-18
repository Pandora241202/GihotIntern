using UnityEngine;

[CreateAssetMenu(fileName = "ChainEventConfig", menuName = "Config/GameEventConfig/ChainEvent")]
public class ChainEventConfig : GameEventConfig
{
    [SerializeField] private Vector3 defaultAnchorPos;

    public override void Activate(GameEvent gameEvent, GameEventData eventData)
    {
        gameEvent.anchorPos = defaultAnchorPos;
    }
}
