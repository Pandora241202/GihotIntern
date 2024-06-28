using UnityEngine;

public class CreepController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            AllManager allManager = AllManager.Instance();
            allManager.creepManager.ProcessCollision(gameObject.GetInstanceID(), other.gameObject);
            //allManager.bulletManager.ProcessCollision(other.gameObject.GetInstanceID());
            //GameObject.Destroy(other.gameObject);
        }
    }
}