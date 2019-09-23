using UnityEngine;
using System.Collections;

public class EdgeDetectionDepth : MonoBehaviour
{

    public Shader edgeDetectShader;
    public Material edgeDetectMaterial = null;

    [Range(0,1)]
    public float _SampleDis = 1f;
    [Range(0, 1)]
    public float _SensitiveNormal=1f;
    [Range(0, 1)]
    public float _SensitiveDepth=1f;
    [Range(0, 1)]
    public float EpsNormal=0.1f;
    [Range(0, 1)]
    public float _EpsDepth=0.1f;

    private void Start()
    {
        Camera.main.depthTextureMode = DepthTextureMode.DepthNormals;
    }

    public Color _OutlineColor = Color.black;

 

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
 

            edgeDetectMaterial.SetFloat("_SampleDis", _SampleDis);
            edgeDetectMaterial.SetFloat("_SensitiveNormal", _SensitiveNormal);
            edgeDetectMaterial.SetFloat("_SensitiveDepth", _SensitiveDepth);
            edgeDetectMaterial.SetFloat("EpsNormal", EpsNormal);
            edgeDetectMaterial.SetFloat("_EpsDepth", _EpsDepth);

            edgeDetectMaterial.SetColor("_OutlineColor", _OutlineColor);
            Graphics.Blit(src, dest, edgeDetectMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}