using UnityEngine;

public static class ScoreHelper
{
    public static float RandomVariance(float min, float max)
    {
        return Random.Range(min, max);
    }
}