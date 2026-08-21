using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetIndicator : MonoBehaviour
{
    [SerializeField] protected RectTransform rect;
    [SerializeField] protected TextMeshProUGUI textLevel;
    [SerializeField] protected TextMeshProUGUI textName;

    [SerializeField] protected Image imgArrow;
    [SerializeField] protected Image imgBG;
    [SerializeField] protected Transform arrowRoot;

    [SerializeField] protected float arrowRotationOffset;
     
    public void UpdatePosition(Vector2 screenPosition)
    {
        rect.anchoredPosition = screenPosition;
        UpdateArrowDirection();
    }
    public void SetText(int level)
    {
        textLevel.text = level.ToString();
    }
    public void SetColor(ColorType color)
    {
        Color targetColor = DataManager.Instance.GetItemData<ColorItemData>(SkinType.skinColor, (int)color).Material.color;
        imgBG.color = targetColor;
        imgArrow.color  = targetColor;
        textName.color = targetColor;
    }

    public void SetNameVisible(bool isVisible)
    {
        if (textName == null) return;

        textName.gameObject.SetActive(isVisible);
    }

    public void SetArrowVisible(bool isVisible)
    {
        if (arrowRoot == null) return;

        arrowRoot.gameObject.SetActive(isVisible);
    }

    public void UpdateArrowDirection()
    {
        if (arrowRoot == null) return;

        float angle = CalculateArrowAngle(rect.anchoredPosition, arrowRotationOffset);
        arrowRoot.localEulerAngles = new Vector3(0f, 0f, angle);
    }

    public static float CalculateArrowAngle(Vector2 directionFromCenter, float rotationOffset)
    {
        if (directionFromCenter.sqrMagnitude <= Mathf.Epsilon)
        {
            return rotationOffset;
        }

        return Mathf.Atan2(directionFromCenter.y, directionFromCenter.x) * Mathf.Rad2Deg + rotationOffset;
    }
}
