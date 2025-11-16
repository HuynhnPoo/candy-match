using UnityEngine;

[CreateAssetMenu(fileName = "LevelLayout", menuName = "Candy Crush/Level Layout", order = 1)]
public class LevelLayoutData : ScriptableObject
{
    [Header("Level Info")]
    public int levelNumber;
    public string levelName;

    [Header("Grid Settings")]
    public int width = 8;
    public int height = 8;

    [Header("Layout Configuration")]
    [HideInInspector]
    public bool[] layoutData; // Lưu dạng 1D array

    [Header("Level Goals")]
    public int targetScore = 1000;
    public int movesLimit = 30;

    [Header("Visual Settings")]
    public Color levelColor = Color.white;
    public Sprite backgroundSprite;

    private void OnEnable()
    {
        ValidateLayout();
    }

    private void OnValidate()
    {
        ValidateLayout();
    }

    public void ValidateLayout()
    {
        int requiredSize = width * height;

        if (layoutData == null || layoutData.Length != requiredSize)
        {
            bool[] newLayout = new bool[requiredSize];

            // Copy dữ liệu cũ nếu có
            if (layoutData != null)
            {
                int copyLength = Mathf.Min(layoutData.Length, requiredSize);
                System.Array.Copy(layoutData, newLayout, copyLength);
            }

            // Fill phần còn lại = true
            for (int i = (layoutData != null ? layoutData.Length : 0); i < requiredSize; i++)
            {
                newLayout[i] = true;
            }

            layoutData = newLayout;
        }
    }

    // Get cell value tại vị trí x, y
    public bool GetCell(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return false;

        int index = y * width + x;
        return layoutData[index];
    }

    // Set cell value tại vị trí x, y
    public void SetCell(int x, int y, bool value)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return;

        int index = y * width + x;
        layoutData[index] = value;
    }

    // Đếm số ô active
    public int GetActiveCellCount()
    {
        int count = 0;
        foreach (bool cell in layoutData)
        {
            if (cell) count++;
        }
        return count;
    }

    // Fill tất cả
    public void FillAll()
    {
        for (int i = 0; i < layoutData.Length; i++)
        {
            layoutData[i] = true;
        }
    }

    // Clear tất cả
    public void ClearAll()
    {
        for (int i = 0; i < layoutData.Length; i++)
        {
            layoutData[i] = false;
        }
    }

    // Invert tất cả
    public void InvertAll()
    {
        for (int i = 0; i < layoutData.Length; i++)
        {
            layoutData[i] = !layoutData[i];
        }
    }
}
