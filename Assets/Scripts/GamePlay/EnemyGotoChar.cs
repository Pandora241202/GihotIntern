using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGotoChar : MonoBehaviour, ITarget
{
    private Renderer rend;
    private Color startColor;

    [SerializeField] private Transform target;
    [SerializeField] private float speed = 1.0f;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    public void Target()
    {
        //rend.material.color = Color.blue;
        //CharacterController.Instance().SetTargetShoot(this.gameObject);
    }

    public void UnTarget()
    {
        rend.material.color = startColor;
    }
}
