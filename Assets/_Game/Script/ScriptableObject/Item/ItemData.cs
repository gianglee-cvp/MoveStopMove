using System;
using UnityEngine;

[Serializable]
public abstract class ItemData
{
    public string itemName;
    public int value;
}

[Serializable]
public class ColorItemData : ItemData
{
    public ColorType colorType;
    public Material material;
}

[Serializable]
public class PantItemData : ItemData
{
    public PantType pantType;
    public Texture2D texture;
}

[Serializable]
public class HairItemData : ItemData
{
    public HairType hairType;
    public Hair hairPrefab;
}

[Serializable]
public class WeaponItemData : ItemData
{
    public WeaponType weaponType;
    public WeaponBase weaponPrefab;
    public BulletBase bulletPrefab;
}