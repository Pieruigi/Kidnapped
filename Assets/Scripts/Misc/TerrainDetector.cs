using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class TerrainDetector: MonoBehaviour
{
    private TerrainData terrainData;
    private int alphamapWidth;
    private int alphamapHeight;
    private float[,,] splatmapData;
    private int numTextures;

    Terrain terrain;

    void Awake()
    {
        terrain = GetComponent<Terrain>();

        terrainData = terrain.terrainData;// Terrain.activeTerrain.terrainData;
        alphamapWidth = terrainData.alphamapWidth;
        alphamapHeight = terrainData.alphamapHeight;

        splatmapData = terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
        numTextures = splatmapData.Length / (alphamapWidth * alphamapHeight);

       
    }

    //public TerrainDetector()
    //{
    //    terrainData = Terrain.activeTerrain.terrainData;
    //    alphamapWidth = terrainData.alphamapWidth;
    //    alphamapHeight = terrainData.alphamapHeight;

    //    splatmapData = terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
    //    numTextures = splatmapData.Length / (alphamapWidth * alphamapHeight);
    //}

    private Vector3 ConvertToSplatMapCoordinate(Vector3 worldPosition)
    {
        Vector3 splatPosition = new Vector3();
        //Terrain ter = Terrain.activeTerrain;
        Vector3 terPosition = terrain.transform.position;
        splatPosition.x = ((worldPosition.x - terPosition.x) / terrain.terrainData.size.x) * terrain.terrainData.alphamapWidth;
        splatPosition.z = ((worldPosition.z - terPosition.z) / terrain.terrainData.size.z) * terrain.terrainData.alphamapHeight;
        return splatPosition;
    }

    public Texture2D GetTexture(Vector3 position)
    {
        Vector3 terrainCord = ConvertToSplatMapCoordinate(position);
        int activeTerrainIndex = 0;
        float largestOpacity = 0f;

        for (int i = 0; i < numTextures; i++)
        {
            if (largestOpacity < splatmapData[(int)terrainCord.z, (int)terrainCord.x, i])
            {
                activeTerrainIndex = i;
                largestOpacity = splatmapData[(int)terrainCord.z, (int)terrainCord.x, i];
            }
        }

        //Debug.Log($"TexName:{terrainData.terrainLayers[activeTerrainIndex].diffuseTexture}");

        //return activeTerrainIndex;

        return terrainData.terrainLayers[activeTerrainIndex].diffuseTexture;
    }

    

}
