using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Item))]
public class GridPieceEditor : Editor
{
    //public override void OnInspectorGUI()
    //{
    //    Item piece = (Item)target;
    //    BoolGrid grid = piece.ItemShape;

    //    DrawDefaultInspector();

    //    EditorGUILayout.Space(8);

    //    // Initialize if needed
    //    if (grid.cells == null || grid.cells.Length != grid.size * grid.size)
    //        grid.Resize(grid.size);

    //    EditorGUI.BeginChangeCheck();

    //    // Size field
    //    int newSize = EditorGUILayout.IntSlider("Grid Size", grid.size, 1, 8);
    //    if (newSize != grid.size)
    //        grid.Resize(newSize);

    //    EditorGUILayout.Space(4);

    //    // Draw the grid
    //    float cellSize = 32f;
    //    for (int r = 0; r < grid.size; r++)
    //    {
    //        EditorGUILayout.BeginHorizontal();
    //        for (int c = 0; c < grid.size; c++)
    //        {
    //            bool current = grid.Get(r, c);
    //            Color prev = GUI.backgroundColor;
    //            GUI.backgroundColor = current ? Color.green : Color.grey;

    //            if (GUILayout.Button("", GUILayout.Width(cellSize), GUILayout.Height(cellSize)))
    //            {
    //                Undo.RecordObject(piece, "Toggle Grid Cell");
    //                grid.Set(r, c, !current);
    //            }

    //            GUI.backgroundColor = prev;
    //        }
    //        EditorGUILayout.EndHorizontal();
    //    }

    //    if (EditorGUI.EndChangeCheck())
    //        EditorUtility.SetDirty(piece);
    //}
}