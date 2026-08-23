using UnityEngine;

public class Constant
{
    public const float THROW_DELAY_TIME = 0.24f;
    public const float ATTACK_RECOVERY_TIME = 0.4f;
    public const float MOVE_SPEED_DEFAULT = 5f;

    public const float RANGE_DEFAULT = 6f; 
    public const float RANGE_MAX = 12f;

    // Character Physic Collider
    public const float CH_PHYSIC_COLLIDER_RADIUS = 0.5f;
    public const float CH_PHYSIC_COLLIDER_HEIGHT = 2.82f;
    public static readonly Vector3 CH_PHYSIC_COLLIDER_CENTER = new Vector3(0f, 1f, 0f);

}
public enum SkinType
{
    skinColor = 0, 
    Pant = 1,
    Hair = 2,
    Weapon = 3,
    Shield = 4
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
public enum ShieldType
{
    None = 0,
    Shield_1 = 1,
    Shield_2 = 2
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
