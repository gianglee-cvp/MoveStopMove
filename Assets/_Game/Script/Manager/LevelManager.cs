using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] LevelData levelData;
    [SerializeField] int currentLevelIndex = 0;
    [SerializeField] protected Player player;

    protected GameObject currentMap;

    public void OnInit()
    {
        player.OnInit();
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
    public void ChangeAnimPlayer(CharacterAnimType type)
    {
        player.ChangeAnim(type);
    }
    public void PlayerTrySkin(int index , SkinType type)
    {
        player.TryCloth(index , type);
    }
    public void ReloadCloth()
    {
        player.ReloadCloth();
    }
}
