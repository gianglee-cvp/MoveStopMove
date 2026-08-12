using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
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
    public Vector3 pos
    {
        get => tf.position;
    }
    [SerializeField] protected CharacterVisual characterVisual;


    [SerializeField] protected bool isMoving = false;
    [SerializeField] protected bool isAttacking = false;
    [SerializeField] protected bool isAttackable = true;

    protected Coroutine attackCO;
    [SerializeField] protected float range;
    protected float offsetRange = 0f;
    //TODO đổi thành protected
    public List<Character> listTarget = new List<Character>();
    protected Character currentTarget;
    public float Range
    {
        get => range;
    }
    //TODO attack theo range khong phai bool 
    public bool attackpress => Keyboard.current.spaceKey.wasPressedThisFrame; 
    public virtual void OnInit()
    {
        characterVisual.OnInit();
        listTarget.Clear();
        currentTarget = null;
        ChangeAnim(CharacterAnimType.Idle);
    }
    public virtual void Idle()
    {
        ChangeAnim(CharacterAnimType.Idle);
        characterVisual.ActiveWeapon();
        isAttacking = false;
        isAttackable  = true;
        isMoving = false;
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

    public virtual void ChangeAnim(CharacterAnimType type)
    {
        characterVisual.ChangeAnim(type);
    }
    public virtual void AddTarget(Character ch)
    {
        listTarget.Add(ch);
    }
    public virtual void SetTarget()
    {
        int cnt = listTarget.Count;
        if(cnt == 0)
        {
            Idle();
        }
        else
        {
            List<int> removeTarget = new List<int>();
            bool hasTarget = false;

            for(int i = 0 ; i < cnt ; i++)
            {
                Character target = listTarget[i];
                bool targetOutRange = Helper.CheckDistanceOutRange(pos,target.pos,range + offsetRange);

                if(!hasTarget && !targetOutRange)
                {
                    hasTarget = true;
                    characterVisual.RotateToTarget(target.tf.position);
                    Attack();
                }
                else if(targetOutRange)
                {
                    removeTarget.Add(i);
                }
            }

            if(!hasTarget) Idle();

            int removeCnt = removeTarget.Count;
            if(removeCnt == 0 ) return;

            for(int i = removeCnt -1 ; i >=0 ; i --)
            {
                listTarget.RemoveAt(removeTarget[i]);
            }
        }
    }
    
}
