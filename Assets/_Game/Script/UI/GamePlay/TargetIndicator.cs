using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetIndicator : MonoBehaviour
{
    [SerializeField] protected RectTransform rect;
    [SerializeField] protected TextMeshProUGUI textMesh;
    [SerializeField] protected Image imgBG;
     
    //TODO Oninit hoawcj serialize 
    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    public void UpdatePosition(Vector2 screenPosition)
    {
        rect.anchoredPosition = screenPosition;
    }
    public void SetText(int level)
    {
        textMesh.text = level.ToString();
    }
    public void SetColor(ColorType color)
    {
        imgBG.color = DataManager.Instance.GetMaterial(color).color;
    }
    
}