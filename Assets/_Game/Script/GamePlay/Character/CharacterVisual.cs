using System;
using UnityEngine;
[Serializable]
public class Skin
{
    public ColorType color;
    public PantType pant;
    public HairType hairType;
    public WeaponType weapon;

}
public class CharacterVisual : MonoBehaviour
{
    protected CharacterAnimType currentAnim;
    [SerializeField] protected Skin currentSkin = new Skin();
    [SerializeField] protected Renderer colorRenderer;
    [SerializeField] protected Renderer pantRenderer;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform righHandTF;
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected Character character;
    protected PoolType currentBulletPoolType;
    protected PoolType currentWeaponPoolType;
    [SerializeField] protected BulletBase currentBullet;
    [SerializeField] protected WeaponBase currentWeapon;

    public void OnInit()
    {
        LoadSkin();
        ApplySkin();
    }
    public void LoadSkin()
    {
        currentSkin.color = ColorType.Red;
        currentSkin.pant = PantType.Batman;
        currentSkin.hairType = HairType.Arrow;
        currentSkin.weapon = WeaponType.Axe_1;
        CheckBulletAndWeaponPreloaded();
    }

    public void ApplySkin()
    {
        ChangeColorType(currentSkin.color);
        ChangePantType(currentSkin.pant);
        ChangeHairType(currentSkin.hairType);
        ChangeWeaponType(currentSkin.weapon);
        ChangeBulletType(currentSkin.weapon);
        Debug.Log($"ApplySkin: {currentSkin.color} - {currentSkin.pant} - {currentSkin.hairType} - {currentSkin.weapon}");
    }
    private void CheckBulletAndWeaponPreloaded()
    {
        BulletBase bulletPrefab = DataManager.Instance.GetBullet(currentSkin.weapon);
        WeaponBase weaponPrefab = DataManager.Instance.GetWeapon(currentSkin.weapon);
        if (!SimplePool.IsPreloaded(weaponPrefab.poolType))
        {
            SimplePool.Preload(weaponPrefab,5, null);
        }
        if (!SimplePool.IsPreloaded(bulletPrefab.poolType))
        {
            SimplePool.Preload(bulletPrefab,20, null);
        }
    }
    public void ChangeColorType(ColorType newColor)
    {
        colorRenderer.sharedMaterial = DataManager.Instance.GetMaterial(newColor);
    }
    public void ChangePantType(PantType newPant)
    {
        //TODO thay thanh property block
        Texture newPantTexture = newPant == PantType.None ? null : DataManager.Instance.GetPant(newPant);
        pantRenderer.material.SetTexture("_BaseMap", newPantTexture);
    }
    public void ChangeHairType(HairType newHair)
    {
        //TODO change hair type
    }   
    public void ChangeWeaponType(WeaponType newWeapon)
    {
        if(currentWeapon != null)
        {
            SimplePool.DeSpawn(currentWeapon);
        }
        currentWeaponPoolType = (PoolType)((int)PoolType.Weapon_0 + (int)newWeapon);
        currentWeapon = SimplePool.Spawn<WeaponBase>(currentWeaponPoolType, righHandTF.position, Quaternion.identity, righHandTF);
        currentWeapon.OnInit(righHandTF);
    }
    public void ChangeBulletType(WeaponType newWeapon)
    {
        //TODO change bullet type
        currentBulletPoolType = (PoolType)((int)PoolType.Bullet_0 + (int)newWeapon);
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
    public void ChangeAcessory()
    {

    }
    public void InitThrow(Vector3 rootPos)
    {
        currentWeapon.OnDeactive();
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
        currentWeapon.Active();
    }
    //TODO xoa
    public float distance = 10f;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 start = transform.position;
        Vector3 end = start + transform.forward * distance;
        Gizmos.DrawLine(start, end);
    }

}
