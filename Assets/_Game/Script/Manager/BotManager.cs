using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class BotManager : Singleton<BotManager>
{
    [SerializeField] protected float spawnRadius = 20f;
    [SerializeField] protected float respawnCheckInterval;
    [SerializeField] protected int totalBotCount;
    [SerializeField] protected int inMapBotCount;
    [SerializeField] protected LayerMask characterLayer;
    protected int currentBotActiveInMapCount => listBotActive.Count;
    protected List<Bot> listBotActive = new List<Bot>();
    protected float respawnTimer;

    public void Init()
    {
        respawnTimer = 0f;
        ClearBots();
        SpawnBot(inMapBotCount);
    }
    void Update()
    {
        if(!GameManager.Instance.IsGameState(Enum_GameState.Play)) return;
        if (currentBotActiveInMapCount >= inMapBotCount || totalBotCount <= currentBotActiveInMapCount)
        {
            return;
        }
        SpawnOneBotRandom();
        respawnTimer += Time.deltaTime;
        if (respawnTimer >= respawnCheckInterval)
        {
            respawnTimer = 0f;
        }

        respawnTimer = 0f;
        // RespawnMissingBots();
        
    }
    public void ApplyLevelData(SingleLevelData data)
    {
        if (data == null)
        {
            return;
        }
        totalBotCount = Mathf.Max(0, data.TotalBotCount);
        inMapBotCount = Mathf.Clamp(data.InMapBotCount, 0, totalBotCount);
        respawnTimer = 0f;
    }
    public void SpawnBot(int cnt)
    {
        for (int i = 0; i < cnt; i++)
        {
            SpawnOneBotRandom();
        }
    }
    protected void RespawnMissingBots()
    {
        int respawnCount = Mathf.Min(inMapBotCount - currentBotActiveInMapCount, totalBotCount - currentBotActiveInMapCount);
        for (int i = 0; i < respawnCount; i++)
        {
            SpawnOneBotRandom();
        }
    }

    protected void SpawnOneBotRandom()
    {
        Vector3 spawnPosition = Vector3.zero;
        for (int i = 0; i < 20; i++)
        {
            spawnPosition = Helper.GetRandomPointOnNavMesh();
            if (!IsNearAnyCharacter(spawnPosition) && !IsInCameraView(spawnPosition))
            {
                break;
            }
        }
        Bot bot = SimplePool.Spawn<Bot>(PoolType.Enemy, spawnPosition, Quaternion.identity, null);
        listBotActive.Add(bot);
        bot.OnInit();
    }

    private bool IsInCameraView(Vector3 position, float margin = 0.1f)
    {
        Camera mainCam = CameraFollow.Instance.GetMainCam();
        if (mainCam == null) return false;

        Vector3 viewportPoint = mainCam.WorldToViewportPoint(position);
        bool inX = viewportPoint.x >= -margin && viewportPoint.x <= (1f + margin);
        bool inY = viewportPoint.y >= -margin && viewportPoint.y <= (1f + margin);
        bool inZ = viewportPoint.z > 0;

        return inZ && inX && inY;
    }

    public void DeSpawnBot(Bot bot)
    {
        listBotActive.Remove(bot);
        totalBotCount = Mathf.Max(0, totalBotCount - 1);
        DeSpawnUnit(bot);
    }

    protected void DeSpawnUnit(Bot bot)
    {
        SimplePool.DeSpawn(bot);
        bot.UnregisterTarget();
    }

    public void ClearBots()
    {
        for (int i = listBotActive.Count - 1; i >= 0; i--)
        {
            Bot bot = listBotActive[i];
            if (bot != null)
            {
                DeSpawnUnit(bot);
            }
        }
        listBotActive.Clear();
    }

    private bool IsNearAnyCharacter(Vector3 position)
    {
        Collider[] hits = Physics.OverlapSphere(position, 5f, characterLayer);
        for (int i = 0; i < hits.Length; i++)
        {
            Character character = CacheComponent<Collider, Character>.Get(hits[i]);
            if (character != null)
            {
                return true;
            }
        }

        return false;
    }
    public int BotActiveCount()
    {
        // return (totalBotCount > currentBotActiveInMapCount) ? totalBotCount : currentBotActiveInMapCount;
        return (currentBotActiveInMapCount < inMapBotCount) ? currentBotActiveInMapCount : totalBotCount;
    }
}
