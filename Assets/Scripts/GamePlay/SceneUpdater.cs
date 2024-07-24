using UnityEngine;

public class SceneUpdater : MonoBehaviour
{
    public CreepManager creepManager;
    public BulletManager bulletManager;
    public PowerUpManager powerUpManager;
    public PlayerManager playerManager;
    public GameEventManager gameEventManager;
    public RenderManager renderManager;
    // Start is called before the first frame update
    void Start()
    {
        creepManager = new CreepManager(AllManager.Instance().allCreepConfig);
        bulletManager = new BulletManager(AllManager.Instance().gunConfig);
        powerUpManager = new PowerUpManager(AllManager.Instance().allDropItemConfig);
        gameEventManager = new GameEventManager(AllManager.Instance().allGameEventConfig);
        playerManager = AllManager.Instance().playerManager;
        renderManager = new RenderManager(Camera.main, AllManager.Instance().treeLayerMask);
    }

    // Update is called once per frame
    void Update()
    {
        if (AllManager.Instance().isPause) return;
        bulletManager.MyUpdate();
        creepManager.MyUpdate();
        powerUpManager.MyUpdate();
        playerManager.MyUpdate();
        gameEventManager.MyUpdate();
        renderManager.MyUpdate();
    }

    private void FixedUpdate()
    {
        playerManager.MyFixedUpdate();
    }

    private void LateUpdate()
    {
        if (AllManager.Instance().isPause) return;
        bulletManager.LateUpdate();
        creepManager.LateUpdate();
        powerUpManager.LateUpdate();
        playerManager.LateUpdate();
    }
}
