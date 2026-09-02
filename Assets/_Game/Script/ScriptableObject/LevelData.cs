using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField] SingleLevelData[] datas;

    public int Count => datas == null ? 0 : datas.Length;

    public SingleLevelData GetLevel(int index)
    {
        if (datas == null || index < 0 || index >= datas.Length)
        {
            return null;
        }

        return datas[index];
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
