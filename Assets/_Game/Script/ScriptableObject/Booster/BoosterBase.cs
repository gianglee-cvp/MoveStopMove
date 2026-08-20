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
    public BoosterType type;

    [Header("Value")]
    public float value;
    public abstract void Apply(Character ch);
}
[CreateAssetMenu(fileName = "SpeedBooster",menuName = "Game/Booster/Booster Speed")]
public class SpeedBooster : BoosterData
{
    public override void Apply(Character ch)
    {
    }
}
[CreateAssetMenu(fileName = "RangeBooster",menuName = "Game/Booster/Booster Range")]
public class RangeBooster : BoosterData
{
    public override void Apply(Character ch)
    {
    }
}
[CreateAssetMenu(fileName = "GoldBooster",menuName = "Game/Booster/Booster Gold")]
public class GoldBooster : BoosterData
{
    public override void Apply(Character ch)
    {
    }
}