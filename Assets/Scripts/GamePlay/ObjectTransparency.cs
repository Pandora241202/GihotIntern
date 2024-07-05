using UnityEngine;

public class ObjectTransparency : MonoBehaviour
{
    public Transform character; 
    public LayerMask obstacleMask;

    private Renderer[] previousHitRenderers; 

    void Update()
    {
        MakeObstaclesTransparent();
    }

    void MakeObstaclesTransparent()
    {
 
        Vector3 direction = character.position - transform.position;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit[] hits = Physics.RaycastAll(ray, direction.magnitude, obstacleMask);
        
        if (previousHitRenderers != null)
        {
            foreach (Renderer rend in previousHitRenderers)
            {
                SetTransparency(rend, 1.0f); 
            }
        }


        if (hits.Length > 0)
        {
            previousHitRenderers = new Renderer[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                Renderer rend = hits[i].transform.GetComponent<Renderer>();
                if (rend != null)
                {
                    SetTransparency(rend, 0.3f); 
                    previousHitRenderers[i] = rend;
                }
            }
        }
    }

    void SetTransparency(Renderer rend, float alpha)
    {
        if (rend != null)
        {
            foreach (Material mat in rend.materials)
            {
                Color color = mat.color;
                color.a = alpha;
                mat.color = color;

                if (alpha < 1.0f)
                {
                    mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    mat.SetInt("_ZWrite", 0);
                    mat.DisableKeyword("_ALPHATEST_ON");
                    mat.EnableKeyword("_ALPHABLEND_ON");
                    mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                }
                else
                {

                    mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    mat.SetInt("_ZWrite", 1);
                    mat.DisableKeyword("_ALPHATEST_ON");
                    mat.DisableKeyword("_ALPHABLEND_ON");
                    mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    mat.renderQueue = -1;
                }
            }
        }
    }
}
