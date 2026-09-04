using UnityEngine;

public enum BoosterType
{
    MoveSpeed = 0 ,
    Gold = 1,
    Range = 2
}

[CreateAssetMenu(fileName = "BoosterData",menuName = "Game/Booster/Booster Data")]
public abstract class BoosterData : ScriptableObject
{
    [SerializeField] protected BoosterType type;

    [Header("Value")]
    [SerializeField] protected float value;

    public BoosterType Type => type;
    public float Value => value;

    public abstract void Apply(Character ch, float itemValue);
}
