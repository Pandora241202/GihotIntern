using UnityEngine;

[CreateAssetMenu(fileName = "AllGameEventConfig", menuName = "Config/GameEventConfig/AllGameEvent")]
public class AllGameEventConfig : ScriptableObject
{
    [SerializeField] GameEventConfig[] gameEventConfigs;

    public GameEventConfig[] GameEventConfigs => gameEventConfigs;
}
