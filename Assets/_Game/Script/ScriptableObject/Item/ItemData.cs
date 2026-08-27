using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public abstract class ItemData
{
    [SerializeField] private string itemName;
    [SerializeField] private float value;
    [SerializeField] private Sprite icon;
    [SerializeField] private int price;
    [SerializeField] private string des;


    public string ItemName => itemName;
    public float Value => value;
    public Sprite Icon => icon;
    public int Price => price;
    public string Des => des;
    public virtual SkinType Type => SkinType.skinColor;
    public virtual int Index => 0;
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
    public override int Index => (int)pantType;
    public override SkinType Type => SkinType.Pant;

}

[Serializable]
public class HairItemData : ItemData
{
    [SerializeField] private HairType hairType;
    [SerializeField] private SpawnObject hairPrefab;


    public HairType HairType => hairType;
    public SpawnObject HairPrefab => hairPrefab;
    public override int Index => (int)hairType;
    public override SkinType Type => SkinType.Hair;
}

[Serializable]
public class ShieldItemData : ItemData
{
    [SerializeField] private ShieldType shieldType;
    [SerializeField] private SpawnObject shieldPrefab;


    public ShieldType ShieldType => shieldType;
    public SpawnObject ShieldPrefab => shieldPrefab;
    public override int Index => (int)shieldType;
    public override SkinType Type => SkinType.Shield;
    
}

[Serializable]
public class WeaponItemData : ItemData
{
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private GameObject prefabShop;
    [SerializeField] private WeaponBase weaponPrefab;
    [SerializeField] private BulletBase bulletPrefab;

    public WeaponType WeaponType => weaponType;
    public WeaponBase WeaponPrefab => weaponPrefab;
    public BulletBase BulletPrefab => bulletPrefab;
    public GameObject PrefabShop => prefabShop;
    public override int Index => (int)weaponType;
    public override SkinType Type => SkinType.Weapon;
}
