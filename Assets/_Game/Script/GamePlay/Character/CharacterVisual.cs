using System;
using UnityEngine;
[Serializable]
public class Skin
{
    public ColorType color;
    public PantType pant;
    public HairType hairType;
    public WeaponType weapon;
    public Skin(ColorType color , PantType pant , HairType hair , WeaponType weapon)
    {
        this.color = color;
        this.pant = pant;
        this.hairType = hair;
        this.weapon = weapon;
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
    [SerializeField] protected Transform shootPoint;

    [SerializeField] protected Character character;
    protected PoolType currentBulletPoolType;
    protected PoolType currentWeaponPoolType;
    [SerializeField] protected BulletBase currentBullet;
    [SerializeField] protected WeaponBase currentWeapon;
    [SerializeField] protected Hair currentHair;

    public void OnInit()
    {
    }
    public void ApplyNewSkin(Skin skin)
    {
        currentSkin = skin;
        ApplySkin();
    }
    public void ApplyRandomSkin()
    {
        //TODO làm generic trong helper hàm enum không hard code cho max nữa, dùng length
        ApplyNewSkin(new Skin(
            (ColorType)UnityEngine.Random.Range(0,(int)ColorType.Black),
            (PantType)UnityEngine.Random.Range(0, (int)PantType.vantim),
            (HairType)UnityEngine.Random.Range(0, 0),
            (WeaponType)UnityEngine.Random.Range(0, (int)WeaponType.Z)
        ));
    }
    public void ApplySkin()
    {
        ChangeColorType(currentSkin.color);
        ChangePantType(currentSkin.pant);
        ChangeHairType(currentSkin.hairType);
        ChangeWeaponType(currentSkin.weapon);
        ChangeBulletType(currentSkin.weapon);
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
        if (currentSkin == null)
        {
            Debug.LogError($"[{name}] ChangeHairType failed: currentSkin is null", this);
            return;
        }

        if (DataManager.Instance == null)
        {
            Debug.LogError($"[{name}] ChangeHairType failed: DataManager.Instance is null", this);
            return;
        }

        //TODO Despawn cai cu 
        if(currentHair != null)
        {
            SimplePool.DeSpawn(currentHair);
        }

        Hair prefab = DataManager.Instance.GetHair(newHair);
        if (prefab == null)
        {
            Debug.LogError($"[{name}] ChangeHairType failed: prefab is null. newHair={newHair}, currentSkinHair={currentSkin.hairType}", this);
            return;
        }

        PoolType expectedPoolType = (PoolType)((int)PoolType.Hair_0 + (int)newHair);
        Debug.Log($"[{name}] ChangeHairType start. newHair={newHair}, currentSkinHair={currentSkin.hairType}, prefab={prefab.name}, prefabPoolType={prefab.poolType}, expectedPoolType={expectedPoolType}", this);

        if (!SimplePool.IsPreloaded(prefab.poolType))
        {
            Debug.LogWarning($"[{name}] Preloading hair prefab. prefab={prefab.name}, prefabPoolType={prefab.poolType}, expectedPoolType={expectedPoolType}", this);
            SimplePool.Preload(prefab,3,null);
        }

        Debug.Log($"[{name}] Hair preload state. prefabPoolPreloaded={SimplePool.IsPreloaded(prefab.poolType)}, expectedPoolPreloaded={SimplePool.IsPreloaded(expectedPoolType)}", this);
        if (prefab.poolType != expectedPoolType)
        {
            Debug.LogError($"[{name}] Hair pool mismatch. prefab={prefab.name}, prefabPoolType={prefab.poolType}, expectedPoolType={expectedPoolType}, newHair={newHair}", this);
        }

        PoolType type = expectedPoolType;
        currentHair = SimplePool.Spawn<Hair>(type, headTF.position, Quaternion.identity, headTF);
        if (currentHair == null)
        {
            Debug.LogError($"[{name}] Hair spawn failed. requestedPoolType={type}, prefab={prefab.name}, prefabPoolType={prefab.poolType}, headTFNull={headTF == null}", this);
            return;
        }

        Debug.Log($"[{name}] Hair spawned successfully. spawnedName={currentHair.name}, requestedPoolType={type}", this);
        currentHair.OnInit();

    }   
    public void ChangeWeaponType(WeaponType newWeapon)
    {
        if(currentWeapon != null)
        {
            SimplePool.DeSpawn(currentWeapon);
        }
        CheckBulletAndWeaponPreloaded();
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
