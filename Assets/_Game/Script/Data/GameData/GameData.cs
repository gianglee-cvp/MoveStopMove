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
    void SaveGame(ref GameData gameData);
}
[System.Serializable]
public class GameData
{
    public int gold;
    public ColorType colorEquippedID;
    public WeaponType weaponEquippedID;
    public HairType hairEquippedID;
    public PantType pantEquippedID;
    public ShieldType shieldEquippedID;
    public List<Enum_ShopState> weaponShopState;
    public List<Enum_ShopState> hairShopState;
    public List<Enum_ShopState> pantShopState;
    public List<Enum_ShopState> shieldShopState;

    public GameData()
    {
        gold = 0;
        colorEquippedID = ColorType.White;
        weaponEquippedID = WeaponType.Axe_0;
        hairEquippedID = HairType.None;
        pantEquippedID = PantType.None;
        shieldEquippedID = ShieldType.None;
        
        weaponShopState = new List<Enum_ShopState>(
            new Enum_ShopState[Enum.GetValues(typeof(WeaponType)).Length]
        );
        hairShopState = new List<Enum_ShopState>(
            new Enum_ShopState[Enum.GetValues(typeof(HairType)).Length]
        );
        pantShopState = new List<Enum_ShopState>(
            new Enum_ShopState[Enum.GetValues(typeof(PantType)).Length]
        );
        shieldShopState = new List<Enum_ShopState>(
            new Enum_ShopState[Enum.GetValues(typeof(ShieldType)).Length]
        );
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
    //TODO refactor switch case
    public int GetEquippedID(SkinType type)
    {
        switch (type)
        {
            case SkinType.skinColor:
                return (int)colorEquippedID;
            case SkinType.Weapon:
                return (int)weaponEquippedID;
            case SkinType.Hair:
                return (int)hairEquippedID;
            case SkinType.Pant:
                return (int)pantEquippedID;
            case SkinType.Shield:
                return (int)shieldEquippedID;
            default:
                return -1;
        }
    }
    public void SetEquippedID(SkinType type, int index)
    {
        switch (type)
        {
            case SkinType.skinColor:
                colorEquippedID = (ColorType)index;
                break;
            case SkinType.Weapon:
                weaponEquippedID = (WeaponType)index;
                break;
            case SkinType.Hair:
                hairEquippedID = (HairType)index;
                break;
            case SkinType.Pant:
                pantEquippedID = (PantType)index;
                break;
            case SkinType.Shield:
                shieldEquippedID = (ShieldType)index;
                break;
        }
    }
    public List<Enum_ShopState> GetShopStateByType(SkinType type)
    {
        switch (type)
        {
            case SkinType.Weapon:
                return weaponShopState;
            case SkinType.Hair:
                return hairShopState;
            case SkinType.Pant:
                return pantShopState;
            case SkinType.Shield:
                return shieldShopState;
            default:
                return null;
        }
    }
}
