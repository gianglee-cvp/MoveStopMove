using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField] SingleLevelData[] datas;
    public SingleLevelData GetLevel(int index)
    {
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