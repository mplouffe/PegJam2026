using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(BoolGrid))]
public class BoolGridDrawer : PropertyDrawer
{
    private const float CellSize = 20f;
    private const float Padding = 2f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty sizeProperty = property.FindPropertyRelative("size");
        SerializedProperty cellsProperty = property.FindPropertyRelative("cells");

        int size = sizeProperty.intValue;

        // Ensure cells array is initialized
        if (cellsProperty.arraySize != size * size)
        {
            cellsProperty.arraySize = size * size;
        }

        // Draw label
        Rect labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(labelRect, label);

        // Draw size slider
        Rect sliderRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + Padding, position.width, EditorGUIUtility.singleLineHeight);
        int newSize = EditorGUI.IntSlider(sliderRect, "Grid Size", size, 1, 8);
        if (newSize != size)
        {
            // Resize while preserving data
            bool[] oldCells = new bool[size * size];
            for (int i = 0; i < cellsProperty.arraySize; i++)
                oldCells[i] = cellsProperty.GetArrayElementAtIndex(i).boolValue;

            sizeProperty.intValue = newSize;
            cellsProperty.arraySize = newSize * newSize;

            int copySize = Mathf.Min(size, newSize);
            for (int r = 0; r < newSize; r++)
                for (int c = 0; c < newSize; c++)
                {
                    bool val = r < copySize && c < copySize && oldCells[r * size + c];
                    cellsProperty.GetArrayElementAtIndex(r * newSize + c).boolValue = val;
                }

            size = newSize;
        }

        // Draw grid
        float gridTop = sliderRect.y + EditorGUIUtility.singleLineHeight + Padding;
        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
            {
                Rect cellRect = new Rect(
                    position.x + c * (CellSize + Padding),
                    gridTop + r * (CellSize + Padding),
                    CellSize, CellSize);

                int index = r * size + c;
                SerializedProperty cellProp = cellsProperty.GetArrayElementAtIndex(index);
                bool current = cellProp.boolValue;

                Color prev = GUI.backgroundColor;
                GUI.backgroundColor = current ? Color.green : Color.grey;

                if (GUI.Button(cellRect, ""))
                    cellProp.boolValue = !current;

                GUI.backgroundColor = prev;
            }
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty sizeProperty = property.FindPropertyRelative("size");
        int size = sizeProperty.intValue;

        float labelHeight = EditorGUIUtility.singleLineHeight + Padding;
        float sliderHeight = EditorGUIUtility.singleLineHeight + Padding;
        float gridHeight = size * (CellSize + Padding);

        return labelHeight + sliderHeight + gridHeight + Padding;
    }
}