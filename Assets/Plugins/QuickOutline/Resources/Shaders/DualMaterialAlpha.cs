using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class DualMaterialAlpha : MonoBehaviour
{
    [Tooltip("Index of the material in the MeshRenderer's materials array to hide (zero-based)")]
    public int materialIndex = 0;

    private MeshRenderer _renderer;
    private Material[] _originalMaterials;
    private Material _backupMaterial;

    void Awake()
    {
        // Cache renderer and original materials
        _renderer = GetComponent<MeshRenderer>();
        _originalMaterials = _renderer.materials;

        // Ensure index is valid before storing
        if (materialIndex >= 0 && materialIndex < _originalMaterials.Length)
        {
            _backupMaterial = _originalMaterials[materialIndex];
        }
        else
        {
            Debug.LogWarning($"Material index {materialIndex} is out of range for '{name}'");
        }

        Hide();
    }

    /// <summary>
    /// Hides the specified material by converting it to transparent with zero alpha.
    /// </summary>
    public void Hide()
    {
        if (_backupMaterial == null) return;

        // Clone current materials array
        var mats = _renderer.materials;

        // Clone the target material to avoid modifying shared asset
        var mat = Instantiate(mats[materialIndex]);
        SetMaterialTransparent(mat);

        mats[materialIndex] = mat;
        _renderer.materials = mats;
    }

    /// <summary>
    /// Restores the original material.
    /// </summary>
    public void Restore()
    {
        if (_backupMaterial == null) return;

        var mats = _renderer.materials;
        mats[materialIndex] = _backupMaterial;
        _renderer.materials = mats;
    }

    /// <summary>
    /// Configures a material to render fully transparent, with alpha = 0.
    /// </summary>
    /// <param name="mat">Material to modify</param>
    private void SetMaterialTransparent(Material mat)
    {
        // Standard shader transparency setup
        mat.SetFloat("_Mode", 3);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

        // Set alpha to zero
        Color c = mat.color;
        c.a = 0f;
        mat.color = c;
    }
}
