using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    [SerializeField] bool isDestroyOnClose = false ;
    [SerializeField] protected List<UIAnimation> uiElement = new List<UIAnimation>();

    private Coroutine closeCoroutine;

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
        StopCloseCoroutine();

        gameObject.SetActive(true); 

        foreach (var element in uiElement)
        {
            if (element != null)
            {
                element.Play();
            }
        }
    }
    public virtual void Close(float time)
    {
        StopCloseCoroutine();
        closeCoroutine = StartCoroutine(CloseWithAnimation(time));
    }

    private IEnumerator CloseWithAnimation(float delay)
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        foreach (var element in uiElement)
        {
            if (element != null)
            {
                element.PlayReverse();
            }
        }

        yield return null;

        yield return new WaitUntil(() =>
            uiElement.TrueForAll(element =>
                element == null || !element.IsPlaying));

        closeCoroutine = null;
        CloseDirectly();
    }

    public virtual void CloseDirectly()
    {
        StopCloseCoroutine();

        if (isDestroyOnClose)
        {
            Destroy(gameObject); 
        }
        else
        {
            gameObject.SetActive(false); 
        }
    }

    private void StopCloseCoroutine()
    {
        if (closeCoroutine != null)
        {
            StopCoroutine(closeCoroutine);
            closeCoroutine = null;
        }
    }
}
