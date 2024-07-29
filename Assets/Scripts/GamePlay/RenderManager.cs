using System.Collections.Generic;
using UnityEditor;
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
            string path = AssetDatabase.GetAssetPath(mats[i]);
            string matFileName = System.IO.Path.GetFileNameWithoutExtension(path);

            string transparentMatFileName = matFileName.Insert(2, "T");
            Material transparentMat = Resources.Load<Material>("Materials/" + transparentMatFileName);

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
            string path = AssetDatabase.GetAssetPath(mats[i]);
            string matFileName = System.IO.Path.GetFileNameWithoutExtension(path);

            string opaqueMatFileName = matFileName.Remove(2, 1);
            Material opaqueMat = Resources.Load<Material>("Materials/" + opaqueMatFileName);

            if (opaqueMat == null)
            {
                continue;
            }

            mats[i] = opaqueMat;
        }

        renderer.sharedMaterials = mats;
    }
}
