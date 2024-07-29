using System.Collections.Generic;
using UnityEngine;

public class RenderManager
{
    private Dictionary<int, GameObject> treeHidePlayerDict = new Dictionary<int, GameObject>();
    private Camera camera;
    private LayerMask treeLayerMask;

    public RenderManager(Camera camera, LayerMask treeLayerMask)
    {
        this.camera = camera;
        this.treeLayerMask = treeLayerMask;
    }

    public void MyUpdate()
    {
        HandleCharacterBehindObjects();
    }

    public void HandleCharacterBehindObjects()
    {
        RaycastHit[] hits = GetAllHitsFromCamToPlayer();
        Dictionary<int, GameObject> curTreeHidePlayerDict = new Dictionary<int, GameObject>();

        foreach (RaycastHit hit in hits)
        {
            int objId = hit.collider.gameObject.GetInstanceID();

            if (treeHidePlayerDict.ContainsKey(objId))
            {
                treeHidePlayerDict.Remove(objId);
            } else
            {
                SetTransparentObj(hit.collider.gameObject);
            }

            curTreeHidePlayerDict.Add(objId, hit.collider.gameObject);
        }

        foreach (GameObject obj in treeHidePlayerDict.Values)
        {
            SetOpaqueObj(obj);
        }

        treeHidePlayerDict = curTreeHidePlayerDict;
    }

    public RaycastHit[] GetAllHitsFromCamToPlayer()
    {
        if (AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].playerTrans == null)
        {
            return new RaycastHit[0];
        }

        Vector3 camToPlayer = AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID].playerTrans.position - camera.transform.position;
        return Physics.RaycastAll(camera.transform.position, camToPlayer, camToPlayer.magnitude, treeLayerMask);
    }

    public void SetTransparentObj(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();

        Material[] mats = renderer.sharedMaterials;

        for (int i = 0; i < mats.Length; i++)
        {
            string matName = mats[i].name;

            string transparentMatName = matName.Insert(2, "T");
            Material transparentMat = Resources.Load<Material>("Materials/" + transparentMatName);

            if (transparentMat == null)
            {
                continue;
            }

            mats[i] = transparentMat;
        }

        renderer.sharedMaterials = mats;
    }

    public void SetOpaqueObj(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();

        Material[] mats = renderer.sharedMaterials;

        for (int i = 0; i < mats.Length; i++)
        {
            string matName = mats[i].name;

            string opaqueMatName = matName.Remove(2, 1);
            Material opaqueMat = Resources.Load<Material>("Materials/" + opaqueMatName);

            if (opaqueMat == null)
            {
                continue;
            }

            mats[i] = opaqueMat;
        }

        renderer.sharedMaterials = mats;
    }
}
