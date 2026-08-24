using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character,IData
{
    //TODO bỏ logic input
    protected InputAction moveAction;
    protected Vector2 moveAmount;
    protected Dictionary<SkinType, Action<int>> skinSetters;
    public int gold;
    public int level = 0; // TODO xoa 
    public override void OnInit()
    {
        base.OnInit();
        moveAction = InputManager.Instance.MoveAction;
        moveAction.Enable();
        characterLevel.HideAttackRange();
        CanvasGamePlay canvas = UIManager.Instance.GetUI<CanvasGamePlay>();
        canvas.RegisterTarget(this);
    }
    public void ShowRangeUI()
    {
        characterLevel.ShowAttackRange();
    }
    void Update()
    {
        //TODO xoa 
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            gold ++;
            DataManager.Instance.SaveGame();
            characterVisual.ApplySkin();
        }
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            LevelUp(level);
            if(isDead) SaveMe();
        } //TODO xoa 
        if (GameManager.Instance.IsGameState(Enum_GameState.Play))
        {
            SetTarget();
            if (moveAction.enabled)
            {
                moveAmount = moveAction.ReadValue<Vector2>().normalized;
                Vector3 move = new Vector3(moveAmount.x , 0 , moveAmount.y).normalized;
                if(move.sqrMagnitude > 0.001f)
                {
                    if(isAttacking)
                    {
                        CancelAttack();
                        Idle();
                    }
                    if(!isMoving)
                    {
                        characterVisual.ChangeAnim(CharacterAnimType.Run);
                        isMoving = true;
                    }
                    TF.position += move * characterLevel.MoveSpeed * Time.deltaTime;
                    TF.rotation = Quaternion.LookRotation(move);
                }
                else 
                {
                    if (isMoving)
                    {
                        Idle();
                    }

                    if (isAttackable)
                    {
                        Attack();
                    }
                }   
            }
        }
    }
    public override Character SetTarget()
    {
        Character oldTarget = currentTarget;
        currentTarget = base.SetTarget();
        if(oldTarget != currentTarget && currentTarget != null)
        {
            Bot enemy = currentTarget as Bot;
            enemy.ShowTargetIndicator();
        }
        return currentTarget;
    }
    public override void RemoveTarget(int index)
    {
        Character removedTarget = listTarget[index];
        if (removedTarget == currentTarget)
        {
            Bot enemy = currentTarget as Bot;
            enemy.HideTargetIndicator();
        }
        base.RemoveTarget(index);
    }
    public override void LevelUp(int level)
    {
        CameraFollow.Instance.SetSize(characterLevel.CalculatorSizeByLevel(level));
        base.LevelUp(level);
    }
    public void LoadGame(GameData data)
    {
        gold = data.gold;
        characterVisual.ApplyNewSkin(data.GetSkinEquipped());
        Debug.Log("Load gold data" + gold);
    }
    public void SaveGame(ref GameData data)
    {
        data.gold = gold;
        data.SaveSkin(characterVisual.CurrentSkin);
        Debug.Log("Save gold data" + gold);
    }
    public void TryCloth(int index , SkinType type)
    {
        if (skinSetters == null) InitSkinSetters();

        if (skinSetters.TryGetValue(type, out Action<int> setter))
        {
            setter(index);
            characterVisual.ApplySkin();
        }
    }
    public void ReloadCloth()
    {
        characterVisual.ApplyNewSkin(DataManager.Instance.GetSkinEquipped());
    }
    //TODO xoa 
    public void SaveMe()
    {
        OnInit();
    }

    private void InitSkinSetters()
    {
        skinSetters = new Dictionary<SkinType, Action<int>>
        {
            { SkinType.skinColor, value => characterVisual.CurrentSkin.color = (ColorType)value },
            { SkinType.Pant, value => characterVisual.CurrentSkin.pant = (PantType)value },
            { SkinType.Hair, value => characterVisual.CurrentSkin.hairType = (HairType)value },
            { SkinType.Weapon, value => characterVisual.CurrentSkin.weapon = (WeaponType)value },
            { SkinType.Shield, value => characterVisual.CurrentSkin.shieldType = (ShieldType)value }
        };
    }
}
