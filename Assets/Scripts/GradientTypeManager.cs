using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GradientTypeManagerCollection
{
    public GradientTypeManager[] gradients;

    public float[,] GetCombinedGradientsArray(int width, int height)
    {
        float[,] combinedArray = new float[width, height];

        foreach (GradientTypeManager gtm in gradients)
        {
            Gradient gradient = gtm.GenerateGradientBasedOnGradientType();
            float[,] gradientMap = gradient.Generate(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float val = combinedArray[x, y] + gradientMap[x, y];
                    if (val >= 1)
                    {
                        combinedArray[x, y] = 1;
                    }
                    else
                    {
                        combinedArray[x, y] = val;
                    }
                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                combinedArray[x, y] = (combinedArray[x, y] - 1) * -1;
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
