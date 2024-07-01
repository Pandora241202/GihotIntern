using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private GameObject prefabBullet;
    [SerializeField] LayerMask creepLayerMask;
    [SerializeField] int gunId;
    GameObject curCreepTarget = null;
    public string id;
    int frame = 0;
    public Vector3 velocity = new Vector3(0, 0, 0);
    //public static CharacterController _instance { get; private set; }
    //public static CharacterController Instance()
    //{
    //    if (_instance == null)
    //    {
    //        _instance = GameObject.FindAnyObjectByType<CharacterController>();
    //    }
    //    return _instance;
    //}

    private void Awake()
    {
        //  SocketCommunication.GetInstance();
    }

    private void Update()
    {
        transform.position += this.velocity;
        Shoot();
    }

    private void FixedUpdate()
    {
        if (id != Player_ID.MyPlayerID) return;
        //float horizontal = _joystick.Horizontal;
        //float vertical = _joystick.Vertical;
        //Vector3 direction = new Vector3(horizontal, 0, vertical);
        //transform.position += direction * speed * Time.deltaTime;
        if (frame % 2 == 0)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(horizontal, 0, vertical) * speed;
            SendData<MoveEvent> data = new SendData<MoveEvent>(new MoveEvent(direction));
            SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
        }
        frame++;
    }


    GameObject GetTagetObj()
    {
        GunType gunType = AllManager.Instance().gunConfig.lsGunType[gunId];

        Collider[] creepColliders = Physics.OverlapSphere(transform.position, gunType.FireRange, creepLayerMask);

        GameObject targetObj = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider creepCollider in creepColliders)
        {
            float distance = Vector3.Distance(transform.position, creepCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                targetObj = creepCollider.gameObject;
            }
        }

        Debug.Log("find target" + targetObj?.GetInstanceID().ToString());
        return targetObj;
    }

    void Shoot()
    {
        GameObject targetObj = GetTagetObj();

        if (targetObj == null) 
        {
            return;
        }

        if (targetObj != curCreepTarget) 
        {
            CreepManager creepManager = AllManager.Instance().creepManager;
            creepManager.MarkTargetCreepById(targetObj.GetInstanceID());
            if (curCreepTarget != null) 
            {
                creepManager.UnmarkTargetCreepById(curCreepTarget.GetInstanceID());
            } 
            curCreepTarget = targetObj;
        }

        AllManager.Instance().bulletManager.SpawnBullet(transform.position, targetObj.transform.position, 2);
    }

    //public void ShootAtTarget(GameObject target)
    //{
    //    Debug.Log("ShootAtTarget");
    //    AllManager.Instance().bulletManager.SpawnBullet(transform.position, target.transform.position, 2);
    //}
}
