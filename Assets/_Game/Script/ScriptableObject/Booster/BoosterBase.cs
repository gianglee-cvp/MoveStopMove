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
[CreateAssetMenu(fileName = "SpeedBooster",menuName = "Game/Booster/Booster Speed")]
public class SpeedBooster : BoosterData
{
    public override void Apply(Character ch, float itemValue)
    {
        if (ch == null) return;
        ch.ApplySpeedBooster(itemValue);
    }
}
[CreateAssetMenu(fileName = "RangeBooster",menuName = "Game/Booster/Booster Range")]
public class RangeBooster : BoosterData
{
    public override void Apply(Character ch, float itemValue)
    {
        if (ch == null) return;
        ch.ApplyRangeBooster(itemValue);
    }
}
[CreateAssetMenu(fileName = "GoldBooster",menuName = "Game/Booster/Booster Gold")]
public class GoldBooster : BoosterData
{
    public override void Apply(Character ch, float itemValue)
    {
        if (ch == null) return;
        ch.ApplyGoldBooster(itemValue);
    }
}
