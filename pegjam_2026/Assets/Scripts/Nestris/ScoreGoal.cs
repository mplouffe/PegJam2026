using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScoreGoal : Goal
{
    public int TargetScore = 100;
    
    public override bool GoalCompleted()
    {
        return Nest.Instance.Score >= TargetScore;
    }
}
