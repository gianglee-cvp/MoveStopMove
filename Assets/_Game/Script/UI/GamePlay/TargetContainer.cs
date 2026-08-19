using System.Collections.Generic;
using UnityEngine;
//TODO sửa không để singleton ở đây 
public class TargetContainer : MonoBehaviour
{
    [SerializeField] protected Camera cam; 
    [SerializeField] protected RectTransform canvasRect;
    [SerializeField] protected TargetIndicator indicatorPrefab;//TODO cho vaof pool dunfg spawn 
    [SerializeField] protected float borderPadding = 200f;
    private Dictionary<Bot, TargetIndicator> activeTargets = new Dictionary<Bot, TargetIndicator>();

    void Awake()
    {
        cam = Camera.main;
    }
    public void RegisterTarget(Bot target)
    {
        if (activeTargets.ContainsKey(target)) return;
        TargetIndicator uiInstance = Instantiate(indicatorPrefab, canvasRect);
        // uiInstance.SetLevel(target.level); set level 
        activeTargets.Add(target, uiInstance);
        uiInstance.SetColor(target.GetColor());
    }
    public void UnregisterTarget(Bot target)
    {
        if (activeTargets.TryGetValue(target, out TargetIndicator uiInstance))
        {
            if (uiInstance != null) Destroy(uiInstance.gameObject);
            activeTargets.Remove(target);
        }
        //Despawn 
        // if(!activeTargets.ContainsKey(target)) return;
        // activeTargets.Remove(target);
    }
    void LateUpdate()
    {
        foreach (var pair in activeTargets)
        {
            Bot target = pair.Key;
            TargetIndicator ui = pair.Value;

            if (target != null && ui != null)
            {
                UpdateSinglePosition(target.transform, ui);
            }
            UpdateLevel(target,ui);
        }
    }
    private void UpdateSinglePosition(Transform targetTransform, TargetIndicator ui)
    {
        Vector3 screenPos = cam.WorldToScreenPoint(targetTransform.position);

        // 1. Kiểm tra nếu ở sau lưng Camera -> Đảo tọa độ
        bool isBehind = screenPos.z < 0;
        if (isBehind)
        {
            screenPos.x = -screenPos.x;
            screenPos.y = -screenPos.y;
        }

        // 2. Chuyển tọa độ về tâm Canvas (Center-Origin)
        Vector2 canvasSize = canvasRect.sizeDelta;
        Vector2 screenCenter = canvasSize * 0.5f;

        Vector2 indicatorPos = new Vector2(
            (screenPos.x / Screen.width) * canvasSize.x - screenCenter.x,
            (screenPos.y / Screen.height) * canvasSize.y - screenCenter.y
        );

        // 3. Giới hạn vị trí (Clamping) nếu ra khỏi góc nhìn
        float limitX = screenCenter.x - borderPadding;
        float limitY = screenCenter.y - borderPadding;

        bool isOffScreen = isBehind || 
                           Mathf.Abs(indicatorPos.x) > limitX || 
                           Mathf.Abs(indicatorPos.y) > limitY;

        ui.gameObject.SetActive(isOffScreen);
        if (isOffScreen)
        {
            //TODO sửa trường hợp indicator Pos.x = 0 
            float m = indicatorPos.y / indicatorPos.x;

            if (Mathf.Abs(indicatorPos.x) * limitY > Mathf.Abs(indicatorPos.y) * limitX)
            {
                indicatorPos.x = indicatorPos.x > 0 ? limitX : -limitX;
                indicatorPos.y = indicatorPos.x * m;
            }
            else
            {
                indicatorPos.y = indicatorPos.y > 0 ? limitY : -limitY;
                indicatorPos.x = indicatorPos.y / m;
            }
        }

        // 4. Cập nhật vị trí UI
        ui.UpdatePosition(indicatorPos);
    }
    public void UpdateLevel(Bot bot , TargetIndicator indicator)
    {
        indicator.SetText(bot.Level);   
    }

}
