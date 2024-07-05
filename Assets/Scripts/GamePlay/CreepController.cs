using UnityEngine;

public class CreepController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            AllManager allManager = AllManager.Instance();
            allManager.creepManager.ProcessCollisionPlayerBullet(gameObject.GetInstanceID(), other.gameObject);
            //allManager.bulletManager.ProcessCollision(other.gameObject.GetInstanceID());
            //GameObject.Destroy(other.gameObject);
        } 
        else if (other.gameObject.CompareTag("MapElement"))
        {
            AllManager.Instance().creepManager.ProcessCollisionMapElement(gameObject.GetInstanceID(), other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MapElement"))
        {
            Creep creep = AllManager.Instance().creepManager.GetActiveCreepById(gameObject.GetInstanceID());

            if (creep.collision_plane_normal_dict.ContainsKey(other.gameObject.GetInstanceID()))
            {
                creep.collision_plane_normal_dict.Remove(other.gameObject.GetInstanceID());
            }
        }
    }
}