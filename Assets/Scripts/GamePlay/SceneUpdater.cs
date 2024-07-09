using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneUpdater : MonoBehaviour
{
    public CreepManager creepManager;
    public BulletManager bulletManager;
    public PowerUpManager powerUpManager;
    // Start is called before the first frame update
    void Start()
    {
        creepManager = new CreepManager(AllManager.Instance().allCreepConfig);
        bulletManager = new BulletManager(AllManager.Instance().gunConfig);
        powerUpManager = new PowerUpManager(AllManager.Instance().allDropItemConfig);
    }

    // Update is called once per frame
    void Update()
    {
        if (AllManager.Instance().isPause) return;
        bulletManager.MyUpdate();
        creepManager.MyUpdate();
        powerUpManager.MyUpdate();
    }

    private void LateUpdate()
    {
        if (AllManager.Instance().isPause) return;
        bulletManager.LateUpdate();
        creepManager.LateUpdate();
        powerUpManager.LateUpdate();
    }
}
