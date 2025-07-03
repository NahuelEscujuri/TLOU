using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class OutlineController : MonoBehaviour
{
    public Color outlineColor = Color.white;
    [Range(0, 10)] public float outlineWidth = 2f;
    [Min(0)] public float blurSize = 1f;
    [Range(0, 10)] public int blurSamples = 4;

    private Material mat;
    private static readonly int ZTestID = Shader.PropertyToID("_ZTest");
    private static readonly int ColorID = Shader.PropertyToID("_OutlineColor");
    private static readonly int WidthID = Shader.PropertyToID("_OutlineWidth");
    private static readonly int BlurSizeID = Shader.PropertyToID("_BlurSize");
    private static readonly int BlurSamplesID = Shader.PropertyToID("_BlurSamples");

    void Awake()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        mat.SetColor(ColorID, outlineColor);
        mat.SetFloat(WidthID, outlineWidth);
        mat.SetFloat(BlurSizeID, blurSize);
        mat.SetInt(BlurSamplesID, blurSamples);
        // Si necesitas cambiar el ZTest:
        // mat.SetFloat(ZTestID, (float)UnityEngine.Rendering.CompareFunction.LessEqual);
    }
}

