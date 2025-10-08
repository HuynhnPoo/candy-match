#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelLayoutData))]
public class LevelLayoutDataEditor : Editor
{
    private LevelLayoutData levelLayout;
    private const float cellSize = 30f;
    private const float spacing = 2f;
    private bool isDragging = false;
    private bool dragValue = false;

    private void OnEnable()
    {
        levelLayout = (LevelLayoutData)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Header
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Level Layout Editor", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        // Level Info
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        levelLayout.levelNumber = EditorGUILayout.IntField("Level Number", levelLayout.levelNumber);
        levelLayout.levelName = EditorGUILayout.TextField("Level Name", levelLayout.levelName);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(10);

        // Grid Settings
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Grid Settings", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();
        int newWidth = EditorGUILayout.IntSlider("Width", levelLayout.width, 3, 12);
        int newHeight = EditorGUILayout.IntSlider("Height", levelLayout.height, 3, 12);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(levelLayout, "Change Grid Size");
            levelLayout.width = newWidth;
            levelLayout.height = newHeight;
            levelLayout.ValidateLayout();
            EditorUtility.SetDirty(levelLayout);
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(10);

        // Grid Editor
        DrawGridEditor();

        EditorGUILayout.Space(10);

        // Quick Actions
        DrawQuickActions();

        EditorGUILayout.Space(10);

        // Level Goals
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Level Goals", EditorStyles.boldLabel);
        levelLayout.targetScore = EditorGUILayout.IntField("Target Score", levelLayout.targetScore);
        levelLayout.movesLimit = EditorGUILayout.IntField("Moves Limit", levelLayout.movesLimit);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(10);

        // Visual Settings
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Visual Settings", EditorStyles.boldLabel);
        levelLayout.levelColor = EditorGUILayout.ColorField("Level Color", levelLayout.levelColor);
        levelLayout.backgroundSprite = (Sprite)EditorGUILayout.ObjectField("Background Sprite", levelLayout.backgroundSprite, typeof(Sprite), false);
        EditorGUILayout.EndVertical();

        // Stats
        EditorGUILayout.Space(10);
        EditorGUILayout.HelpBox($"Active Cells: {levelLayout.GetActiveCellCount()} / {levelLayout.width * levelLayout.height}", MessageType.Info);

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(levelLayout);
        }
    }

    private void DrawGridEditor()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Grid Layout Editor", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Click: Toggle cell | Drag: Paint multiple cells", MessageType.Info);

        Event e = Event.current;

        float gridWidth = levelLayout.width * (cellSize + spacing) + spacing;
        float gridHeight = levelLayout.height * (cellSize + spacing) + spacing;

        Rect gridRect = GUILayoutUtility.GetRect(gridWidth, gridHeight);

        // Background
        EditorGUI.DrawRect(gridRect, new Color(0.2f, 0.2f, 0.2f, 1f));

        // Draw grid cells (from top to bottom for visual clarity)
        for (int y = levelLayout.height - 1; y >= 0; y--)
        {
            for (int x = 0; x < levelLayout.width; x++)
            {
                float xPos = gridRect.x + spacing + x * (cellSize + spacing);
                float yPos = gridRect.y + spacing + (levelLayout.height - 1 - y) * (cellSize + spacing);

                Rect cellRect = new Rect(xPos, yPos, cellSize, cellSize);

                bool cellValue = levelLayout.GetCell(x, y);

                // Draw cell
                Color cellColor = cellValue ? new Color(0.3f, 0.8f, 0.3f, 1f) : new Color(0.5f, 0.5f, 0.5f, 1f);
                EditorGUI.DrawRect(cellRect, cellColor);

                // Border
                Handles.color = Color.black;
                Handles.DrawSolidRectangleWithOutline(cellRect, Color.clear, new Color(0f, 0f, 0f, 0.5f));

                // Coordinate text
                GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
                labelStyle.alignment = TextAnchor.MiddleCenter;
                labelStyle.fontSize = 8;
                labelStyle.normal.textColor = cellValue ? Color.white : Color.gray;
                GUI.Label(cellRect, $"{x},{y}", labelStyle);

                // Handle mouse events
                if (e.type == EventType.MouseDown && cellRect.Contains(e.mousePosition))
                {
                    Undo.RecordObject(levelLayout, "Toggle Cell");
                    levelLayout.SetCell(x, y, !cellValue);
                    isDragging = true;
                    dragValue = !cellValue;
                    EditorUtility.SetDirty(levelLayout);
                    e.Use();
                }
                else if (isDragging && e.type == EventType.MouseDrag && cellRect.Contains(e.mousePosition))
                {
                    if (levelLayout.GetCell(x, y) != dragValue)
                    {
                        Undo.RecordObject(levelLayout, "Paint Cells");
                        levelLayout.SetCell(x, y, dragValue);
                        EditorUtility.SetDirty(levelLayout);
                        e.Use();
                    }
                }
            }
        }

        if (e.type == EventType.MouseUp)
        {
            isDragging = false;
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawQuickActions()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Quick Actions", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Fill All", GUILayout.Height(30)))
        {
            Undo.RecordObject(levelLayout, "Fill All");
            levelLayout.FillAll();
            EditorUtility.SetDirty(levelLayout);
        }

        if (GUILayout.Button("Clear All", GUILayout.Height(30)))
        {
            Undo.RecordObject(levelLayout, "Clear All");
            levelLayout.ClearAll();
            EditorUtility.SetDirty(levelLayout);
        }

        if (GUILayout.Button("Invert", GUILayout.Height(30)))
        {
            Undo.RecordObject(levelLayout, "Invert All");
            levelLayout.InvertAll();
            EditorUtility.SetDirty(levelLayout);
        }

        EditorGUILayout.EndHorizontal();

        // Preset Shapes
        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Preset Shapes", EditorStyles.miniBoldLabel);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Cross", GUILayout.Height(25)))
        {
            Undo.RecordObject(levelLayout, "Create Cross");
            CreateCrossShape();
            EditorUtility.SetDirty(levelLayout);
        }

        if (GUILayout.Button("Diamond", GUILayout.Height(25)))
        {
            Undo.RecordObject(levelLayout, "Create Diamond");
            CreateDiamondShape();
            EditorUtility.SetDirty(levelLayout);
        }

        if (GUILayout.Button("Frame", GUILayout.Height(25)))
        {
            Undo.RecordObject(levelLayout, "Create Frame");
            CreateFrameShape();
            EditorUtility.SetDirty(levelLayout);
        }

        if (GUILayout.Button("Checkerboard", GUILayout.Height(25)))
        {
            Undo.RecordObject(levelLayout, "Create Checkerboard");
            CreateCheckerboardShape();
            EditorUtility.SetDirty(levelLayout);
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

    private void CreateCrossShape()
    {
        levelLayout.ClearAll();
        int midX = levelLayout.width / 2;
        int midY = levelLayout.height / 2;

        for (int x = 0; x < levelLayout.width; x++)
        {
            levelLayout.SetCell(x, midY, true);
        }

        for (int y = 0; y < levelLayout.height; y++)
        {
            levelLayout.SetCell(midX, y, true);
        }
    }

    private void CreateDiamondShape()
    {
        levelLayout.ClearAll();
        int midX = levelLayout.width / 2;
        int midY = levelLayout.height / 2;
        int radius = Mathf.Min(midX, midY);

        for (int x = 0; x < levelLayout.width; x++)
        {
            for (int y = 0; y < levelLayout.height; y++)
            {
                int distance = Mathf.Abs(x - midX) + Mathf.Abs(y - midY);
                if (distance <= radius)
                {
                    levelLayout.SetCell(x, y, true);
                }
            }
        }
    }

    private void CreateFrameShape()
    {
        levelLayout.FillAll();

        for (int x = 1; x < levelLayout.width - 1; x++)
        {
            for (int y = 1; y < levelLayout.height - 1; y++)
            {
                levelLayout.SetCell(x, y, false);
            }
        }
    }

    private void CreateCheckerboardShape()
    {
        for (int x = 0; x < levelLayout.width; x++)
        {
            for (int y = 0; y < levelLayout.height; y++)
            {
                levelLayout.SetCell(x, y, (x + y) % 2 == 0);
            }
        }
    }
}
#endif