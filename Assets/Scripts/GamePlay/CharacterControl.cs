using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public Transform gunTransform;
    public TextMeshProUGUI txtName;
    public GameObject goChar;
    //[SerializeField] public int gunId;
    [SerializeField] LayerMask creepLayerMask;
    public FloatingJoystick joystick;
    public string id;
    public Animator charAnim;

    private bool isInvincible = false;
    public GameObject goCircleRes;
    public float timeRevive;

    public bool isRevive = false;
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
        isRevive = false;
        timeRevive = 1f;
    }

    private void Update()
    {
        //Debug.Log("collide with: " + collision_plane_normal_dict.Count + " obj");
        //characterController.Move(velocity); 
        if (AllManager.Instance().isPause) return;

        if (isRevive)
        {
            timeRevive -= Time.deltaTime;
            if (timeRevive <= 0)
            {
                Debug.Log("Revived");
            }
        }

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

    public void EnableInvincibility(float duration)
    {
        StartCoroutine(InvincibilityRoutine(duration));
        Debug.Log("Cant touch me");
    }

    private IEnumerator InvincibilityRoutine(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }

    Dictionary<int, Vector3> collision_plane_normal_dict = new Dictionary<int, Vector3>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Respawn"))
        {
            //timeRevive = 1f;
            string player_id = other.gameObject.GetComponentInParent<CharacterControl>().id;
            SendData<ReviveEvent> data =  new SendData<ReviveEvent>(new ReviveEvent(player_id));
            SocketCommunication.GetInstance().Send(JsonUtility.ToJson(data));
        }
        else if (other.gameObject.CompareTag("Creep"))
        {
            if (isInvincible)
            {
                return;
            }
            AllManager.Instance().playerManager.ProcessCollisionCreep(id, other.gameObject.GetInstanceID());
            EnableInvincibility(1f);
        }
        else if (other.gameObject.CompareTag("EnemyBullet"))
        {
            if (isInvincible)
            {
                return;
            }
            AllManager.Instance().playerManager.ProcessCollisionEnemyBullet(id, other.gameObject.GetInstanceID());
            EnableInvincibility(1f);
        }
        else if (other.gameObject.CompareTag("MapElement"))
        {
            Debug.Log("Collide with map element");
            int elementId = other.gameObject.GetInstanceID();
            Player player = AllManager.Instance().playerManager.dictPlayers[id];
            if (player.collision_plane_normal_dict.ContainsKey(elementId)) return;
            Vector3 collide_point = other.ClosestPoint(transform.position);
            collide_point.y = transform.position.y;
            player.collision = true;
            player.collision_plane_normal_dict.Add(elementId, (transform.position - collide_point).normalized);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.gameObject.CompareTag("Respawn"))
        //{
        //    isRevive = true;
        //}
        if (other.gameObject.CompareTag("MapElement"))
        {
            int elementId = other.gameObject.GetInstanceID();
            Player player = AllManager.Instance().playerManager.dictPlayers[id];
            if (player.collision_plane_normal_dict.ContainsKey(elementId))
            {
                Vector3 collide_point = other.ClosestPoint(transform.position);
                collide_point.y = transform.position.y;
                player.collision = true;
                player.collision_plane_normal_dict[elementId] = (transform.position - collide_point).normalized;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.CompareTag("Respawn"))
        //{
        //    isRevive = false;
        //}
        if (other.gameObject.CompareTag("MapElement"))
        {
            Player player = AllManager.Instance().playerManager.dictPlayers[id];
            if (player.collision_plane_normal_dict.ContainsKey(other.gameObject.GetInstanceID()))
            {
                player.collision_plane_normal_dict.Remove(other.gameObject.GetInstanceID());
                if (player.collision_plane_normal_dict.Count == 0) player.collision = false;
            }
        }
    }
}