using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SOItem", menuName = "Game/SOItem")]
public class SOItem : ScriptableObject
{
    [SerializeField] private ColorItemData[] Colors;
    [SerializeField] private PantItemData[] listPant;
    [SerializeField] private HairItemData[] listHair;
    [SerializeField] private ShieldItemData[] listShield;
    [SerializeField] private WeaponItemData[] listWeapon;
    [SerializeField] private BoosterData colorBooster;
    [SerializeField] private BoosterData pantBooster;
    [SerializeField] private BoosterData hairBooster;
    [SerializeField] private BoosterData shieldBooster;
    [SerializeField] private BoosterData weaponBooster;
    private Dictionary<SkinType, ItemData[]> itemMap;
    private Dictionary<SkinType, BoosterData> boosterMap;

    private void OnEnable()
    {
        OnInit();
    }

    public void OnInit()
    {
        itemMap = new Dictionary<SkinType, ItemData[]>
        {
            { SkinType.skinColor, Colors },
            { SkinType.Pant, listPant },
            { SkinType.Hair, listHair },
            { SkinType.Shield, listShield },
            { SkinType.Weapon, listWeapon }
        };

        boosterMap = new Dictionary<SkinType, BoosterData>
        {
            { SkinType.skinColor, colorBooster },
            { SkinType.Pant, pantBooster },
            { SkinType.Hair, hairBooster },
            { SkinType.Shield, shieldBooster },
            { SkinType.Weapon, weaponBooster }
        };
    }

    public T GetData<T>(SkinType skinType, int index) where T : ItemData
    {
        if (itemMap == null || !itemMap.TryGetValue(skinType, out ItemData[] items))
        {
            return null;
        }

        if (index < 0 || index >= items.Length)
        {
            return null;
        }

        return items[index] as T;
    }

    public BoosterData GetBooster(SkinType skinType)
    {
        if (boosterMap == null || !boosterMap.TryGetValue(skinType, out BoosterData booster))
        {
            return null;
        }

        return booster;
    }
    public ItemData[] GetListData(SkinType type)
    {
        if (itemMap == null || !itemMap.TryGetValue(type, out ItemData[] items))
        {
            return null;
        }
        return items;
    }
}
