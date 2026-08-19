using System;
using UnityEngine;

public class CharacterLevel : MonoBehaviour
{
    [SerializeField] protected int level;
    [SerializeField] protected float size;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float range;
    [SerializeField] protected int exp;
    [SerializeField] AttackRange attackRange;

    [SerializeField] CharacterVisual characterVisual;
    [SerializeField] CapsuleCollider physicCollider;

    public int Level => level;
    public float MoveSpeed => moveSpeed;
    public float Range => range;
    public float Size => size;
    public Vector3 Scale => Vector3.one * size;
    public int Exp => exp;
    public void PowerUp(int level)
    {
        this.level = level;
        SetRange(Constant.RANGE_DEFAULT + level);
        SetSize(range);
        characterVisual.transform.localScale = Scale;

        physicCollider.radius = Size * Constant.CH_PHYSIC_COLLIDER_RADIUS;
        physicCollider.height = Constant.CH_PHYSIC_COLLIDER_HEIGHT * Size; 
        physicCollider.center = Size  * Constant.CH_PHYSIC_COLLIDER_CENTER; 
    }
    public void SetLevel(int level)
    {
        this.level = level;
    }

    public void SetSize(float range)
    {
        size = range / Constant.RANGE_DEFAULT;
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }
    
    public int CalculateExp(int increase)
    {
        int tmpExp = exp + Mathf.Max(1, increase);
        int tmpLevel = level;

        if (tmpLevel == 0) tmpLevel = 1;
        
        for (int i = 0; i < 300; i++)
        {
            if (tmpExp - tmpLevel < 0) break;

            tmpExp -= tmpLevel;
            tmpLevel++;
        }

        exp = tmpExp;
        return tmpLevel;
    }

    public void SetRange(float rangeF)
    {
        // range = rangeF > Constant.RANGE_MAX ? Constant.RANGE_MAX : rangeF;
        range = Math.Clamp(rangeF,Constant.RANGE_DEFAULT,Constant.RANGE_MAX);
        attackRange.UpdateRange(range);
    }
    public float CalculatorSizeByLevel(int level)
    {
        float tmpRange = Math.Clamp(Constant.RANGE_DEFAULT + level,Constant.RANGE_DEFAULT,Constant.RANGE_MAX);
        return tmpRange/Constant.RANGE_DEFAULT;

    }
}
