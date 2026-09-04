using UnityEngine;

[CreateAssetMenu(fileName = "RangeBooster",menuName = "Game/Booster/Booster Range")]
public class RangeBooster : BoosterData
{
    public override void Apply(Character ch, float itemValue)
    {
        if (ch == null) return;
        ch.ApplyRangeBooster(itemValue);
    }
}
