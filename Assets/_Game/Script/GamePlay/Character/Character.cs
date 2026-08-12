using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public enum CharacterAnimType
{
    Idle = 0, 
    Run = 1,
    Attack = 2,
    Win = 3,
    Dance = 4,
    Dead = 5,
    Ulti =6
}

public class Character : MonoBehaviour
{
    [SerializeField] protected Transform tf; 
    [SerializeField] protected CharacterVisual characterVisual;


    [SerializeField] protected bool isMoving = false;
    [SerializeField] protected bool isAttacking = false;
    [SerializeField] protected bool isAttackable = true;

    protected Coroutine attackCO;
    [SerializeField] protected float range;
    public float Range
    {
        get => range;
    }
    //TODO attack theo range khong phai bool 
    public bool attackpress => Keyboard.current.spaceKey.wasPressedThisFrame; 
    public virtual void OnInit()
    {
        characterVisual.OnInit();
        ChangeAnim(CharacterAnimType.Idle);
    }
    public virtual void Attack()
    {
        isAttacking  = true;
        isAttackable = false;
        isMoving = false;
        ChangeAnim(CharacterAnimType.Attack);
        attackCO = StartCoroutine(Throw(tf.position)); 
    }
    protected IEnumerator Throw(Vector3 pos)
    {
        //TODO cho vao constant
        yield return new WaitForSeconds(0.24f);
        if (isAttacking)
        {
            characterVisual.Throw(pos);
        }
        isAttacking = false;
        //todo cho vao constant
        yield return new WaitForSeconds(0.4f);
        attackCO = null;
        Idle();
    }
    public virtual void CancelAttack()
    {
        if (attackCO != null)
        {
            StopCoroutine(attackCO);
            attackCO = null;
        }
        Idle();
    }
    public virtual void Idle()
    {
        ChangeAnim(CharacterAnimType.Idle);
        characterVisual.ActiveWeapon();
        isAttacking = false;
        isAttackable  = true;
        isMoving = false;
    }
    public virtual void ChangeAnim(CharacterAnimType type)
    {
        characterVisual.ChangeAnim(type);
    }
}
