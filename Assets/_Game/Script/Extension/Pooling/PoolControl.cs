using UnityEngine;

public class PoolControl : MonoBehaviour
{
    [SerializeField] PoolAmount[] poolAmounts;
    public void OnInit()
    {
        //TODO them du type knight , boomerang , ... 
        for(int i =0 ; i < poolAmounts.Length ; i++)
        {
            GameUnit prefab = poolAmounts[i].prefab;
            Transform parent = poolAmounts[i].parent;

            SimplePool.Preload(prefab , poolAmounts[i].amount , parent);
            SimplePool.RegisterParent(prefab.GetType(),parent);
            Debug.Log("Preload " + prefab.name + " Type : " + prefab.GetType() + " Amount: " + poolAmounts[i].amount);   
        }
        GameUnit[] units = Resources.LoadAll<GameUnit>("Pool/");
        foreach(GameUnit unit in units)
        {
            SimplePool.Preload(unit , 1 , null);
            Debug.Log(unit.name);
        }

    }
}
[System.Serializable]
public class PoolAmount
{
    public GameUnit prefab; 
    public int amount; 
    public Transform parent;

}
public enum PoolType
{
    Player = 0,
    Enemy = 1,
    Weapon_0 = 100,
    Weapon_1 = 101,
    Weapon_2 = 102,
    Weapon_3 = 103,
    Weapon_4 = 104,
    Weapon_5 = 105,
    Weapon_6 = 106,
    Weapon_7 = 107,
    Weapon_8 = 108,
    Weapon_9 = 109,
    Weapon_10 = 110,
    Weapon_11 = 111,
    Bullet_0 = 200,
    Bullet_1 = 201,
    Bullet_2 = 202,
    Bullet_3 = 203,
    Bullet_4 = 204,
    Bullet_5 = 205,
    Bullet_6 = 206,
    Bullet_7 = 207,
    Bullet_8 = 208,
    Bullet_9 = 209,
    Bullet_10 = 210,
    Bullet_11 = 211,

    Hair_0 = 300,
    Hair_1 = 301,
    Hair_2 = 302,
    Hair_3 = 303,
    Hair_4 = 304,
    Hair_5 = 305,
    Hair_6 = 306,
    Hair_7 = 307,
    Hair_8 = 308,
    Hair_9 = 309,
    Hair_10 = 310,
    ParticleEffect = 400

}
