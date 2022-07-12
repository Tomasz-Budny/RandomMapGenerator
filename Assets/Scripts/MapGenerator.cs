using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColourMap }
    public enum TypeOfGeneratedTerrain { Normal, Island}
    public enum GradientType { Linear, Radial, Square}

    public GradientType gradientType;
    [Range(-1, 10)]
    public float power;
    public Gradient gradient;
    public TypeOfGeneratedTerrain typeOfGeneratedTerrain;
    [Range(0,10)]
    public float middleSquereSize;
    public DrawMode drawMode;

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;
    public int octaves;
    [Range(0, 1)]
    public float persistance;
    [Range(1, 3)]
    public float lacunarity;
    public int seed;
    public Vector2 offSet;

    public bool autoUpdate;

    public TerrainType[] regions;

    public void GenerateMap()
    {
        float[,] noiseMap;
        float[,] gradMap;

        GenerateGradientBasingOnGradientType();
        gradMap = GenerateGradArrDependingOnGradientType();
        noiseMap = Noise.GeneratePerlinNoiseModifiedByGrad(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offSet, gradMap);

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
    private void GenerateGradientBasingOnGradientType()
    {
        switch (gradientType)
        {
            case GradientType.Linear:
                gradient = new LinearGradient(power);
                break;
            case GradientType.Radial:
                gradient = new RadialGradient(power);
                break;
            case GradientType.Square:
                gradient = new SquareGradient(power);
                break;
            default:
                gradient = new Gradient(power);
                break;
        }
    }

    private float[,] GenerateGradArrDependingOnGradientType()
    {
        switch(gradientType)
        {
            case GradientType.Linear:
                return ((LinearGradient)gradient).Generate(mapWidth, mapHeight);
            case GradientType.Radial:
                return ((RadialGradient)gradient).Generate(mapWidth, mapHeight);
            case GradientType.Square:
                return ((SquareGradient)gradient).Generate(mapWidth, mapHeight);
            default:
                return new float[mapWidth, mapHeight];
        }
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

        if (lacunarity < 1)
            lacunarity = 1;

        if (octaves < 0)
            octaves = 1;
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
