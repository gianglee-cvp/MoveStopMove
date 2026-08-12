using System;
using UnityEditor.UIElements;
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
    public void ChangeRotation(Vector3 move)
    {
        transform.rotation = Quaternion.LookRotation(move);
    }
    public void InitThrow(Vector3 des)
    {
        currentWeapon.OnDeactive();
        BulletBase bullet = Instantiate(item.bulletPrefab);
        bullet.Init(shootPoint , character.Range , des);        
        currentBullet = bullet;
    }
    public void Throw(Vector3 des)
    {
        InitThrow(des);
        currentBullet.Throw(des);
    }
    public void ActiveWeapon()
    {
        currentWeapon.Active();
    }
    public void RotateToTarget(Vector3 des)
    {
        Vector3 direction = des - transform.position;
        direction.y = 0f;
        transform.rotation = Quaternion.LookRotation(direction);
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