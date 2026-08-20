using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SOItem", menuName = "Game/SOItem")]
public class SOItem : ScriptableObject
{
    [SerializeField] private ColorItemData[] Colors;
    [SerializeField] private PantItemData[] listPant;
    [SerializeField] private HairItemData[] listHair;
    [SerializeField] private WeaponItemData[] listWeapon;
    [SerializeField] private BoosterData pantBooster;
    [SerializeField] private BoosterData hairBooster;

    public Material GetMaterial(ColorType color)
    {
        // return Color[(int)color];
        return Colors[(int)color].material;
    }
    public Texture2D GetPant(PantType type)
    {
        return listPant[(int)type].texture;
    }
    public Hair GetHair(HairType type)
    {
        return listHair[(int)type].hairPrefab;
    }
    public WeaponBase GetWeapon(WeaponType type)
    {
        return listWeapon[(int)type].weaponPrefab;
    }
    public BulletBase GetBullet(WeaponType type)
    {
        return listWeapon[(int)type].bulletPrefab;
    }
}