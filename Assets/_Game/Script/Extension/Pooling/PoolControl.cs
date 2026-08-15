using UnityEngine;

public class PoolControl : MonoBehaviour
{
    [SerializeField] PoolAmount[] poolAmounts;
    public void OnInit()
    {
        GameUnit[] gameUnits = Resources.LoadAll<GameUnit>("Pool/");
        for(int i =0 ; i < poolAmounts.Length ; i++)
        {
            SimplePool.Preload(poolAmounts[i].prefab , poolAmounts[i].amount , poolAmounts[i].parent);
            Debug.Log("Preload " + poolAmounts[i].prefab.name + " Amount: " + poolAmounts[i].amount);   
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
}
