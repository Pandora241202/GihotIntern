using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    public Transform gunTransform;

    [SerializeField] private GameObject prefabBullet;

    [SerializeField] public int gunId; 
    [SerializeField] LayerMask creepLayerMask;
    GameObject curCreepTarget = null;
    public string id;
    int frame = 0;
    public Vector3 velocity = new Vector3(0, 0, 0);
    float lastFireTime = 0f;
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
        //gunId = GameObject.FindAnyObjectByType<SceneUpdater>().bulletManager.GetGunId();
    }
    private void Start(){
        prefabBullet = AllManager.Instance().bulletManager.gunConfig.lsGunType[gunId].bulletPrefab;
    }
    private void Update()
    {
        transform.position += this.velocity;
        Shoot();
        if (id != Player_ID.MyPlayerID) return;
        //float horizontal = _joystick.Horizontal;
        //float vertical = _joystick.Vertical;
        //Vector3 direction = new Vector3(horizontal, 0, vertical);
        //transform.position += direction * speed * Time.deltaTime;
        if (frame % 5 == 0)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 velocity = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;
            SendData<MoveEvent> data = new SendData<MoveEvent>(new MoveEvent(velocity, transform.position));
            SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
        }
        frame++;
    }

    private void FixedUpdate()
    {

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

        Vector3 directionToTarget = (targetObj.transform.position - gunTransform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        gunTransform.rotation = lookRotation;
        lastFireTime = AllManager.Instance().bulletManager.SpawnBullet(gunTransform.position, curCreepTarget, gunId, lastFireTime);

        //float angle = Vector3.Angle(gunTransform.forward, directionToTarget);
        //if (angle < 10f)
        //{
        //    AllManager.Instance().bulletManager.SpawnBullet(gunTransform.position, curCreepTarget, gunId);
        //}
    }

    public void SetTargetShoot(GameObject target)
    {
        Debug.Log("SetTargetShoot");
        //int gunId = AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].gunId;
        // AllManager.Instance().bulletManager.SpawnBullet(gunTransform.position, target.transform.position, gunId);
        AllManager.Instance().bulletManager.SetTarget(target);
    }
}
