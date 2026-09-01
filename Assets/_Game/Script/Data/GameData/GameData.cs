using System.Collections.Generic;
using System;
using UnityEngine;
public enum Enum_ShopState
{
    buy = 0,
    bought = 1,
    equipped = 2
}   
public interface IData
{
    void LoadGame(GameData data);
    void SaveGame(GameData gameData);
}
[System.Serializable]
// data nao dung rieng thi duoc lay luon tham chieu, bien nao dung chung thi chi duoc thay doi thong qua datamanager
public class GameData
{
    [SerializeField] protected int gold;
    [SerializeField] protected ColorType colorEquippedID;
    [SerializeField] protected WeaponType weaponEquippedID;
    [SerializeField] protected HairType hairEquippedID;
    [SerializeField] protected PantType pantEquippedID;
    [SerializeField] protected ShieldType shieldEquippedID;
    [SerializeField] protected List<Enum_ShopState> weaponShopState;
    [SerializeField] protected List<Enum_ShopState> hairShopState;
    [SerializeField] protected List<Enum_ShopState> pantShopState;
    [SerializeField] protected List<Enum_ShopState> shieldShopState;

    #region Get / Set Methods
    public int GetGold() => gold;
    public void SetGold(int value) => gold = value;

    public ColorType GetColorEquippedID() => colorEquippedID;
    public void SetColorEquippedID(ColorType value) => colorEquippedID = value;

    public WeaponType GetWeaponEquippedID() => weaponEquippedID;
    public void SetWeaponEquippedID(WeaponType value) => weaponEquippedID = value;

    public HairType GetHairEquippedID() => hairEquippedID;
    public void SetHairEquippedID(HairType value) => hairEquippedID = value;

    public PantType GetPantEquippedID() => pantEquippedID;
    public void SetPantEquippedID(PantType value) => pantEquippedID = value;

    public ShieldType GetShieldEquippedID() => shieldEquippedID;
    public void SetShieldEquippedID(ShieldType value) => shieldEquippedID = value;

    public IReadOnlyList<Enum_ShopState> GetWeaponShopState() => weaponShopState;
    public IReadOnlyList<Enum_ShopState> GetHairShopState() => hairShopState;
    public IReadOnlyList<Enum_ShopState> GetPantShopState() => pantShopState;
    public IReadOnlyList<Enum_ShopState> GetShieldShopState() => shieldShopState;
    #endregion

    private Dictionary<SkinType, Func<int>> equippedGetters;
    private Dictionary<SkinType, Action<int>> equippedSetters;
    private Dictionary<SkinType, List<Enum_ShopState>> shopStateMap;

    public GameData()
    {
        gold = 0;
        colorEquippedID = ColorType.White;
        weaponEquippedID = WeaponType.Arrow;
        hairEquippedID = HairType.None;
        pantEquippedID = PantType.None;
        shieldEquippedID = ShieldType.None;
        
        weaponShopState = CreateShopStateList(weaponEquippedID);
        hairShopState = CreateShopStateList(hairEquippedID);
        pantShopState = CreateShopStateList(pantEquippedID);
        shieldShopState = CreateShopStateList(shieldEquippedID);
    }

    private List<Enum_ShopState> CreateShopStateList<T>(T equippedItem) where T : Enum
    {
        var list = new List<Enum_ShopState>(new Enum_ShopState[Enum.GetValues(typeof(T)).Length]);
        int equippedIndex = Convert.ToInt32(equippedItem);
        if (equippedIndex >= 0 && equippedIndex < list.Count)
        {
            list[equippedIndex] = Enum_ShopState.equipped;
        }
        return list;
    }

    private void InitDictionaries()
    {
        if (equippedGetters != null) return;

        equippedGetters = new Dictionary<SkinType, Func<int>>
        {
            { SkinType.skinColor, () => (int)colorEquippedID },
            { SkinType.Weapon, () => (int)weaponEquippedID },
            { SkinType.Hair, () => (int)hairEquippedID },
            { SkinType.Pant, () => (int)pantEquippedID },
            { SkinType.Shield, () => (int)shieldEquippedID }
        };

        equippedSetters = new Dictionary<SkinType, Action<int>>
        {
            { SkinType.skinColor, index => colorEquippedID = (ColorType)index },
            { SkinType.Weapon, index => weaponEquippedID = (WeaponType)index },
            { SkinType.Hair, index => hairEquippedID = (HairType)index },
            { SkinType.Pant, index => pantEquippedID = (PantType)index },
            { SkinType.Shield, index => shieldEquippedID = (ShieldType)index }
        };

        shopStateMap = new Dictionary<SkinType, List<Enum_ShopState>>
        {
            { SkinType.Weapon, weaponShopState },
            { SkinType.Hair, hairShopState },
            { SkinType.Pant, pantShopState },
            { SkinType.Shield, shieldShopState }
        };
    }

    public Skin GetSkinEquipped()
    {
        return new Skin(colorEquippedID, pantEquippedID, hairEquippedID, weaponEquippedID, shieldEquippedID);
    }

    public void SaveSkin(Skin skin)
    {
        colorEquippedID = skin.color;
        weaponEquippedID = skin.weapon;
        hairEquippedID = skin.hairType;
        pantEquippedID = skin.pant;
        shieldEquippedID = skin.shieldType;
    }

    public int GetEquippedID(SkinType type)
    {
        InitDictionaries();
        return equippedGetters.TryGetValue(type, out var getter) ? getter() : -1;
    }

    public T GetEquippedID<T>(SkinType type) where T : Enum
    {
        return (T)Enum.ToObject(typeof(T), GetEquippedID(type));
    }

    public void SetEquippedID(SkinType type, int index)
    {
        InitDictionaries();
        if (equippedSetters.TryGetValue(type, out var setter))
        {
            setter(index);
        }
    }

    public void SetEquippedID<T>(SkinType type, T enumValue) where T : Enum
    {
        SetEquippedID(type, Convert.ToInt32(enumValue));
    }

    public IReadOnlyList<Enum_ShopState> GetShopStateByType(SkinType type)
    {
        InitDictionaries();
        return shopStateMap.TryGetValue(type, out var list) ? list : null;
    }

    public void SetShopState(SkinType type, int index, Enum_ShopState state)
    {
        InitDictionaries();
        if (shopStateMap.TryGetValue(type, out var list) && index >= 0 && index < list.Count)
        {
            list[index] = state;
        }
    }

    public void EnsureShopStateCount(SkinType type, int requiredCount, Enum_ShopState defaultState = Enum_ShopState.buy)
    {
        InitDictionaries();
        if (shopStateMap.TryGetValue(type, out var list))
        {
            while (list.Count < requiredCount)
            {
                list.Add(defaultState);
            }
        }
    }
}
