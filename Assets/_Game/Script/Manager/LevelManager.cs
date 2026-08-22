using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] LevelData levelData;
    [SerializeField] int currentLevelIndex = 0;
    [SerializeField] protected Player player;

    protected GameObject currentMap;

    public void OnInit()
    {
        LoadLevel(currentLevelIndex);
    }

    public void LoadLevel(int index)
    {
        SingleLevelData level = levelData.GetLevel(index);

        BotManager.Instance.ClearBots();
        CacheManager.ClearNavMeshCache();
        if (currentMap != null)
        {
            Destroy(currentMap);
        }

        if (level.map != null)
        {
            currentMap = Instantiate(level.map, Vector3.zero, Quaternion.identity);
        }

        currentLevelIndex = index;
        BotManager.Instance.ApplyLevelData(level);
        BotManager.Instance.Init();
    }
    public void StartGame()
    {
        player.ShowRangeUI();
    }

    public void NextLevel()
    {
        int nextLevelIndex = (currentLevelIndex + 1) % levelData.Count;
        LoadLevel(nextLevelIndex);
    }
}
