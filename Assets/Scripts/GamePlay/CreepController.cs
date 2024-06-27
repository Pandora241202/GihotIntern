using UnityEngine;

public class CreepController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("playerBullet"))
        {
            AllManager allManager = AllManager._instance;
            allManager.creepManager.ProcessCollision(gameObject.GetInstanceID(), other.gameObject);
            //allManager.bulletManager.ProcessCollision(other.gameObject.GetInstanceID());
        }
    }
}