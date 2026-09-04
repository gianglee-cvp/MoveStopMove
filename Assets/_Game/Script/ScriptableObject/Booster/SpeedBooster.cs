using UnityEngine;

[CreateAssetMenu(fileName = "SpeedBooster",menuName = "Game/Booster/Booster Speed")]
public class SpeedBooster : BoosterData
{
    public override void Apply(Character ch, float itemValue)
    {
        if (ch == null) return;
        ch.ApplySpeedBooster(itemValue);
    }
}
