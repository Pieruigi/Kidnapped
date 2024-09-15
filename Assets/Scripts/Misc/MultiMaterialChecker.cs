using UnityEngine;

public class MultiMaterialChecker : MonoBehaviour
{
    // Cache dei materiali delle sub-meshes
    private Material[] cachedMaterials;

    void Awake()
    {
        // Ottieni il MeshRenderer e il MeshFilter dell'oggetto
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        if (meshRenderer != null && meshFilter != null && meshFilter.sharedMesh != null)
        {
            // Inizializza l'array dei materiali
            cachedMaterials = meshRenderer.sharedMaterials;
        }
        else
        {
            Debug.LogError("MeshRenderer o MeshFilter non trovato, o la mesh è nulla.");
            cachedMaterials = new Material[0];
        }
    }

    // Funzione per ottenere il materiale basato sull'indice del triangolo
    public Material GetMaterialByTriangleIndex(int triangleIndex)
    {
        if (cachedMaterials.Length == 0)
        {
            Debug.LogWarning("Material cache is empty.");
            return null;
        }

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogError("MeshFilter o Mesh non trovato.");
            return null;
        }

        Mesh mesh = meshFilter.sharedMesh;

        // Se la mesh non è leggibile, non possiamo accedere ai triangoli
        if (!mesh.isReadable)
        {
            Debug.LogWarning("Mesh is not readable. Cannot determine material by triangle index.");
            // Come fallback, restituisci il primo materiale disponibile
            return cachedMaterials.Length > 0 ? cachedMaterials[0] : null;
        }

        // Calcola l'indice della sub-mesh a partire dall'indice del triangolo
        int totalTriangles = 0;
        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            int[] triangles = mesh.GetTriangles(i);
            totalTriangles += triangles.Length / 3;

            if (triangleIndex < totalTriangles)
            {
                return cachedMaterials[i];
            }
        }

        Debug.LogWarning("Triangle index out of range.");
        return null;
    }
}
