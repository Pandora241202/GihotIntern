using UnityEngine;

public class MoveTowardPlayerCreepConfig : CreepConfig
{
    float DistanceBetween(Vector3 pos1, Vector3 pos2)
    {
        return (pos1 - pos2).magnitude;
    }

    protected (int, float) GetNearestPlayerWithDis(Transform creepTransform)
    {
        Player[] players = AllManager._instance.playerManager.Players;

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

        return (playerIdToTarget, minDis);
    }

    public override void Move(Transform creepTransform, float speed) 
    {
        Player[] players = AllManager._instance.playerManager.Players;
        
        (int playerId, float _) = GetNearestPlayerWithDis(creepTransform);

        creepTransform.Translate((players[playerId].playerTrans.position - creepTransform.position).normalized * speed * Time.deltaTime);
    }
}