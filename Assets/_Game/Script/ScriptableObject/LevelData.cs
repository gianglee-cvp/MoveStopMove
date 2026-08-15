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
    public int totalBotCount;
    public int inMapBotCount;
    public GameObject map;
}
