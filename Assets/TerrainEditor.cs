using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainEditor : MonoBehaviour
{

    public Terrain terrain;
    public int brush;
    public float strenght;
    private TerrainData terrainData;
    private int heightmapHeight;
    private int heightmapWidth;
    private float[,] heights;

    // Start is called before the first frame update
    void Start()
    {
        terrainData = terrain.terrainData;
        heightmapHeight = terrainData.heightmapResolution;
        heightmapWidth = terrainData.heightmapResolution;
        heights = terrainData.GetHeights(0, 0, heightmapWidth, heightmapHeight);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButton(0)){
            if (Physics.Raycast(ray, out hit)){
                ChangeHeight(hit.point, brush, strenght);
            }
        }
        if (Input.GetMouseButton(1)){
            if (Physics.Raycast(ray, out hit)){
                ChangeHeight(hit.point, brush, -strenght);
            }
        }

    }

    public void ChangeHeight(Vector3 point, int brushSize, float heightDelta){
        int mouseX = (int)((point.x / terrainData.size.x) * heightmapWidth);
        int mouseZ = (int)((point.z / terrainData.size.z) * heightmapHeight);

        float[,] modifiedHeights = new float[brushSize, brushSize];
        float heightDeltaCoef = heightDelta * Time.deltaTime;

        for (int x = 0; x < brushSize; x++){
            for (int z = 0; z < brushSize; z++){
                float y = heights[mouseX-brushSize/2+x, mouseZ-brushSize/2+z] + (1/Mathf.Max(1, Mathf.Sqrt(Mathf.Pow(x - brushSize/2, 2) + Mathf.Pow(z - brushSize/2, 2))))*heightDeltaCoef;
                y = Mathf.Max(Mathf.Min(y, terrainData.size.y), 0);
                heights[mouseX-brushSize/2+x, mouseZ-brushSize/2+z] = y;
                modifiedHeights[x, z] = y;
            }
        }

        //Debug.Log(modifiedHeights);
        //modifiedHeights[0, 0] = y;
        //heights[mouseX, mouseZ] = y;
        terrainData.SetHeights(mouseX-brushSize/2, mouseZ-brushSize/2, modifiedHeights);
    }
}
