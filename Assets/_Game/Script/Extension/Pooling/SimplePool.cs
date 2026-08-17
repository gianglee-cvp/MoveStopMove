using System.Collections.Generic;
using UnityEngine;

public static class SimplePool 
{
    public static Dictionary<PoolType,Pool> poolInstance = new Dictionary<PoolType, Pool>(); 
    public static Dictionary<System.Type , Transform> poolParent = new Dictionary<System.Type, Transform>();
    public static void RegisterParent(System.Type type, Transform parent)
    {
        if (type == null || parent == null) return;
        poolParent[type] = parent;
    }
    public static Transform GetRegisteredParent(GameUnit unit)
    {
        if (unit == null) return null;
        poolParent.TryGetValue(unit.GetType(), out Transform parent);
        return parent;
    }
    public static bool IsPreloaded(PoolType poolType)
    {
        return poolInstance.ContainsKey(poolType) && poolInstance[poolType] != null;
    }
    public static void Preload(GameUnit prefab , int amount, Transform parent)
    {
        if(prefab == null)
        {
            Debug.LogError("PREFAB IS EMPTY");
            return; 
        }
        if(parent == null)
        {
            parent = GetRegisteredParent(prefab);
        }
        if(!poolInstance.ContainsKey(prefab.poolType) || poolInstance[prefab.poolType] == null)
        {
            Pool p = new Pool(); 
            p.Preload(prefab, amount, parent); 
            poolInstance[prefab.poolType] = p ;
        }
    }
    public static T Spawn<T>(PoolType poolType , Vector3 pos , Quaternion rot , Transform parent) where T : GameUnit
    {
        if (!poolInstance.ContainsKey(poolType))
        {
            Debug.LogError(poolType + " Is not preload");
            return null;
        }
        return poolInstance[poolType].Spawn(pos, rot, parent) as T ;
    }
    public static void DeSpawn(GameUnit unit)
    {
        if (!poolInstance.ContainsKey(unit.poolType))
        {
            Debug.LogError(unit.poolType + "Is not preload");
        }
        poolInstance[unit.poolType].DeSpawn(unit); 
    }
    public static void Collect(PoolType poolType)
    {
        if (!poolInstance.ContainsKey(poolType))
        {
            Debug.LogError(poolType + "Is not preload");
        }
        poolInstance[poolType].Collect(); 
    }
    public static void CollectAll()
    {
        foreach(var item in poolInstance.Values)
        {
            item.Collect(); 
        }
    }
    public static void Release(PoolType poolType)
    {
        poolInstance[poolType].Release(); 
    }
    public static void ReleaseAll()
    {
        foreach(var item in poolInstance.Values)
        {
            item.Release(); 
        }
    }

}


public class Pool
{
    Transform parentPool; 
    GameUnit prefab; 

    Queue<GameUnit> inactive = new  Queue<GameUnit>();
    List<GameUnit> active = new  List<GameUnit>();

    public void Preload(GameUnit prefab, int amount , Transform parent)
    {
        this.parentPool = parent; 
        this.prefab = prefab; 
        for(int i= 0 ; i < amount ; i++)
        {
            DeSpawn(GameObject.Instantiate(prefab, parent));
        }
    }
    // lay phan tu trong pool 
    public GameUnit Spawn(Vector3 pos , Quaternion rot , Transform parent)
    {
        GameUnit unit; 
        if(inactive.Count <=0)
        {
            unit = GameObject.Instantiate(prefab);
        }
        else
        {
            unit = inactive.Dequeue(); 
        }
        unit.TF.SetPositionAndRotation(pos,rot);
        unit.TF.SetParent(parent);
        active.Add(unit);
        unit.gameObject.SetActive(true);


        return unit;
    }
    // tra phan tu ve pool 
    public void DeSpawn(GameUnit unit)
    {
        if (unit != null && unit.gameObject.activeSelf)
        {
            active.Remove(unit); 
            inactive.Enqueue(unit);
            unit.TF.SetParent(parentPool); 
            unit.gameObject.SetActive(false);

        }
    }
    // thu thap tat ca phan tu ve pool 
    public void Collect()
    {
        while(active.Count > 0)
        {
            DeSpawn(active[0]);
        }
    }
    // destroy tat ca phan tu 
    public void Release()
    {
        Collect(); 
        while(inactive.Count > 0)
        {
            GameObject.Destroy(inactive.Dequeue().gameObject);
        }
        inactive.Clear();
    }
    public Queue<GameUnit> GetInactive()
    {
        return inactive; 
    }
}
