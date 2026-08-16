using System.Collections.Generic;
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
    public List<Enum_ShopState> weaponShopState;
    public List<Enum_ShopState> hairShopState;
    public List<Enum_ShopState> pantShopState;

    public GameData()
    {
        gold = 0;
        colorEquippedID = ColorType.White;
        weaponEquippedID = WeaponType.Axe_0;
        hairEquippedID = HairType.None;
        pantEquippedID = PantType.None;
        
        weaponShopState = new List<Enum_ShopState>(
            new Enum_ShopState[(int)WeaponType.Z]
        );
        hairShopState = new List<Enum_ShopState>(
            new Enum_ShopState[(int)HairType.Rau]
        );
        pantShopState = new List<Enum_ShopState>(
            new Enum_ShopState[(int)PantType.vantim]
        );
    }
    public Skin GetSkinEquipped()
    {
        return new Skin(colorEquippedID,pantEquippedID,hairEquippedID,weaponEquippedID);
    }
    public void SaveSkin(Skin skin)
    {
        colorEquippedID = skin.color;
        weaponEquippedID = skin.weapon;
        hairEquippedID = skin.hairType;
        pantEquippedID = skin.pant;
    }
}
