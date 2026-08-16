using System;
using System.Collections.Generic;
using UnityEngine;
public enum SkinType
{
    skinColor = 0, 
    Pant = 1,
    Hair = 2,
    Weapon = 3
}

public enum ColorType
{
    White = 0,
    Blue = 1,
    Red = 2,
    Yellow = 3,
    Green = 4, 
    Black = 5,
}
public enum PantType
{
    None =0,
    Batman = 1,
    chambi = 2,
    comy = 3,
    dabao = 4,
    onion = 5,
    pokemon = 6,
    rainbow = 7,
    skull = 8,
    vantim = 9,
}
public enum HairType
{
    None = 0,
    Arrow = 1,
    Crown = 2,
    Ear = 3,
    Flower = 4,
    Hair = 5,
    Hat = 6,
    Hat_Cap = 7,
    Horn = 8,
    Rau = 9 
}
public enum WeaponType
{
    Arrow = 0,
    Axe_0 = 1,
    Axe_1 = 2,
    Boomerang = 3,
    Candy_0 = 4,
    Candy_1 = 5,
    Candy_2 = 6,
    Candy_4 = 7,
    Hammer = 8,
    Knife = 9,
    Uzi = 10,
    Z = 11
}
[CreateAssetMenu(fileName = "SOSkin", menuName = "Game/SOSkin")]
public class SOSkin : ScriptableObject
{
    [SerializeField] private Material[] Color;
    [SerializeField] private Texture2D[] listPant;
    [SerializeField] private Hair[] listHair;
    [SerializeField] private WeaponBase[] listWeapon;
    [SerializeField] private BulletBase[] listBullet;
    [SerializeField] private SOSkinName listName;

    public Material GetMaterial(ColorType color)
    {
        return Color[(int)color];
    }
    public Texture2D GetPant(PantType type)
    {
        return listPant[(int)type];
    }
    public Hair GetHair(HairType type)
    {
        return listHair[(int)type];
    }
    public WeaponBase GetWeapon(WeaponType type)
    {
        return listWeapon[(int)type];
    }
    public BulletBase GetBullet(WeaponType type)
    {
        return listBullet[(int)type];
    }
    public string GetName(SkinType type , int index)
    {
        return listName.GetName(type,index);
    }
}
[CreateAssetMenu(fileName = "SOSkinName", menuName = "Game/SOSkinName")]
public class SOSkinName : ScriptableObject
{
    [SerializeField] private string[] colorName;
    [SerializeField] private string[] pantName;
    [SerializeField] private string[] hairName;
    [SerializeField] private List<string[]> ListString;
    public string GetName(SkinType type , int index)
    {
        return ListString[(int)type][index];
    }
}

