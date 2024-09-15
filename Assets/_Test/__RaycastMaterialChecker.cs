using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __RaycastMaterialChecker : MonoBehaviour
{
    // Maximum distance for the Raycast
    public float rayDistance = 100f;

    // The Camera from which to cast the Ray (usually the Main Camera)
    public Camera camera;

    void Update()
    {
        // Perform the Raycast on mouse click
        if (Input.GetMouseButtonDown(0))
        {
            
            RaycastHit hit;

            // Perform the Raycast and check if it hits something
            if (Physics.Raycast(transform.position+Vector3.up, Vector3.down, out hit, 1f))
            {

                //if(hit.collider.GetComponent<MultiMaterialChecker>() != null)
                //{
                //    Debug.Log("Material:" +  hit.collider.GetComponent<MultiMaterialChecker>().GetMaterialByTriangleIndex(hit.triangleIndex).name);
                //}

                //return;

                // Get the MeshRenderer of the hit object
                MeshRenderer meshRenderer = hit.collider.GetComponent<MeshRenderer>();

                if (meshRenderer != null)
                {
                    // Get the Mesh of the object
                    MeshFilter meshFilter = hit.collider.GetComponent<MeshFilter>();

                    if (meshFilter != null && meshFilter.sharedMesh != null)
                    {
                        // Get the index of the hit triangle
                        int triangleIndex = hit.triangleIndex;

                        // Get the Mesh of the hit object
                        Mesh mesh = meshFilter.sharedMesh;

                        // Check if the mesh is readable (if not, handle it differently)
                        if (mesh.isReadable)
                        {
                            int totalTriangles = 0;

                            // Iterate through each sub-mesh
                            for (int i = 0; i < mesh.subMeshCount; i++)
                            {
                                // Count the number of triangles in the current sub-mesh
                                int[] triangles = mesh.GetTriangles(i);
                                totalTriangles += triangles.Length / 3;

                                // Check if the hit triangle index belongs to the current sub-mesh
                                if (triangleIndex < totalTriangles)
                                {
                                    // The hit triangle is part of this sub-mesh, get the material
                                    Material material = meshRenderer.sharedMaterials[i];
                                    Debug.Log("Hit Material: " + material.name);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            // Handle mesh that is not readable
                            // We cannot access the triangles, so we fallback to a safe approximation

                            if (mesh.subMeshCount > 0)
                            {
                                // Approximate: Use subMeshIndex 0 as a fallback
                                Material material = meshRenderer.sharedMaterials[0];
                                Debug.Log("Hit on non-readable mesh, returning first material: " + material.name);
                            }
                            else
                            {
                                Debug.LogWarning("Mesh has no sub-meshes or materials.");
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning("No mesh found on the hit object.");
                    }
                }
            






        }
        }
    }
}
