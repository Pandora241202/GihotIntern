using UnityEngine;

public class MoveFarPlayerCreepConfig : CreepConfig
{
    [SerializeField] float startMoveAwayDistance;

    float DistanceBetween(Vector3 pos1, Vector3 pos2)
    {
        return (pos1 - pos2).magnitude;
    }

    public override void Move(Transform creepTransform, float speed) 
    {
        Player[] players = AllManager._instance.playerManager.lsPlayers.ToArray();
        
        float minDis = DistanceBetween(players[0].playerTrans.position, creepTransform.position);
        int playerIdToTarget = 0;

        for (int i = 1; i < players.Length; i++)
        {
            float dis = DistanceBetween(players[0].playerTrans.position, creepTransform.position);
            if (dis < minDis)
            {
                playerIdToTarget = i;
                minDis = dis;
            }
        }

        if (minDis <= startMoveAwayDistance)
        {
            creepTransform.Translate((creepTransform.position - players[playerIdToTarget].playerTrans.position).normalized * speed * Time.deltaTime);
        }
    }
}