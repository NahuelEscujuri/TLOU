using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ListenModeTarget: MonoBehaviour
{
    private static HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();
    [SerializeField, HideInInspector]
    private List<Mesh> bakeKeys = new List<Mesh>();

    [SerializeField, HideInInspector]
    private List<ListVector3> bakeValues = new List<ListVector3>();
    Material listenModeMaterial;
    Renderer[] renderers;
    float trancitionDuration = 1;



    [Serializable]
    private class ListVector3
    {
        public List<Vector3> data;
    }

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        listenModeMaterial = Instantiate(Resources.Load<Material>(@"Materials/ListenMode"));
        listenModeMaterial.SetFloat("_Alpha", 0);
        // defaultMaterial = skinnedMeshRenderer.material;
        // Retrieve or generate smooth normals
        LoadSmoothNormals();
    }

    private void OnEnable()
    {
        foreach (var renderer in renderers)
        {

            // Append outline shaders
            var materials = renderer.sharedMaterials.ToList();

            materials.Add(listenModeMaterial);

            renderer.materials = materials.ToArray();
        }
    }

    public void Show()
    {
        StartChangeAlpha(listenModeMaterial, 1, trancitionDuration);
    }
    public void Hide()
    {
        StartChangeAlpha(listenModeMaterial, 0, trancitionDuration);
    }

    void LoadSmoothNormals()
    {

        // Retrieve or generate smooth normals
        foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
        {

            // Skip if smooth normals have already been adopted
            if (!registeredMeshes.Add(meshFilter.sharedMesh))
            {
                continue;
            }

            // Retrieve or generate smooth normals
            var index = bakeKeys.IndexOf(meshFilter.sharedMesh);
            var smoothNormals = (index >= 0) ? bakeValues[index].data : SmoothNormals(meshFilter.sharedMesh);

            // Store smooth normals in UV3
            meshFilter.sharedMesh.SetUVs(3, smoothNormals);

            // Combine submeshes
            var renderer = meshFilter.GetComponent<Renderer>();

            if (renderer != null)
            {
                CombineSubmeshes(meshFilter.sharedMesh, renderer.sharedMaterials);
            }
        }

        // Clear UV3 on skinned mesh renderers
        foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {

            // Skip if UV3 has already been reset
            if (!registeredMeshes.Add(skinnedMeshRenderer.sharedMesh))
            {
                continue;
            }

            // Clear UV3
            skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];

            // Combine submeshes
            CombineSubmeshes(skinnedMeshRenderer.sharedMesh, skinnedMeshRenderer.sharedMaterials);
        }
    }

    void CombineSubmeshes(Mesh mesh, Material[] materials)
    {

        // Skip meshes with a single submesh
        if (mesh.subMeshCount == 1)
        {
            return;
        }

        // Skip if submesh count exceeds material count
        if (mesh.subMeshCount > materials.Length)
        {
            return;
        }

        // Append combined submesh
        mesh.subMeshCount++;
        mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
    }
    #region Change Alpha

    // Metodo que usa corrutina para interpola suavemente el valor _Alpha del material.
    void StartChangeAlpha(Material mat, float targetAlpha, float duration)
    {
        StopCoroutine("ChangeAlpha");
        StartCoroutine(ChangeAlpha(mat, targetAlpha, duration));
    }

    // Corrutina que interpola suavemente el valor _Alpha del material.
    private IEnumerator ChangeAlpha(Material mat, float targetAlpha, float duration)
    {
        // Obtenemos el alpha inicial
        float startAlpha = mat.GetFloat("_Alpha");
        float elapsed = 0f;

        // Mientras no lleguemos al tiempo total...
        while (elapsed < duration)
        {
            // Avanzamos el tiempo transcurrido
            elapsed += Time.deltaTime;
            // Calculamos cuánto de la transición hemos completado (0..1)
            float t = Mathf.Clamp01(elapsed / duration);
            // Interpolamos el valor de alpha
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            // Lo asignamos al material
            mat.SetFloat("_Alpha", currentAlpha);
            // Esperamos al siguiente frame
            yield return null;
        }

        // Aseguramos que quede exactamente en el valor final
        mat.SetFloat("_Alpha", targetAlpha);
    }
    #endregion

    List<Vector3> SmoothNormals(Mesh mesh)
    {

        // Group vertices by location
        var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);

        // Copy normals to a new list
        var smoothNormals = new List<Vector3>(mesh.normals);

        // Average normals for grouped vertices
        foreach (var group in groups)
        {

            // Skip single vertices
            if (group.Count() == 1)
            {
                continue;
            }

            // Calculate the average normal
            var smoothNormal = Vector3.zero;

            foreach (var pair in group)
            {
                smoothNormal += smoothNormals[pair.Value];
            }

            smoothNormal.Normalize();

            // Assign smooth normal to each vertex
            foreach (var pair in group)
            {
                smoothNormals[pair.Value] = smoothNormal;
            }
        }

        return smoothNormals;
    }
}