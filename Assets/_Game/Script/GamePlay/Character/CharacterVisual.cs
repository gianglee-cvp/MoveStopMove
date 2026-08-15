using System;
using UnityEngine;
[Serializable]
public struct baseItem
{
    public WeaponBase weaponPrefab;
    public BulletBase bulletPrefab;
}
public class CharacterVisual : MonoBehaviour
{
    protected CharacterAnimType currentAnim;
    [SerializeField] protected baseItem item;
    [SerializeField] protected Animator animator; 
    [SerializeField] protected Transform righHandTF;    
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected Character character;
    [SerializeField] protected BulletBase currentBullet;
    [SerializeField] protected WeaponBase currentWeapon;

    public void OnInit()
    {
        if(currentWeapon == null)
        {
            // currentWeapon = Instantiate(item.weaponPrefab);
            currentWeapon = SimplePool.Spawn<WeaponBase>(
                PoolType.Weapon_0,
                righHandTF.position,
                Quaternion.identity,
                righHandTF
            );
        }
        currentWeapon.OnInit(righHandTF);
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
        // BulletBase bullet = Instantiate(item.bulletPrefab);
        BulletBase bullet = SimplePool.Spawn<BulletBase>(PoolType.Bullet_0,shootPoint.position, shootPoint.rotation,null);
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