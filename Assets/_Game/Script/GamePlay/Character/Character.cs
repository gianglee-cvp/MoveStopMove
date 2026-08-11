using System.Collections;
using Unity.VisualScripting;
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
    //TODO đổi logic input sang chỗ khác 

    private InputAction moveAction;
    private Vector2 moveAmount;

    protected bool isMoving = false;
    protected bool isAttacking = false;
    //TODO attack theo range khong phai bool 
    public bool attackpress = false; 
    public void OnInit()
    {
        characterVisual.OnInit();
        moveAction = InputManager.Instance.MoveAction;
        moveAction.Enable();
        ChangeAnim(CharacterAnimType.Idle);
    }
    void Update()
    {   
        if (moveAction.enabled)
        {
            moveAmount = moveAction.ReadValue<Vector2>().normalized;
            Vector3 move = new Vector3(moveAmount.x , 0 , moveAmount.y);
            if(move.sqrMagnitude > 0.001f)
            {
                if(!isMoving)
                {
                    ChangeAnim(CharacterAnimType.Run);
                    isMoving = true;
                }
                //TODO them bien speed
                tf.Translate(move * 5f * Time.deltaTime);
                characterVisual.ChangeRotation(move);
            }
            else 
            {
                // if (!isAttacking)
                // {
                //     isMoving = false;
                //     ChangeAnim(CharacterAnimType.Idle);
                // }

            }
        }
        if (attackpress)
        {
            Attack();
            attackpress = false;
        }
    }
    public void Attack()
    {
        isAttacking  = true;
        ChangeAnim(CharacterAnimType.Attack);
        Vector3 pos = tf.position + transform.forward * 3;
        StartCoroutine(Throw(pos)); 

    }
    private IEnumerator Throw(Vector3 pos)
    {
        //TODO cho vao constant
        yield return new WaitForSeconds(0.3f);
        if (isAttacking)
        {
            characterVisual.Throw(pos);
        }
        isAttacking = false;
        //todo cho vao constant
        yield return new WaitForSeconds(0.7f);

        ChangeAnim(CharacterAnimType.Idle);
        characterVisual.ActiveWeapon();
    }
    public void ChangeAnim(CharacterAnimType type)
    {
        characterVisual.ChangeAnim(type);
    }
    
    // public void Attack()
    // {
    //     characterVisual.Attack();
    //     //TODO 0.4 cho vaof const
    //     Invoke(nameof(Throw),0.4f);
    // }
    // public void Throw()
    // {
    //     characterVisual.Throw();
    //     isAttacking = false;
    // }
    public bool CheckAttack()
    {
        return isAttacking;
    }
    public void SetAttackState(bool attack)
    {
        isAttacking = attack;
    }
}