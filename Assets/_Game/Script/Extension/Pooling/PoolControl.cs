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
    Bullet_0 = 200
}
