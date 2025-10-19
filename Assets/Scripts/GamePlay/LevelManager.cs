using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private LevelDatabase levelDatabase;
    private LevelLayoutData currentLevelLayout;
    private bool[,] levelLayout;
    private GridManager grid;

    public void SetGridManager(GridManager grid)
    {
        this.grid = grid;
    }

    // Gọi để nạp layout mới
    public void LoadNewLevelData()
    {
        if (levelDatabase == null)
        {
            Debug.LogError("❌ Level Database not assigned!");
            CreateDefaultLayout();
            return;
        }

        // Giả sử level hiện tại là 1 (bạn có thể thay bằng GameManager.Instance.CurrentLevel)
        currentLevelLayout = levelDatabase.GetLevelLayout(1);
        if (currentLevelLayout == null)
        {
            Debug.LogWarning("⚠️ Level layout not found, using default layout.");
            CreateDefaultLayout();
            return;
        }

        grid.Width = currentLevelLayout.width;
        grid.Height = currentLevelLayout.height;

        levelLayout = new bool[grid.Width, grid.Height];
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                levelLayout[x, y] = currentLevelLayout.GetCell(x, y);
            }
        }

        Debug.Log($"✅ Loaded level data: {grid.Width}x{grid.Height}");
        grid.SetLevelLayout(levelLayout);
    }

    private void CreateDefaultLayout()
    {
        levelLayout = new bool[grid.Width, grid.Height];
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                levelLayout[x, y] = true;
            }
        }
        grid.SetLevelLayout(levelLayout);
    }

}
