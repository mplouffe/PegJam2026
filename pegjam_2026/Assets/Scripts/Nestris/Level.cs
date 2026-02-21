using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level
{
    public string LevelName;

    public List<Goal> LevelGoals;

    public bool LevelPassed()
    {
        int passes = 0;
        foreach(var goal in LevelGoals)
        {
            if (goal.GoalCompleted())
            {
                passes++;
            }
        }
        return passes >= LevelGoals.Count / 2;
    }
}
