using System;
using UnityEngine;
[Serializable]
public class Skin
{
    public ColorType color;
    public PantType pant;
    public HairType hairType;
    public WeaponType weapon;
    public ShieldType shieldType;
    public Skin(ColorType color , PantType pant , HairType hair , WeaponType weapon, ShieldType shieldType = ShieldType.None)
    {
        this.color = color;
        this.pant = pant;
        this.hairType = hair;
        this.weapon = weapon;
        this.shieldType = shieldType;
    }
}
public class CharacterVisual : MonoBehaviour
{
    protected CharacterAnimType currentAnim;
    [SerializeField] protected Skin currentSkin;
    public Skin CurrentSkin
    {
        get => currentSkin;
    }
    [SerializeField] protected Renderer colorRenderer;
    [SerializeField] protected Renderer pantRenderer;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform righHandTF;
    [SerializeField] protected Transform headTF;
    [SerializeField] protected Transform shieldTF;
    [SerializeField] protected Transform shootPoint;

    [SerializeField] protected Character character;
    protected PoolType currentBulletPoolType;
    protected PoolType currentWeaponPoolType;
    [SerializeField] protected BulletBase currentBullet;
    [SerializeField] protected WeaponBase currentWeapon;
    [SerializeField] protected SpawnObject currentHair;
    [SerializeField] protected SpawnObject currentShield;
    [SerializeField] protected ColorType currentColorType;
    public ColorType Color => currentColorType;
    public Transform HeadTF => headTF;
    public Vector3 HeadPos => headTF.position;
    public bool CheckWeaponActive => currentWeapon != null && currentWeapon.gameObject.activeSelf;
    
