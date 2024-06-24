using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShootNearest : MonoBehaviour
{
    public float searchRadius = 10.0f;
    public int maxColliders = 10;
    private ITarget currentTarget;

    private void Update()
    {
        FindInRadius();
    }

    void FindInRadius()
    {
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, searchRadius, hitColliders);
        float closestDistance = Mathf.Infinity;
        ITarget closestTarget = null;

        for (int i = 0; i < numColliders; i++)
        {
            if (hitColliders[i].TryGetComponent<ITarget>(out ITarget target))
            {
                float distance = Vector3.Distance(transform.position, hitColliders[i].transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }
        }

        if (currentTarget != closestTarget)
        {
            if (currentTarget != null)
            {
                currentTarget.UnTarget();
            }
            currentTarget = closestTarget;
            if (currentTarget != null)
            {
                currentTarget.Target();
                Debug.Log("Targeting: " + currentTarget);
            }
        }
    }
}