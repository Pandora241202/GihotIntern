using UnityEngine;

[CreateAssetMenu(fileName = "Creep4Config", menuName = "Config/CreepConfig/MoveTowardPlayer/Creep4")]
public class Creep4Config : MoveTowardPlayerCreepConfig
{
    [SerializeField] float startIncreaseSpeedDis;
    [SerializeField] float speedMultiplayer;

    public override void Attack(Creep creep)
    {
        //(string playerId, float dis) = GetNearestPlayerWithDis(creep.creepTrans);
        Debug.Log(CharacterController.Instance());
        float dis = (CharacterController.Instance().transform.position - creep.creepTrans.position).magnitude;

        if (dis <= startIncreaseSpeedDis)
        {
            creep.speed += creep.speed*speedMultiplayer*Time.deltaTime;
        }
    }
}