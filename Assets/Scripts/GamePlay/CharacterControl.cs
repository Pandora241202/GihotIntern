using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    [SerializeField] public float speed = 0.5f;
    public Transform gunTransform;

    [SerializeField] private GameObject prefabBullet;
    public GameObject goChar;
    [SerializeField] public int gunId;
    [SerializeField] LayerMask creepLayerMask;
    //private FixedJoystick joystick;
    private FloatingJoystick joystick;
    GameObject curCreepTarget = null;
    public string id;
    int frame = 0;
    //public Vector3 velocity = new Vector3(0, 0, 0);
    float lastFireTime = 0f;
    public Animator charAnim;
    public Vector3 input_velocity = Vector3.zero;
    Vector3 final_velocity = Vector3.zero;
    bool collision = false;
    Vector3 normal = Vector3.zero;
    public float correctPositionTime = 0;
    public bool isColliding = false;
    public bool lerp = false;
    public int frameLerp = 16;
    public int elapseFrame = 0;
    public Vector3 lerpVertor = Vector3.zero;
    public Vector3 lerpPosition = Vector3.zero;
    //public bool isFire = false;
    //public static CharacterControl _instance { get; private set; }
    //public static CharacterControl Instance()
    //{
    //    if (_instance == null)
    //    {
    //        _instance = GameObject.FindAnyObjectByType<CharacterControl>();
    //    }
    //    return _instance;
    //}

    private void Awake()
    {
        //  SocketCommunication.GetInstance();
        //gunId = GameObject.FindAnyObjectByType<SceneUpdater>().bulletManager.GetGunId();
    }

    private void Start()
    {
        //joystick = UIManager._instance._joystick;
        joystick = UIManager._instance._fjoystick;
    }
    public void SetGunAndBullet()
    {
        GunType gunType = AllManager.Instance().bulletManager.gunConfig.lsGunType[gunId];
        prefabBullet = gunType.bulletPrefab;
        GameObject currentGunPrefab = gunType.gunPrefab;
        GameObject gun = Instantiate(currentGunPrefab, transform.position, Quaternion.identity);
        gun.transform.SetParent(transform.Find("Gun"));
    }

    private void Update()
    {
        //Debug.Log("collide with: " + collision_plane_normal_dict.Count + " obj");
        //characterController.Move(velocity); 
        
        GameObject levelUpEffect = AllManager.Instance().playerManager.dictPlayers[id].levelUpEffect;

        if (levelUpEffect != null)
        {
            levelUpEffect.transform.position = transform.position;

            ParticleSystem ps = levelUpEffect.GetComponent<ParticleSystem>();
            if (ps)
            { 
                if (ps.isStopped)
                {
                    Destroy(levelUpEffect);
                    AllManager.Instance().playerManager.dictPlayers[id].levelUpEffect = null;
                }
            }
        }
    }

    public void Lerp()
    {
        //Vector3 lerpVector = lerpPosition - transform.position;

        //if(lerpVector.magnitude < speed * Time.fixedDeltaTime * 3)
        //{
        //    lerp = false ;
        //    final_velocity = input_velocity;
        //    transform.position = lerpPosition;
        //}

        //if (Vector3.Angle(lerpVector, input_velocity) > 90)
        //{
        //    lerp = false;
        //}

        //Vector3 lerpDirection = lerpVector.normalized;
        //Vector3 direction = (lerpDirection + input_velocity.normalized).normalized;

        //transform.position += direction * Time.fixedDeltaTime * speed;
        //final_velocity = direction * speed;

        //if (Vector3.Angle(velocity, lerpDirection) > 70) return;

        if ((lerpPosition - transform.position).magnitude <= speed * Time.fixedDeltaTime)
        {
            lerp = false;
            transform.position = lerpPosition;
            final_velocity = input_velocity;
            return;
        }

        
       // Vector3 interpolationPosition = Vector3.Lerp(transform.position, lerpPosition, (float)1 / frameLerp);
        //elapseFrame++;
        //transform.position = interpolationPosition;

        lerpPosition += input_velocity * Time.fixedDeltaTime;
        final_velocity = (lerpVertor + input_velocity).normalized * speed;
    }

    private void FixedUpdate()
    {
        if (AllManager.Instance().isPause) return;

        if (lerp)
        {
            Lerp();
        }
        else
        {
            final_velocity = input_velocity;
        }

        normal = Vector3.zero;
            
        if (collision)
        {
            for (int i = 0; i < collision_plane_normal_dict.Count; i++)
            {
                normal += collision_plane_normal_dict.ElementAt(i).Value;
            }

            final_velocity = (normal + final_velocity.normalized).normalized * speed;
        }

        if (correctPositionTime < Time.fixedDeltaTime * 5)
        {
            transform.position += final_velocity * Time.fixedDeltaTime;

        }



        correctPositionTime += Time.fixedDeltaTime;




        if (input_velocity != Vector3.zero)
        {
            charAnim.SetBool("isRun", true);
            goChar.transform.rotation = Quaternion.LookRotation(input_velocity);
        }
        else
        {
            charAnim.SetBool("isRun", false);
        }

        //if (correctingPosition) return;

        //if (frame % 100 == 0) correctingPosition = true;


        if (id != Player_ID.MyPlayerID) return;
        
        if (frame % 3 == 0)
        {
            float horizontal = joystick.Horizontal;
            float vertical = joystick.Vertical;

            Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

            Vector3 v = direction * speed;

            SendData<PlayerState> data = new SendData<PlayerState>(new PlayerState(transform.position, v, 
                Quaternion.LookRotation(direction),AllManager._instance.playerManager.dictPlayers[id],true));
            SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
        }


        frame++;

        //if (frame % 10 == 0)
        //{
        //    SendData<PlayerPosition> position = new SendData<PlayerPosition>(new PlayerPosition(transform.position));
        //    SocketCommunication.GetInstance().Send(JsonUtility.ToJson(position));
        //}
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

        //Debug.Log("find target" + targetObj?.GetInstanceID().ToString());
        return targetObj;
    }

    public void Shoot()
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
        int playerDmg = AllManager.Instance().playerManager.GetPlayerDmg(id);
        lastFireTime = AllManager.Instance().bulletManager
            .SpawnBullet(gunTransform.position, curCreepTarget, gunId, lastFireTime, "PlayerBullet", playerDmg, id);

        //float angle = Vector3.Angle(gunTransform.forward, directionToTarget);
        //if (angle < 10f)
        //{
        //    AllManager.Instance().bulletManager.SpawnBullet(gunTransform.position, curCreepTarget, gunId);
        //}
    }

    Dictionary<int, Vector3> collision_plane_normal_dict = new Dictionary<int, Vector3>();
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Creep"))
        {
            AllManager.Instance().playerManager.ProcessCollisionCreep(id, other.gameObject.GetInstanceID());
        }
        else if (other.gameObject.CompareTag("EnemyBullet"))
        {
            AllManager.Instance().playerManager.ProcessCollisionEnemyBullet(id, other.gameObject.GetInstanceID());
        }
        if (other.gameObject.CompareTag("MapElement"))
        {
            Debug.Log("Collide with map element");
            int id = other.gameObject.GetInstanceID();
            if (collision_plane_normal_dict.ContainsKey(id)) return;
            Vector3 collide_point = other.ClosestPoint(transform.position);
            collide_point.y = transform.position.y;
            collision = true;
            collision_plane_normal_dict.Add(id, (transform.position - collide_point).normalized);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("MapElement"))
        {
            int id = other.gameObject.GetInstanceID();
            if (collision_plane_normal_dict.ContainsKey(id))
            {
                Vector3 collide_point = other.ClosestPoint(transform.position);
                collide_point.y = transform.position.y;
                collision = true;
                collision_plane_normal_dict[id] = (transform.position - collide_point).normalized;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MapElement"))
        {
            if (collision_plane_normal_dict.ContainsKey(other.gameObject.GetInstanceID()))
            {
                collision_plane_normal_dict.Remove(other.gameObject.GetInstanceID());
                if (collision_plane_normal_dict.Count == 0) collision = false;
            }
        }
    }
}