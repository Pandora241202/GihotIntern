using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneUpdater : MonoBehaviour
{
    public CreepManager creepManager;
    public BulletManager bulletManager;
    // Start is called before the first frame update
    void Start()
    {
        creepManager = new CreepManager(AllManager.Instance().allCreepConfig);
        bulletManager = new BulletManager(AllManager.Instance().gunConfig);
    }

    // Update is called once per frame
    void Update()
    {
        bulletManager.MyUpdate();
        creepManager.MyUpdate();
    }

    private void LateUpdate()
    {
        bulletManager.LateUpdate();
        creepManager.LateUpdate();
    }
}
