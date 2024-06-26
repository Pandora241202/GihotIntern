using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private CharacterController controller;
    [SerializeField] private GameObject prefabBullet;
    
    public static CharacterController _instance { get; private set; }
     public static CharacterController Instance()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindAnyObjectByType<CharacterController>();
        }
        return _instance;
    }

    private void Awake() {
      //  SocketCommunication.GetInstance();
    }
    private void Update() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical);
        transform.position += direction * speed * Time.deltaTime;
    }
    public void ShootAtTarget(GameObject target)
    {
        Debug.Log("ShootAtTarget");
        AllManager.Instance().bulletManager.SpawnBullet(transform.position,target.transform.position,1);
    }
}
