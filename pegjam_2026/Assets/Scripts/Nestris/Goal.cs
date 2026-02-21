using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Goal : MonoBehaviour
{
    public EGoalType Type {  get; protected set; }

    public string Description;

    public abstract bool GoalCompleted();
}

public enum EGoalType
{
    Score,
    CardType,
}
