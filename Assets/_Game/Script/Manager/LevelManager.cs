using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] protected LevelData levelData;
    [SerializeField] protected int currentLevelIndex = 0;
    [SerializeField] protected Player player;

    protected GameObject currentMap;

    public void OnInit()
    {
        LoadLevel(currentLevelIndex);
        player.OnInit(levelData.GetPlayerPosition(), levelData.GetPlayerRotation());
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

        if (level.Map != null)
        {
            currentMap = Instantiate(level.Map, Vector3.zero, Quaternion.identity);
        }

        currentLevelIndex = index;
        BotManager.Instance.ApplyLevelData(level);
    }
    public void StartGame()
    {
        BotManager.Instance.Init();
        player.ShowRangeUI();
    }

    public void NextLevel()
    {
        int nextLevelIndex = (currentLevelIndex + 1) % levelData.Count;
        LoadLevel(nextLevelIndex);
    }
    public Player GetPlayer()
    {
        return player;
    }

    public void EndGame()
    {
        BotManager.Instance.ClearBots();
        CacheManager.ClearNavMeshCache();
        player.OnInit();
    }
    public void Retry()
    {
        EndGame();
        OnInit();
    }
}

