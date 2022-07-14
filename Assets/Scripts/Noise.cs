using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Noise
{
    public float noiseScale;
    public int octaves;
    [Range(0, 1)]
    public float persistance;
    [Range(1, 3)]
    public float lacunarity;
    public int seed;
    public Vector2 offSet;
    public float[,] GenerateNoise(int mapWidth, int mapHeight)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        for (int i = 0; i < octaves; i++)
        {
            float offSetX = prng.Next(-10000, 100000) + offSet.x;
            float offSetY = prng.Next(-10000, 100000) + offSet.y;

            octaveOffsets[i] = new Vector2(offSetX, offSetY);
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2;
        float halfHeight = mapHeight / 2;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                // Z tego trzeba bêdzie oddzeln¹ metodê zrobiæ
                for (int i = 0; i < octaves; i++)
                {
                    
                    float xCoord = (x - halfWidth) / noiseScale * frequency + octaveOffsets[i].x;
                    float yCoord = (y - halfHeight) / noiseScale * frequency + octaveOffsets[i].y;
                    float noiseVal = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 1;

                    noiseHeight += noiseVal * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                if (noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;

                noiseMap[x, y] = noiseHeight;
            }
        }

        noiseMap = NormalizeNoiseMapUsingInverseLerp(noiseMap, minNoiseHeight, maxNoiseHeight);
        
        return noiseMap;
    }

    private  float[,] NormalizeNoiseMapUsingInverseLerp(float[,] noiseMap, float minNoiseHeight, float maxNoiseHeight)
    {
        int mapHeight = noiseMap.GetLength(1);
        int mapWidth = noiseMap.GetLength(0);

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }

    public  float[,] GeneratePerlinNoiseModifiedByGrad(int mapWidth, int mapHeight, float[,] gradientMap)
    {
        float[,] noiseMap = GenerateNoise(mapWidth, mapHeight);

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;
        float noiseHeight = 0;

        float[,] squareNoiseMap = new float[mapWidth, mapHeight];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseHeight = noiseMap[x, y] - gradientMap[x, y];
                squareNoiseMap[x, y] = noiseHeight;

                if (noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;
            }
        }

        return squareNoiseMap;
    }
}
