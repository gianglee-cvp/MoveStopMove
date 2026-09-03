using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField] SingleLevelData[] datas;
    [SerializeField] protected Vector3 playerPosition;
    [SerializeField] protected Vector3 playerRotaion;

    public int Count => datas == null ? 0 : datas.Length;

    public SingleLevelData GetLevel(int index)
    {
        if (datas == null || index < 0 || index >= datas.Length)
        {
            return null;
        }

        return datas[index];
    }
    public Vector3 GetPlayerPosition()
    {
        return playerPosition;
    }
    public Vector3 GetPlayerRotation()
    {
        return playerRotaion;
    }
}

[System.Serializable]
public class SingleLevelData
{
    [SerializeField] private int totalBotCount;
    [SerializeField] private int inMapBotCount;
    [SerializeField] private GameObject map;

    public int TotalBotCount => totalBotCount;
    public int InMapBotCount => inMapBotCount;
    public GameObject Map => map;
}
