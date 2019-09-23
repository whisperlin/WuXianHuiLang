using UnityEngine;
using System.Collections;

public class EdgeDetection : MonoBehaviour
{

    public Shader edgeDetectShader;
    public Material edgeDetectMaterial = null;
    

    [Range(0.0f, 1.0f)]
    public float edgesOnly = 0.0f;

    public Color edgeColor = Color.black;

    public Color backgroundColor = Color.white;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (null == edgeDetectMaterial)
        {
            if (null != edgeDetectShader)
            {
                edgeDetectMaterial = new Material(edgeDetectShader);
            }
        }
        if (edgeDetectMaterial != null)
        {
            edgeDetectMaterial.SetFloat("_EdgeOnly", edgesOnly);
            edgeDetectMaterial.SetColor("_EdgeColor", edgeColor);
            edgeDetectMaterial.SetColor("_BackgroundColor", backgroundColor);
            Graphics.Blit(src, dest, edgeDetectMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}