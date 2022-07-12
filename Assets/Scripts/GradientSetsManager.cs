using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GradientSetsManager
{
    public enum GradientType { Linear, Radial, Square }

    public GradientType gradientType;
    public float figureSize;

    public Gradient GenerateGradientBasedOnGradientType()
    {
        switch (gradientType)
        {
            case GradientType.Linear:
                return new LinearGradient(figureSize);
            case GradientType.Radial:
                return new RadialGradient(figureSize);
            case GradientType.Square:
                return new SquareGradient(figureSize);
            default:
                throw new System.Exception("Unhandled type of gradient!");
        }
    }
}
