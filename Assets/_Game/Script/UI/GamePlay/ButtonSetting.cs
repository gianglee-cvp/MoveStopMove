using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class ButtonSeting : MonoBehaviour
{
    [SerializeField] protected UIAnimation anim; 
    [SerializeField] protected Button button;
    public void Open(Action onComplete = null)
    {
        gameObject.SetActive(true);
        StopAllCoroutines();

        if (anim != null)
        {
            if (onComplete != null)
            {
                Action callback = null;
                callback = () =>
                {
                    anim.OnComplete -= callback;
                    onComplete.Invoke();
                };
                anim.OnComplete += callback;
            }
            anim.Play();
        }
        else
        {
            onComplete?.Invoke();
        }
    }
    public void Close(Action onComplete = null)
    {
        StopAllCoroutines();

        if (gameObject.activeInHierarchy && anim != null)
        {
            Action callback = null;
            callback = () =>
            {
                anim.OnComplete -= callback;
                gameObject.SetActive(false);
                onComplete?.Invoke();
            };
            anim.OnComplete += callback;
            anim.PlayReverse();
        }
        else
        {
            gameObject.SetActive(false);
            onComplete?.Invoke();
        }
    }
}