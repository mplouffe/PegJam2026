using lvl_0;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Goal
{
    public EGoalType Type;

    public string Description;

    public int GoalCount = 100;

    public ECardType TargetType;

    public ECardType ComparisonType;

    public TypeOperator Operator;


    public bool GoalCompleted()
    {
        bool result = false;
        switch (Type)
        {
            case EGoalType.Score:
                result = Nest.Instance.Score >= GoalCount;
                break;
            case EGoalType.CardType:
                result = Operator switch
                {
                    TypeOperator.LessThan => GoalCount >= Nest.Instance.GetTileCount(TargetType),
                    TypeOperator.MoreThan => GoalCount <= Nest.Instance.GetTileCount(TargetType),
                    _ => false
                };
                break;
            case EGoalType.ProximityType:
                int count = Nest.Instance.GetProximityCount(TargetType, ComparisonType);
                result = Operator switch
                {
                    TypeOperator.LessThan => GoalCount >= count,
                    TypeOperator.MoreThan => GoalCount <= count,
                    _ => false
                };
                break;
        }
        return result;
    }
}

public enum EGoalType
{
    Score,
    CardType,
    ProximityType
}

public enum TypeOperator
{
    LessThan,       // actually <=
    MoreThan,       // actually >=
}
