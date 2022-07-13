using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientTypeManagerCollection
{
    public GradientTypeManager[] gradients;

    public float[,] GetCombinedGradientsArray(int width, int height)
    {
        float[,] combinedArray = new float[width, height];
        float maxHeight = float.MinValue;
        float minHeight = float.MaxValue;

        foreach(GradientTypeManager gtm in gradients)
        {
            Gradient gradient = gtm.GenerateGradientBasedOnGradientType();
            float[,] gradientMap = gradient.Generate(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    combinedArray[x, y] += gradientMap[x, y];
                    float currentHeight = combinedArray[x, y];

                    if (maxHeight < currentHeight)
                        maxHeight = currentHeight;
                    if (minHeight > currentHeight)
                        minHeight = currentHeight;
                }
            }
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                combinedArray[x, y] = Mathf.InverseLerp(minHeight, maxHeight, combinedArray[x, y]);
            }
        }
        return combinedArray;
    }
}


[System.Serializable]
public class GradientTypeManager
{
    public enum GradientType { Linear, Radial, Square }

    public GradientType gradientType;
    public float figureSize;
    public Vector2 figurePosition;

    public Gradient GenerateGradientBasedOnGradientType()
    {
        switch (gradientType)
        {
            case GradientType.Linear:
                return new LinearGradient(figureSize);
            case GradientType.Radial:
                return new RadialGradient(figureSize, figurePosition);
            case GradientType.Square:
                return new SquareGradient(figureSize);
            default:
                throw new System.Exception("Unhandled type of gradient!");
        }
    }
}
