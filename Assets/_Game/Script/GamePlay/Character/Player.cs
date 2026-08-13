using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    protected InputAction moveAction;
    protected Vector2 moveAmount;
    public override void OnInit()
    {
        base.OnInit();
        moveAction = InputManager.Instance.MoveAction;
        moveAction.Enable();
    }
    void Update()
    {   
        SetTarget();
        if (moveAction.enabled)
        {
            moveAmount = moveAction.ReadValue<Vector2>().normalized;
            Vector3 move = new Vector3(moveAmount.x , 0 , moveAmount.y);
            if(move.sqrMagnitude > 0.001f)
            {
                if(isAttacking)
                {
                    CancelAttack();
                }

                if(!isMoving)
                {
                    ChangeAnim(CharacterAnimType.Run);
                    isMoving = true;
                }
                //TODO them bien speed
                tf.Translate(move * 5f * Time.deltaTime);
                characterVisual.ChangeRotation(move);
                
                if(isAttacking) CancelAttack();
            }
            else 
            {
                if (!isAttacking && isAttackable)
                {
                    Attack();
                }
            }   
        }
        //TODO khong dung press ma dung trigger target
        // if (attackpress)
        // {
        //     if(!isAttackable) return;
        //     Attack();
        //     // attackpress = false;
        // }
    }
    public override Character SetTarget()
    {
        Character oldTarget = currentTarget;
        currentTarget = base.SetTarget();
        if(oldTarget != currentTarget && currentTarget != null)
        {
            EnemyBase enemy = currentTarget as EnemyBase;
            enemy.ShowTargetIndicator();
        }
        return currentTarget;
    }
    public override void RemoveTarget(int index)
    {
        base.RemoveTarget(index);
        EnemyBase enemy = currentTarget as EnemyBase;
        enemy?.HideTargetIndicator();
    }
}