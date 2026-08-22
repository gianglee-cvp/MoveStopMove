using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class CanvasLoading : UICanvas
{
    [SerializeField] protected Image imgBlackFade;
    [SerializeField] protected float fadeDuration = 0.4f;

    public float FadeDuration => fadeDuration;

    public override void Open()
    {
        ResetFadeAlpha(0f);
        base.Open();
        StartCoroutine(WaitUntilLoadedThenFade());
    }

    private IEnumerator WaitUntilLoadedThenFade()
    {
        yield return new WaitUntil(() => GameManager.Instance.IsLoaded);

        Color color = imgBlackFade.color;
        color.a = 0f;
        imgBlackFade.color = color;

        if (fadeDuration <= 0f)
        {
            color.a = 1f;
            imgBlackFade.color = color;
            CloseDirectly();
            yield break;
        }

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            float alpha = elapsed / fadeDuration;

            color.a = alpha;
            imgBlackFade.color = color;

            yield return null;
        }

        color.a = 1f;
        imgBlackFade.color = color;
        CloseDirectly();
    }

    private void ResetFadeAlpha(float alpha)
    {
        if (imgBlackFade == null) return;

        Color color = imgBlackFade.color;
        color.a = alpha;
        imgBlackFade.color = color;
    }
}
