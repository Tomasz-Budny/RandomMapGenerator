using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gradient
{
    protected Gradient(float figureSize)
    {
        this.figureSize = figureSize;
    }
    public float figureSize;
    public abstract float[,] Generate(int width, int height);
     
}

public class LinearGradient : Gradient
{ 
    public LinearGradient(float figureSize) : base(figureSize) { }
    public override float[,] Generate(int width, int height)
    {
        float halfWidth = width / 2f;

        float[,] gradient = new float[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int x = i;
                float colorValue = (float)x / width;
                colorValue = 1 - colorValue;
                colorValue *= Mathf.Pow(colorValue, figureSize);
                gradient[i, j] = colorValue;
            }
        }
        return gradient;
    }
}

public class SquareGradient : Gradient
{
    public SquareGradient(float figureSize): base(figureSize) { }
    public override float[,] Generate(int width, int height)
    {
        int halfWidth = width / 2;
        int halfHeight = height / 2;

        float[,] gradient = new float[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int x = i;
                int y = j;

                float colorValue;

                x = x > halfWidth ? width - x : x;
                y = y > halfHeight ? height - y : y;

                int smaller = x < y ? x : y;
                colorValue = smaller / (float)halfWidth;

                colorValue = colorValue;  
                colorValue *= Mathf.Pow(colorValue, figureSize);
                gradient[i, j] = colorValue;
            }
        }

        return gradient;
    }
}

public class RadialGradient : Gradient
{
    Vector2 position;
    public RadialGradient(float figureSize, Vector2 position): base(figureSize) { this.position = position; }
    public override float[,] Generate(int width, int height)
    {
        int halfWidth = width / 2;
        int halfHeight = height / 2;

        float[,] gradient = new float[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int x = i;
                int y = j;

                float colorValue;
                float distanceToCenter = Mathf.Sqrt(Mathf.Pow(x - halfWidth + position.x, 2) + Mathf.Pow(y - halfHeight + position.y, 2));
                distanceToCenter = distanceToCenter / (Mathf.Sqrt(2) * halfWidth + Mathf.Abs(position.x) + Mathf.Abs(position.y));
                colorValue =  1 - distanceToCenter;
                colorValue *= Mathf.Pow(colorValue, figureSize);
                gradient[i, j] = colorValue;
            }
        }
        return gradient;
    }
}
