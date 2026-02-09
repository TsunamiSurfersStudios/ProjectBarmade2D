using UnityEngine;

[System.Serializable]
public class EventCondition
{
    public enum ConditionType
    {
        None,           // No condition checking
        IntEquals,
        FloatEquals,
        StringEquals,
        BoolEquals,
        GameObjectEquals
    }

    public ConditionType conditionType = ConditionType.None;

    // Expected values (set in inspector)
    public int expectedInt;
    public float expectedFloat;
    public string expectedString;
    public bool expectedBool;
    public GameObject expectedGameObject;

    public bool Evaluate(object actualValue)
    {
        if (conditionType == ConditionType.None)
            return true; // No condition = always pass

        switch (conditionType)
        {
            case ConditionType.IntEquals:
                return actualValue is int intVal && intVal == expectedInt;

            case ConditionType.FloatEquals:
                return actualValue is float floatVal && Mathf.Approximately(floatVal, expectedFloat);

            case ConditionType.StringEquals:
                return actualValue is string strVal && strVal == expectedString;

            case ConditionType.BoolEquals:
                return actualValue is bool boolVal && boolVal == expectedBool;

            case ConditionType.GameObjectEquals:
                return actualValue is GameObject objVal && objVal == expectedGameObject;

            default:
                return true;
        }
    }
}