using System;
using System.Collections;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private float duration = 0.25f;

    [SerializeField]
    private AnimationCurve curve =
        AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("Scale")]
    [SerializeField] private bool useScale = true;
    [SerializeField] private Vector3 scaleFrom = Vector3.zero;
    [SerializeField] private Vector3 scaleTo = Vector3.one;

    [Header("Fade")]
    [SerializeField] private bool useFade = false;

    [SerializeField, Range(0f, 1f)]
    private float alphaFrom = 0f;

    [SerializeField, Range(0f, 1f)]
    private float alphaTo = 1f;

    [Header("Move")]
    [SerializeField] private bool useMove = false;
    [SerializeField] private Vector2 positionFrom;
    [SerializeField] private Vector2 positionTo;

    [Header("Rotate")]
    [SerializeField] private bool useRotate = false;
    [SerializeField] private Vector3 rotationFrom;
    [SerializeField] private Vector3 rotationTo;

    [Header("References")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private CanvasGroup canvasGroup;

    private Coroutine animationCoroutine;

    public bool IsPlaying { get; private set; }

    public bool IsShown { get; private set; }

    public event Action OnComplete;

    // =========================================================
    // UNITY
    // =========================================================

    private void Awake()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        if (canvasGroup == null && useFade)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }

    // =========================================================
    // PUBLIC
    // =========================================================

    public void Play()
    {
        StartAnimation(false);
    }

    public void PlayReverse()
    {
        StartAnimation(true);
    }

    public void Stop()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }

        IsPlaying = false;
    }

    // =========================================================
    // START
    // =========================================================

    private void StartAnimation(bool reverse)
    {
        Stop();

        animationCoroutine = StartCoroutine(
            Animate(reverse)
        );
    }

    // =========================================================
    // ANIMATION
    // =========================================================

    private IEnumerator Animate(bool reverse)
    {
        IsPlaying = true;

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            float normalizedTime =
                Mathf.Clamp01(time / duration);

            float t = curve.Evaluate(normalizedTime);

            if (reverse)
            {
                t = 1f - t;
            }

            UpdateAnimation(t);

            yield return null;
        }

        // Đảm bảo giá trị cuối chính xác
        UpdateAnimation(reverse ? 0f : 1f);

        IsPlaying = false;

        animationCoroutine = null;

        IsShown = !reverse;

        OnComplete?.Invoke();
    }

    // =========================================================
    // UPDATE
    // =========================================================

    private void UpdateAnimation(float t)
    {
        if (useScale)
        {
            UpdateScale(t);
        }

        if (useFade)
        {
            UpdateFade(t);
        }

        if (useMove)
        {
            UpdateMove(t);
        }

        if (useRotate)
        {
            UpdateRotate(t);
        }
    }

    // =========================================================
    // SCALE
    // =========================================================

    private void UpdateScale(float t)
    {
        transform.localScale =
            Vector3.Lerp(
                scaleFrom,
                scaleTo,
                t
            );
    }

    // =========================================================
    // FADE
    // =========================================================

    private void UpdateFade(float t)
    {
        if (canvasGroup == null)
        {
            return;
        }

        canvasGroup.alpha =
            Mathf.Lerp(
                alphaFrom,
                alphaTo,
                t
            );
    }

    // =========================================================
    // MOVE
    // =========================================================

    private void UpdateMove(float t)
    {
        if (rectTransform == null)
        {
            return;
        }

        rectTransform.anchoredPosition =
            Vector2.Lerp(
                positionFrom,
                positionTo,
                t
            );
    }

    // =========================================================
    // ROTATE
    // =========================================================

    private void UpdateRotate(float t)
    {
        transform.localEulerAngles =
            Vector3.Lerp(
                rotationFrom,
                rotationTo,
                t
            );
    }

    // =========================================================
    // IMMEDIATE
    // =========================================================

    public void SetStart()
    {
        UpdateAnimation(0f);

        IsShown = false;
    }

    public void SetEnd()
    {
        UpdateAnimation(1f);

        IsShown = true;
    }

    // =========================================================
    // DESTROY
    // =========================================================

    private void OnDestroy()
    {
        Stop();
    }
}