using UnityEngine;
using System.Collections.Generic;
public class BotManager : Singleton<BotManager>
{
    //TODO load data tu scriptable object
    protected int totalBotCount = 20;
    protected int inMapBotCount = 5;
    protected int currentBotCount => listBotActive.Count;
    protected List<GameUnit> listBotActive = new List<GameUnit>();
    //TODO cho vao init level manager
    public void Init()
    {
        SpawnBot(inMapBotCount);
        for(int i = 0 ; i < inMapBotCount ; i++)
        {
            Bot bot = listBotActive[i] as Bot;
            bot.OnInit();
        }
    }
    public void SpawnBot(int cnt)
    {
        //TODO sua lai logic random 
        
        for(int i = 0 ; i < cnt ; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-100f,100f),0,Random.Range(-100f,100f));
            GameUnit bot = SimplePool.Spawn<Bot>(PoolType.Enemy, pos, Quaternion.identity,null);
            listBotActive.Add(bot);
        }
    }
    public void DeSpawnBot(GameUnit bot)
    {
        listBotActive.Remove(bot);
        SimplePool.DeSpawn(bot);
    }
}