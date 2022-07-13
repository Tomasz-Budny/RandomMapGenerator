using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColourMap }

    public GradientTypeManagerCollection gradients;
    public DrawMode drawMode;

    public int mapWidth;
    public int mapHeight;

    public Noise noise;

    public bool autoUpdate;

    public TerrainType[] regions;

    public void GenerateMap()
    {
        float[,] noiseMap;
        float[,] gradMap;

        gradMap = gradients.GetCombinedGradientsArray(mapWidth, mapHeight);
        noiseMap = noise.GeneratePerlinNoiseModifiedByGrad(mapWidth, mapHeight, gradMap);

        Color[] colourMap = new Color[mapWidth * mapHeight];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentheight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if(currentheight <= regions[i].height){
                        colourMap[y * mapWidth + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }

        mapDisplay displayMap = FindObjectOfType<mapDisplay>();
        if(drawMode == DrawMode.NoiseMap){
            displayMap.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }else if(drawMode == DrawMode.ColourMap){
            displayMap.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight));
        }
        GenerateGradient(gradMap);
    }

    public void GenerateGradient(float[,] gradient)
    {
        GradientDisplay display = FindObjectOfType<GradientDisplay>();
        display.DrawTexture(TextureGenerator.TextureFromHeightMap(gradient));
    }

    private void OnValidate()
    {
        if (mapHeight < 1)
            mapHeight = 1;
        if (mapWidth < 1)
            mapWidth = 1;
    }
}

[System.Serializable]
public struct TerrainType
{
    public string GetName()
    {
        return name;
    }

    public string name;
    public float height;
    public Color colour;
}
