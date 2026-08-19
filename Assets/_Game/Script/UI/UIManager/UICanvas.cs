using UnityEngine;
using TMPro;

public class UICanvas : MonoBehaviour
{
    [SerializeField] bool isDestroyOnClose = false ;
    private void Awake()
    {
        RectTransform rect = GetComponent<RectTransform>(); 
        float ratio = (float)Screen.width / (float)Screen.height;
        if(ratio > 2.1f)
        {
            Vector2 leftBottom = rect.offsetMin; 
            Vector2 rightBottom = rect.offsetMax;

            leftBottom.y = 0f; 
            rightBottom.y= -100f; 

            rect.offsetMin = leftBottom; 
            rect.offsetMax = rightBottom;
        }
    }
    // Goi truoc khi canvas active 
    public virtual void Setup()
    {
        
    }

    public virtual void Open()
    {
        gameObject.SetActive(true); 
    }
    public virtual void Close(float time)
    {
        Invoke(nameof(CloseDirectly), time) ; 
    }
    public virtual void CloseDirectly()
    {
        if (isDestroyOnClose)
        {
            Destroy(gameObject); 
        }
        else
        {
            gameObject.SetActive(false); 
        }
    }
}
