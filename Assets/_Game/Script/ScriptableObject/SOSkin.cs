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
[CreateAssetMenu(fileName = "SOSkin", menuName = "Game/SOSkin")]
public class SOSkin : ScriptableObject
{
    [SerializeField] private Material[] Color;
    [SerializeField] private Texture2D[] listPant;
    [SerializeField] private GameObject[] listHair;
    [SerializeField] private WeaponBase[] listWeapon;
    [SerializeField] private SOSkinName listName;

    public Material GetMaterial(ColorType color)
    {
        return Color[(int)color];
    }
    public Texture2D GetPant(PantType type)
    {
        return listPant[(int)type];
    }
    public GameObject GetHair(HairType type)
    {
        return listHair[(int)type];
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

