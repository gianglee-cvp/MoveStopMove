using UnityEngine;
using System.Collections.Generic;

public class BotManager : Singleton<BotManager>
{
    [SerializeField] protected float spawnRadius = 20f;
    [SerializeField] protected float respawnCheckInterval;
    [SerializeField] protected int totalBotCount;
    [SerializeField] protected int inMapBotCount;
    protected int currentBotCount => listBotActive.Count;
    protected List<Bot> listBotActive = new List<Bot>();
    protected float respawnTimer;

    void Update()
    {
        if (currentBotCount >= inMapBotCount || totalBotCount <= currentBotCount)
        {
            return;
        }
        if(totalBotCount <= currentBotCount) return;
        SpawnOneBotRandom();
        respawnTimer += Time.deltaTime;
        if (respawnTimer >= respawnCheckInterval)
        {
            respawnTimer = 0f;
        }

        respawnTimer = 0f;
        // RespawnMissingBots();
        
    }

    public void Init()
    {
        respawnTimer = 0f;
        ClearBots();
        SpawnBot(inMapBotCount);
    }
    public void ApplyLevelData(SingleLevelData data)
    {
        if (data == null)
        {
            return;
        }
        totalBotCount = Mathf.Max(0, data.totalBotCount);
        inMapBotCount = Mathf.Clamp(data.inMapBotCount, 0, totalBotCount);
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
        int respawnCount = Mathf.Min(inMapBotCount - currentBotCount, totalBotCount - currentBotCount);
        for (int i = 0; i < respawnCount; i++)
        {
            SpawnOneBotRandom();
        }
    }

    protected void SpawnOneBotRandom()
    {
        // Vector3 spawnPosition = Helper.GetRandomSpawnPosition(spawnRadius);
        Vector3 spawnPosition = Helper.GetRandomPointOnNavMesh();
        Bot bot = SimplePool.Spawn<Bot>(PoolType.Enemy, spawnPosition, Quaternion.identity, null);

        listBotActive.Add(bot);
        bot.OnInit();
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
}
