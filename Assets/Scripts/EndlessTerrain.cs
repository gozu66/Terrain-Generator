using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndlessTerrain : MonoBehaviour
{
    public const float maxViewDist = 450;
    public Transform viewer;

    public Material mapMaterial;

    public static Vector2 viewerPosition;
    static MapGenerator mapGenerator;
    int chunkSize;
    int chunksVisableInViewDst;

    Dictionary<Vector2, TerrainChunk> TerrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

    void Start()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();
        chunkSize = MapGenerator.mapChunkSize - 1;
        chunksVisableInViewDst = Mathf.RoundToInt(maxViewDist / chunkSize);
    }

    void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
        UpdateVisibleChunks();
    }

    void UpdateVisibleChunks()
    {
        for(int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++)
        {
            terrainChunksVisibleLastUpdate[i].SetVisible(false);
        }
        terrainChunksVisibleLastUpdate.Clear();

        int currentChunkCoorX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCoorY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        for(int yOffset = -chunksVisableInViewDst; yOffset <= chunksVisableInViewDst; yOffset++)
        {
            for(int xOffset = -chunksVisableInViewDst; xOffset <= chunksVisableInViewDst; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoorX + xOffset, currentChunkCoorY + yOffset);

                if(TerrainChunkDictionary.ContainsKey(viewedChunkCoord))
                {
                    TerrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();

                    if(TerrainChunkDictionary[viewedChunkCoord].isVisible())
                    {
                        terrainChunksVisibleLastUpdate.Add(TerrainChunkDictionary[viewedChunkCoord]);
                    }
                }
                else
                {
                    TerrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, transform, mapMaterial));
                }
            }
        }
    }

    public class TerrainChunk
    {
        GameObject meshObject;
        Vector2 position;
        Bounds bounds;

        MeshRenderer meshRenderer;
        MeshFilter meshFilter;

        public TerrainChunk(Vector2 coord, int size, Transform parent, Material material)
        {
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 positionV3 = new Vector3(position.x, 0, position.y);

            meshObject = new GameObject("Terrain Chunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();

            meshRenderer.material = material;

            meshObject.transform.position = positionV3;

            meshObject.transform.parent = parent;

            SetVisible(false);

            mapGenerator.RequestMapData(onMapDataRecieved);
        }

        void onMapDataRecieved(MapData mapData)
        {
            mapGenerator.RequestMeshData(mapData, OnMeshDataRecieved); 
        }

        void OnMeshDataRecieved(MeshData meshData)
        {
            meshFilter.mesh = meshData.CreateMesh();
        }
 
        public void UpdateTerrainChunk()
        {
            float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
            bool visible = viewerDstFromNearestEdge <= maxViewDist;
            SetVisible(visible);
        }

        public void SetVisible(bool visible)
        {
            meshObject.SetActive(visible);
        }

        public bool isVisible()
        {
            return meshObject.activeSelf;
        }
    }
}