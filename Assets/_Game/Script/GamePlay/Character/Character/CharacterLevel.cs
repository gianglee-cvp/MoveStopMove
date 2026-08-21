using System;
using UnityEngine;

public class CharacterLevel : MonoBehaviour
{
    [SerializeField] protected int level;
    [SerializeField] protected float size;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float range;
    [SerializeField] protected int exp;
    [SerializeField] protected AttackRange attackRange;

    [SerializeField] protected CharacterVisual characterVisual;
    [SerializeField] protected CapsuleCollider physicCollider;

    //bonus 
    protected float bonusRange = 0;
    protected float bonusMoveSpeed = 0;
    protected float baseMoveSpeed = 0;

    private void Awake()
    {
        if (moveSpeed <= 0f)
        {
            moveSpeed = Constant.MOVE_SPEED_DEFAULT;
        }

        baseMoveSpeed = moveSpeed;
        SetMoveSpeed(baseMoveSpeed);
    }
    

    public int Level => level;
    public float MoveSpeed => moveSpeed;
    public float Range => range;
    public float Size => size;
    public Vector3 Scale => Vector3.one * size;
    public int Exp => exp;
    public void PowerUp(int level)
    {
        this.level = level;
        SetRange(CalculateRangeByLevel(level));
        SetSize(CalculatorSizeByLevel(level));
        ApplyBooster();
        ApplyMoveSpeedBooster();
    }
    public void SetLevel(int level)
    {
        this.level = level;
    }

    public void SetSize(float newSize)
    {
        size = Mathf.Max(1f, newSize);
        characterVisual.transform.localScale = Scale;
        
        if (physicCollider != null)
        {
            physicCollider.radius = Size * Constant.CH_PHYSIC_COLLIDER_RADIUS;
            physicCollider.height = Constant.CH_PHYSIC_COLLIDER_HEIGHT * Size;
            physicCollider.center = Size * Constant.CH_PHYSIC_COLLIDER_CENTER;
        }
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
        range = Math.Clamp(rangeF,Constant.RANGE_DEFAULT,Constant.RANGE_MAX);
        if (attackRange != null)
        {
            attackRange.UpdateRange(range);
        }
    }
    public float CalculateRangeByLevel(int level)
    {
        return Math.Clamp(Constant.RANGE_DEFAULT + level, Constant.RANGE_DEFAULT, Constant.RANGE_MAX);
    }
    public float CalculatorSizeByLevel(int level)
    {
        return CalculateRangeByLevel(level) / Constant.RANGE_DEFAULT;
    }
    public void ApplyBooster()
    {
        float boostedRange = CalculateRangeByLevel(level) * (1 + bonusRange / 100f);
        SetRange(boostedRange);
    }
    public void ApplyMoveSpeedBooster()
    {
        float boostedMoveSpeed = baseMoveSpeed * (1 + bonusMoveSpeed / 100f);
        SetMoveSpeed(boostedMoveSpeed);
    }
    public void InitBooster(float rangeBonusPercent)
    {
        bonusRange = rangeBonusPercent;
        ApplyBooster();
    }
    public void ResetBoosters()
    {
        bonusRange = 0f;
        bonusMoveSpeed = 0f;
        ApplyBooster();
        ApplyMoveSpeedBooster();
    }
    public void AddRangeBonus(float rangeBonusPercent)
    {
        bonusRange += rangeBonusPercent;
        ApplyBooster();
    }
    public void AddMoveSpeedBonus(float moveSpeedBonusPercent)
    {
        bonusMoveSpeed += moveSpeedBonusPercent;
        ApplyMoveSpeedBooster();
    }
}
