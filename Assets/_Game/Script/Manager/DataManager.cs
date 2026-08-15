using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    [SerializeField] private SOSkin soSkin;

    // public Skin GetSkin(Skin source)
    // {
    //     Skin skin = source == null ? new Skin() : source.Clone();

    //     if (soSkin == null)
    //     {
    //         return skin;
    //     }

    //     skin.material = soSkin.GetMaterial(skin.color);
    //     skin.pantTexture = soSkin.GetPant(skin.pant);
    //     skin.hairPrefab = soSkin.GetHair(skin.hair);
    //     skin.weaponPrefab = soSkin.GetWeapon(skin.weapon);
    //     skin.bulletPrefab = soSkin.GetBullet(skin.weapon);
    //     return skin;
    // }
    public Material GetMaterial(ColorType color)
    {
        return soSkin.GetMaterial(color);
    }
    public Texture2D GetPant(PantType type)
    {
        return soSkin.GetPant(type);
    }
    public GameObject GetHair(HairType type)
    {
        return soSkin.GetHair(type);
    }
    public WeaponBase GetWeapon(WeaponType type)
    {
        return soSkin.GetWeapon(type);
    }
    public BulletBase GetBullet(WeaponType type)
    {
        return soSkin.GetBullet(type);
    }
    public string GetName(SkinType type , int index)
    {
        return soSkin.GetName(type,index);
    }
}
