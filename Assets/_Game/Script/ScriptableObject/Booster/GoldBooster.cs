using UnityEngine;

[CreateAssetMenu(fileName = "GoldBooster",menuName = "Game/Booster/Booster Gold")]
public class GoldBooster : BoosterData
{
    public override void Apply(Character ch, float itemValue)
    {
        if (ch == null) return;
        ch.ApplyGoldBooster(itemValue);
    }
}
