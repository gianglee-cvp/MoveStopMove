using System;
using UnityEngine;

[Serializable]
public abstract class ItemData
{
    [SerializeField] private string itemName;
    [SerializeField] private float value;

    public string ItemName => itemName;
    public float Value => value;
}

[Serializable]
public class ColorItemData : ItemData
{
    [SerializeField] private ColorType colorType;
    [SerializeField] private Material material;

    public ColorType ColorType => colorType;
    public Material Material => material;
}

[Serializable]
public class PantItemData : ItemData
{
    [SerializeField] private PantType pantType;
    [SerializeField] private Texture2D texture;

    public PantType PantType => pantType;
    public Texture2D Texture => texture;
}

[Serializable]
public class HairItemData : ItemData
{
    [SerializeField] private HairType hairType;
    [SerializeField] private Hair hairPrefab;

    public HairType HairType => hairType;
    public Hair HairPrefab => hairPrefab;
}

[Serializable]
public class WeaponItemData : ItemData
{
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private WeaponBase weaponPrefab;
    [SerializeField] private BulletBase bulletPrefab;

    public WeaponType WeaponType => weaponType;
    public WeaponBase WeaponPrefab => weaponPrefab;
    public BulletBase BulletPrefab => bulletPrefab;
}