    public void OnInit()
    {
        if(currentBullet != null)
        {
            currentBullet.SetNullOwner();
            currentBullet = null;
        }
        ActiveWeapon();
    }
    public void ApplyNewSkin(Skin skin)
    {
        currentSkin = skin;
        ApplySkin();
    }
    public void ApplyRandomSkin()
    {
        ApplyNewSkin(new Skin(
            Helper.RandomEnumValue<ColorType>(),
            Helper.RandomEnumValue<PantType>(),
            Helper.RandomEnumValue<HairType>(),
            Helper.RandomEnumValue<WeaponType>(),
            Helper.RandomEnumValue<ShieldType>()
        ));
    }
    public void ApplySkin()
    {
        ResetAndApplyBoosters();
        ChangeColorType(currentSkin.color);
        ChangePantType(currentSkin.pant);
        ChangeHairType(currentSkin.hairType);
        ChangeShieldType(currentSkin.shieldType);
        ChangeWeaponType(currentSkin.weapon);
        ChangeBulletType(currentSkin.weapon);
    }
    public void ChangeColorType(ColorType newColor)
    {
        ColorItemData colorData = DataManager.Instance.GetItemData<ColorItemData>(SkinType.skinColor, (int)newColor);
        colorRenderer.sharedMaterial = colorData.Material;
        currentColorType = newColor;
    }
    public void ChangePantType(PantType newPant)
    {
        //TODO thay thanh property block
        PantItemData pantData = DataManager.Instance.GetItemData<PantItemData>(SkinType.Pant, (int)newPant);
        Texture newPantTexture = newPant == PantType.None ? null : pantData.Texture;
        pantRenderer.material.SetTexture("_BaseMap", newPantTexture);
    }
    public void ChangeHairType(HairType newHair)
    {        
        if(currentHair != null)
        {
            SimplePool.DeSpawn(currentHair);
        }
        HairItemData hairData = DataManager.Instance.GetItemData<HairItemData>(SkinType.Hair, (int)newHair);
        if (hairData == null || hairData.HairPrefab == null)
        {
            currentHair = null;
            return;
        }
        currentHair = SimplePool.Spawn<SpawnObject>(hairData.HairPrefab.poolType, headTF.position, Quaternion.identity, headTF);
        currentHair.OnInit();
    }
    public void ChangeShieldType(ShieldType newShield)
    {
        if (currentShield != null)
        {
            SimplePool.DeSpawn(currentShield);
        }

        ShieldItemData shieldData = DataManager.Instance.GetItemData<ShieldItemData>(SkinType.Shield, (int)newShield);
        if (shieldData == null || shieldData.ShieldPrefab == null)
        {
            currentShield = null;
            return;
        }
        currentShield = SimplePool.Spawn<SpawnObject>(shieldData.ShieldPrefab.poolType, shieldTF.position, Quaternion.identity, shieldTF);
        currentShield.OnInit();
    }
    public void ChangeWeaponType(WeaponType newWeapon)
    {
        if(currentWeapon != null)
        {
            SimplePool.DeSpawn(currentWeapon);
        }
        WeaponItemData weaponData = DataManager.Instance.GetItemData<WeaponItemData>(SkinType.Weapon, (int)newWeapon);
        currentWeaponPoolType = weaponData.WeaponPrefab.poolType;
        currentWeapon = SimplePool.Spawn<WeaponBase>(currentWeaponPoolType, righHandTF.position, Quaternion.identity, righHandTF);
        currentWeapon.OnInit();
    }
    public void ChangeBulletType(WeaponType newWeapon)
    {
        //TODO change bullet type
        WeaponItemData weaponData = DataManager.Instance.GetItemData<WeaponItemData>(SkinType.Weapon, (int)newWeapon);
        currentBulletPoolType = weaponData.BulletPrefab.poolType;
    }
    public void ChangeAnim(CharacterAnimType newAnim)
    {
        if(currentAnim != newAnim)
        {
            animator.SetBool(currentAnim.ToString(), false);
            currentAnim = newAnim;
            animator.SetBool(newAnim.ToString(),true);
        }
    }
    public void InitThrow(Vector3 rootPos)
    {
        if (currentWeapon != null)
        {
            currentWeapon.OnDeactive();
        }
        BulletBase bullet = SimplePool.Spawn<BulletBase>(currentBulletPoolType,shootPoint.position, shootPoint.rotation,null);
        bullet.Init(shootPoint.forward , character.Range , rootPos , character);
        currentBullet = bullet;
    }
    public void Throw(Vector3 rootPos)
    {
        InitThrow(rootPos);
        currentBullet.Throw();
    }
    public void ActiveWeapon()
    {
        if (currentWeapon == null) return;
        currentWeapon.Active();
    }
    public void OnBulletDespawn(BulletBase bullet)
    {
        if (bullet == null || currentBullet != bullet) return;

        currentBullet = null;

        if (character != null && character.isActiveAndEnabled && !character.IsDead)
        {
            ActiveWeapon();
        }
    }
    public ColorType GetCurrentColorType()
    {
        return currentColorType;
    }
    public void DespawnSkin()
    {
        currentBullet = null;
        SimplePool.DeSpawn(currentWeapon);
        currentWeapon = null;
        SimplePool.DeSpawn(currentHair);
        currentHair = null;
        SimplePool.DeSpawn(currentShield);
        currentShield = null;
    }
    // //TODO xoa
    // public float distance = 10f;
    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.green;
    //     Vector3 start = transform.position;
    //     Vector3 end = start + transform.forward * distance;
    //     Gizmos.DrawLine(start, end); 
    // }

    private void ResetAndApplyBoosters()
    {
        if (character == null) return;

        character.ResetBoosters();
        ApplyBooster(
            DataManager.Instance.GetBooster(SkinType.Pant),
            DataManager.Instance.GetItemData<PantItemData>(SkinType.Pant, (int)currentSkin.pant)
        );
        ApplyBooster(
            DataManager.Instance.GetBooster(SkinType.Hair),
            DataManager.Instance.GetItemData<HairItemData>(SkinType.Hair, (int)currentSkin.hairType)
        );
        ApplyBooster(
            DataManager.Instance.GetBooster(SkinType.Shield),
            DataManager.Instance.GetItemData<ShieldItemData>(SkinType.Shield, (int)currentSkin.shieldType)
        );
    }

    private void ApplyBooster(BoosterData boosterData, ItemData itemData)
    {
        if (boosterData == null || itemData == null || character == null) return;
        boosterData.Apply(character, itemData.Value);
    }
    
}
