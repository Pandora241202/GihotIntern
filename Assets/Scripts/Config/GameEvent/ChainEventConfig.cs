using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChainEventConfig", menuName = "Config/GameEventConfig/ChainEvent")]
public class ChainEventConfig : GameEventConfig
{
    [SerializeField] Vector3 defaultAnchorPos;
    [SerializeField] GameObject anchorPrefab;
    [SerializeField] GameObject connectLinePrefab;
    [SerializeField] float speed;
    [SerializeField] float chainRange;

    public override void Activate(GameEvent gameEvent)
    {
        gameEvent.anchorTrans = GameObject.Instantiate(anchorPrefab, defaultAnchorPos, Quaternion.identity).transform;
        gameEvent.speed = speed;

        Dictionary<string, Player> playersDict = AllManager.Instance().playerManager.dictPlayers;
        
        foreach (var pair in playersDict)
        {
            Player player = pair.Value;
            
            Vector3 disVector = gameEvent.anchorTrans.position - player.playerTrans.position;
            disVector.y = 0.05f;

            Transform lineTrans = GameObject.Instantiate(connectLinePrefab, (gameEvent.anchorTrans.position + player.playerTrans.position) / 2, Quaternion.identity).transform;
            lineTrans.localRotation = Quaternion.LookRotation(new Vector3(disVector.x, 0, disVector.z), new Vector3(1, 0, 0)) * Quaternion.Euler(90, 0, 0);
            lineTrans.localScale = new Vector3(0.1f, disVector.magnitude / 2, 0.1f);

            gameEvent.connectLineTransDict.Add(pair.Key, lineTrans);

            player.playerTrans.position = defaultAnchorPos;
        }
    }

    private bool IsStuck(List<Vector3> vectors, Vector3 sum, int i)
    {
        if (i == -1)
        {
            return false;
        }

        if (i == 0)
        {
            Vector3 v = sum - vectors[0].normalized;
            return -0.1 <= v.magnitude && v.magnitude <= 0.1;
        }

        return IsStuck(vectors, sum, i - 1) || IsStuck(vectors, (sum - vectors[i].normalized).normalized, i - 1);
    }

    public override void Apply(GameEvent gameEvent) 
    {
        Dictionary<string, Player> playersDict = AllManager.Instance().playerManager.dictPlayers;

        List<Vector3> disVectorOutOfRangeList = new List<Vector3>();

        foreach (var pair in playersDict)
        {
            Player player = pair.Value;

            Vector3 disVector = player.playerTrans.position - gameEvent.anchorTrans.position;
            disVector.y = 0;
            
            if (disVector.magnitude - chainRange >= -0.001)
            {
                disVectorOutOfRangeList.Add(disVector);
            }

            ConnectPlayerAnchor(gameEvent.anchorTrans, disVector, gameEvent.connectLineTransDict[pair.Key]);
        }

        if (IsStuck(disVectorOutOfRangeList, Vector3.zero, disVectorOutOfRangeList.Count - 1))
        {
            foreach (var pair in playersDict)
            {
                Player player = pair.Value;

                Vector3 newPos = player.playerTrans.position + player.playerTrans.gameObject.GetComponent<CharacterControl>().final_velocity * Time.fixedDeltaTime;
                Vector3 disVector = newPos - gameEvent.anchorTrans.position;
                disVector.y = 0;
                
                if (disVector.magnitude - chainRange >= -0.001)
                {
                    player.playerTrans.gameObject.GetComponent<CharacterControl>().input_velocity = Vector3.zero;
                    player.playerTrans.gameObject.GetComponent<CharacterControl>().lerpVertor = Vector3.zero;
                }
            }
        }
        else
        {
            Vector3 sumDis = Vector3.zero;

            foreach (var pair in playersDict)
            {
                Player player = pair.Value;

                Vector3 newPos = player.playerTrans.position + player.playerTrans.gameObject.GetComponent<CharacterControl>().final_velocity * Time.fixedDeltaTime;
                Vector3 disVector = newPos - gameEvent.anchorTrans.position;
                disVector.y = 0;

                if (disVector.magnitude - chainRange > -0.001)
                {
                    sumDis += disVector - disVector.normalized * chainRange;
                }
            }

            gameEvent.anchorTrans.position += sumDis;
        }
    }

    private void ConnectPlayerAnchor(Transform anchorTrans, Vector3 anchorToPlayer, Transform lineTrans)
    {
        lineTrans.position = anchorTrans.position + anchorToPlayer / 2;
        lineTrans.localRotation = Quaternion.LookRotation(anchorToPlayer, new Vector3(1, 0, 0)) * Quaternion.Euler(90, 0, 0);
        lineTrans.localScale = new Vector3(0.1f, anchorToPlayer.magnitude / 2, 0.1f);
    }
}
