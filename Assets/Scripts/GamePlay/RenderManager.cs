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

        Material[] mats = renderer.materials;

        foreach (Material mat in mats)
        {
            mat.SetFloat("_Mode", 3);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;

            Color color = mat.GetColor("_Color");
            color.a = 0.2f;
            mat.SetColor("_Color", color);

            mat.DisableKeyword("_EMISSION");
        }
    }

    public void SetOpaqueObj(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();

        Material[] mats = renderer.materials;

        foreach (Material mat in mats)
        {
            mat.SetFloat("_Mode", 0);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            mat.SetInt("_ZWrite", 1);
            mat.EnableKeyword("_ALPHATEST_ON");
            mat.DisableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 2000;

            Color color = mat.GetColor("_Color");
            color.a = 1f;
            mat.SetColor("_Color", color);

            mat.EnableKeyword("_EMISSION");
        }
    }
}
