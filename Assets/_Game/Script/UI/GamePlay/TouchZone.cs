using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;

public class TouchZone : MonoBehaviour, IPointerDownHandler , IDragHandler , IPointerUpHandler
{
    [SerializeField] protected RectTransform joyStickHolder;
    [SerializeField] protected CanvasGroup joyStickCanvasGroup;
    [SerializeField] protected OnScreenStick joyStickVisual; 
    void Awake()
    {
        ActiveJoyStick();
        SetJoystickVisible(false);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        // joyStickHolder.gameObject.SetActive(true);
        joyStickHolder.position = eventData.position;
        SetJoystickVisible(true);

        ExecuteEvents.Execute(
            joyStickVisual.gameObject,
            eventData,
            ExecuteEvents.pointerDownHandler
        );
    }
    public void OnDrag(PointerEventData eventData)
    {
        ExecuteEvents.Execute(
            joyStickVisual.gameObject,
            eventData,
            ExecuteEvents.dragHandler
        );
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        ExecuteEvents.Execute(
            joyStickVisual.gameObject,
            eventData,
            ExecuteEvents.pointerUpHandler
        );
        // joyStickHolder.gameObject.SetActive(false);
        SetJoystickVisible(false);
    }

    // fix loi khi joystick hien len khien touch zone -> pointer up 
    private void SetJoystickVisible(bool visible)
    {
        joyStickCanvasGroup.alpha = visible ? 1f : 0f;
        // tat trong inspector
        // joyStickCanvasGroup.blocksRaycasts = false;
        // joyStickCanvasGroup.interactable = false;
    }
    public void ReleaseJoystick()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        OnPointerUp(eventData);
        joyStickHolder.gameObject.SetActive(false);
    }
    public void ActiveJoyStick()
    {
        joyStickHolder.gameObject.SetActive(true);
    }
}