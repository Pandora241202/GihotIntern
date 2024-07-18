using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] GameObject[] players;
    [SerializeField] float chainRange;
    [SerializeField] float speed;
    [SerializeField] GameObject line;
    [SerializeField] List<GameObject> lines = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < players.Length; i++)
        {
            Vector3 disVector = transform.position - players[i].transform.position;
            disVector.y = 0.05f;
            lines.Add(GameObject.Instantiate(line, (transform.position + players[i].transform.position) / 2, Quaternion.identity));
            lines[i].transform.localRotation = Quaternion.LookRotation(new Vector3(disVector.x, 0, disVector.z), new Vector3(1, 0, 0)) * Quaternion.Euler(90, 0, 0);
            lines[i].transform.localScale = new Vector3(0.1f, disVector.magnitude/2, 0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    { 
        List<Vector3> distanceVectorList = new List<Vector3>();
        List<GameObject> outOfRangePlayesrs = new List<GameObject>();

        /*for (int i = 0; i < players.Length; i++)
        {
            Vector3 newPos = players[i].transform.position + players[i].transform.forward * speed * Time.deltaTime;
            Vector3 distanceVector = newPos - transform.position;
            distanceVector.y = 0;
            if (distanceVector.magnitude >= chainRange)
            {
                distanceVectorList.Add(distanceVector);
                outOfRangePlayesrs.Add(players[i]);
            }
            else
            {
                players[i].transform.position = newPos;
                ConnectPlayerAnchor(distanceVector, i);
            }
        }

        if (distanceVectorList.Count == 0)
        {
            return;
        }

        if (CheckIfAnySumIsZero(distanceVectorList, Vector3.zero, distanceVectorList.Count - 1))
        {
            for (int i = 0; i < outOfRangePlayesrs.Count; i++)
            {
                outOfRangePlayesrs[i].transform.position = transform.position + distanceVectorList[i].normalized*chainRange;
                ConnectPlayerAnchor(distanceVectorList[i].normalized * chainRange, i);
            }
        }
        else
        {
            Vector3 sumDis = Vector3.zero;
            foreach (Vector3 distanceVector in distanceVectorList)
            {
                sumDis += distanceVector;
            }

            transform.position += sumDis.normalized * speed * Time.deltaTime;

            for (int i = 0; i < outOfRangePlayesrs.Count; i++)
            {
                Vector3 newPos = players[i].transform.position + players[i].transform.forward * speed * Time.deltaTime;
                Vector3 distanceVector = newPos - transform.position;
                
                if (distanceVector.magnitude > chainRange)
                {
                    players[i].transform.position = transform.position + distanceVector.normalized * chainRange;
                    distanceVector = distanceVector.normalized * chainRange;
                } else
                {
                    players[i].transform.position += players[i].transform.forward * speed * Time.deltaTime;
                }

                ConnectPlayerAnchor(distanceVector, i);
            }
        }*/

        for (int i = 0; i < players.Length; i++)
        {
            Vector3 distanceVector = players[i].transform.position - transform.position;
            distanceVector.y = 0;
            if (distanceVector.magnitude - chainRange >= -0.001)
            {
                distanceVectorList.Add(distanceVector);
            }
        }

        if (CheckIfAnySumIsZero(distanceVectorList, Vector3.zero, distanceVectorList.Count-1))
        {
            for (int i = 0; i < players.Length; i++)
            {
                Vector3 newPos = players[i].transform.position + players[i].transform.forward * speed * Time.deltaTime;
                Vector3 distanceVector = newPos - transform.position;
                distanceVector.y = 0;
                if (distanceVector.magnitude - chainRange <= 0.001)
                {
                    players[i].transform.position = newPos;
                    ConnectPlayerAnchor(distanceVector, i);
                }
                else
                {
                    //players[i].transform.position = transform.position + distanceVector.normalized*chainRange;
                    //players[i].transform.position = new Vector3(players[i].transform.position.x, 1, players[i].transform.position.z);
                    //ConnectPlayerAnchor(distanceVector.normalized * chainRange, i);
                }
            }
        }
        else
        {
            Debug.Log("dcm");

            Vector3 sumDis = Vector3.zero;

            for (int i = 0; i < players.Length; i++)
            {
                Vector3 newPos = players[i].transform.position + players[i].transform.forward * speed * Time.deltaTime;
                Vector3 distanceVector = newPos - transform.position;
                distanceVector.y = 0;
                if (distanceVector.magnitude > chainRange)
                {
                    sumDis += distanceVector-distanceVector.normalized*chainRange;
                }
                players[i].transform.position = newPos;
                ConnectPlayerAnchor(distanceVector, i);
            }

            transform.position += sumDis;
        }
    }

    bool CheckIfAnySumIsZero(List<Vector3> vectors, Vector3 sum, int i)
    {
        if (i == -1) 
        {
            return false; 
        }

        if (i == 0)
        {
            Vector3 v = sum - vectors[0].normalized;
            Debug.Log(sum);
            Debug.Log(v);
            return -0.01 <= v.magnitude && v.magnitude <= 0.01;
        }

        return CheckIfAnySumIsZero(vectors, sum, i - 1) || CheckIfAnySumIsZero(vectors, (sum - vectors[i].normalized).normalized, i - 1);
    }

    private void ConnectPlayerAnchor(Vector3 anchorToPlayer, int i)
    {
        lines[i].transform.position = transform.position + anchorToPlayer / 2;
        lines[i].transform.localRotation = Quaternion.LookRotation(anchorToPlayer, new Vector3(1, 0, 0)) * Quaternion.Euler(90, 0, 0);
        lines[i].transform.localScale = new Vector3(0.1f, anchorToPlayer.magnitude / 2, 0.1f);
    }
}
