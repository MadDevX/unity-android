using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomizedSet<T> where T : IRandomizable
{
    public static T Generate(List<T> entries)
    {
        float random = Random.Range(0.0f, ProbabilitySum(entries));
        int index = 0;
        float currentProbability = entries[0].GetProbability();
        while (random > currentProbability)
        {
            index++;
            currentProbability += entries[index].GetProbability();
        }
        return entries[index];
    }

    private static float ProbabilitySum(List<T> entries)
    {
        float sum = 0.0f;
        foreach (var entry in entries)
        {
            sum += entry.GetProbability();
        }
        return sum;
    }
}
