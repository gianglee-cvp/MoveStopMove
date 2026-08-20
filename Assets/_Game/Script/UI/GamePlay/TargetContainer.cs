using System.Collections.Generic;
using UnityEngine;

public class TargetContainer : MonoBehaviour
{
    [SerializeField] protected Camera cam;
    [SerializeField] protected RectTransform canvasRect;
    [SerializeField] protected TargetIndicator indicatorPrefab;
    [SerializeField] protected float borderPadding = 200f;
    [SerializeField] protected Vector2 baseOffset;

    private readonly Dictionary<Bot, TargetIndicator> activeTargets = new Dictionary<Bot, TargetIndicator>();

    void Awake()
    {
        cam = Camera.main;
    }

    public void RegisterTarget(Bot target)
    {
        if (activeTargets.ContainsKey(target)) return;

        TargetIndicator uiInstance = Instantiate(indicatorPrefab, canvasRect);
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
    }

    void LateUpdate()
    {
        foreach (var pair in activeTargets)
        {
            Bot target = pair.Key;
            TargetIndicator ui = pair.Value;

            if (target == null || ui == null) continue;

            UpdateSinglePosition(target, ui);
            UpdateLevel(target, ui);
        }
    }

    private void UpdateSinglePosition(Bot target, TargetIndicator ui)
    {
        Vector3 screenPos = cam.WorldToScreenPoint(target.GetHeadPos());

        bool isBehind = screenPos.z < 0;
        if (isBehind)
        {
            screenPos.x = -screenPos.x;
            screenPos.y = -screenPos.y;
        }

        Vector2 canvasSize = canvasRect.sizeDelta;
        Vector2 screenCenter = canvasSize * 0.5f;

        Vector2 rawIndicatorPos = ConvertScreenToCanvasPosition(screenPos, canvasSize, screenCenter);

        float limitX = screenCenter.x - borderPadding;
        float limitY = screenCenter.y - borderPadding;

        Vector2 desiredPosition = CalculateOnScreenIndicatorPosition(rawIndicatorPos, baseOffset);
        bool isOffScreen = IsOffScreen(desiredPosition, isBehind, limitX, limitY);

        Vector2 indicatorPos = CalculateDisplayedIndicatorPosition(
            rawIndicatorPos,
            baseOffset,
            isBehind,
            limitX,
            limitY
        );

        ui.gameObject.SetActive(true);
        ui.SetNameVisible(!isOffScreen);
        ui.SetArrowVisible(isOffScreen);
        ui.UpdatePosition(indicatorPos);
    }

    private static Vector2 ClampToBorder(Vector2 indicatorPos, float limitX, float limitY)
    {
        if (Mathf.Approximately(indicatorPos.x, 0f))
        {
            return new Vector2(0f, indicatorPos.y >= 0f ? limitY : -limitY);
        }

        float slope = indicatorPos.y / indicatorPos.x;

        if (Mathf.Abs(indicatorPos.x) * limitY > Mathf.Abs(indicatorPos.y) * limitX)
        {
            float clampedX = indicatorPos.x > 0 ? limitX : -limitX;
            return new Vector2(clampedX, clampedX * slope);
        }

        float clampedY = indicatorPos.y > 0 ? limitY : -limitY;
        return new Vector2(clampedY / slope, clampedY);
    }

    public void UpdateLevel(Bot bot, TargetIndicator indicator)
    {
        indicator.SetText(bot.Level);
    }

    public static Vector2 CalculateOnScreenIndicatorPosition(Vector2 screenPosition, Vector2 baseOffset)
    {
        return screenPosition + baseOffset;
    }

    public static Vector2 CalculateDisplayedIndicatorPosition(
        Vector2 screenPosition,
        Vector2 baseOffset,
        bool isBehind,
        float limitX,
        float limitY)
    {
        Vector2 desiredPosition = CalculateOnScreenIndicatorPosition(screenPosition, baseOffset);
        bool isOffScreen = IsOffScreen(desiredPosition, isBehind, limitX, limitY);

        if (!isOffScreen)
        {
            return desiredPosition;
        }

        return ClampToBorder(desiredPosition, limitX, limitY);
    }

    public static bool IsOffScreen(Vector2 desiredPosition, bool isBehind, float limitX, float limitY)
    {
        return isBehind ||
               Mathf.Abs(desiredPosition.x) > limitX ||
               Mathf.Abs(desiredPosition.y) > limitY;
    }

    public static Vector2 ConvertScreenToCanvasPosition(Vector3 screenPos, Vector2 canvasSize, Vector2 screenCenter)
    {
        return new Vector2(
            (screenPos.x / Screen.width) * canvasSize.x - screenCenter.x,
            (screenPos.y / Screen.height) * canvasSize.y - screenCenter.y
        );
    }
}
