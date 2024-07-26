
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

    public override void Activate(GameEvent gameEvent, GameEventData eventData)
    {
        base.Activate(gameEvent, eventData);

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

    public override void End(GameEvent gameEvent, bool endState)
    {
        base.End(gameEvent, endState);
        Destroy(gameEvent.anchorTrans.gameObject);
        foreach (var pair in gameEvent.connectLineTransDict)
        {
            Destroy(pair.Value.gameObject);
        }
        gameEvent.connectLineTransDict.Clear();
    }

    public override void FixedApply(GameEvent gameEvent)
    {
        Dictionary<string, Player> playersDict = AllManager.Instance().playerManager.dictPlayers;

        List<string> playerLeaveIdList = new List<string>();
        foreach (var pair in gameEvent.connectLineTransDict)
        {
            if (!playersDict.ContainsKey(pair.Key))
            {
                playerLeaveIdList.Add(pair.Key);
            }
        }
        foreach (string id in playerLeaveIdList)
        {
            Destroy(gameEvent.connectLineTransDict[id].gameObject.gameObject);
            gameEvent.connectLineTransDict.Remove(id);
        }

        foreach (var pair in playersDict)
        {
            Player player = pair.Value;

            Vector3 disVector = player.playerTrans.position - gameEvent.anchorTrans.position;
            disVector.y = 0;

            if (disVector.magnitude > chainRange)
            {
                player.playerTrans.position = gameEvent.anchorTrans.position + (player.playerTrans.position - gameEvent.anchorTrans.position).normalized * chainRange;
            }

            ConnectPlayerAnchor(gameEvent.anchorTrans, disVector, gameEvent.connectLineTransDict[pair.Key]);
        }

        Vector3 sumDis = Vector3.zero;

        foreach (var pair in playersDict)
        {
            Player player = pair.Value;

            Vector3 newPos = player.playerTrans.position + player.final_velocity * Time.fixedDeltaTime;
            Vector3 disVector = newPos - gameEvent.anchorTrans.position;
            disVector.y = 0;

            if (disVector.magnitude > chainRange)
            {
                sumDis += disVector - disVector.normalized * chainRange;
            }
        }

        gameEvent.anchorTrans.position += sumDis;
    }

    private void ConnectPlayerAnchor(Transform anchorTrans, Vector3 anchorToPlayer, Transform lineTrans)
    {
        lineTrans.position = anchorTrans.position + anchorToPlayer / 2;
        lineTrans.localRotation = Quaternion.LookRotation(anchorToPlayer, new Vector3(1, 0, 0)) * Quaternion.Euler(90, 0, 0);
        lineTrans.localScale = new Vector3(0.1f, anchorToPlayer.magnitude / 2, 0.1f);
    }
}
