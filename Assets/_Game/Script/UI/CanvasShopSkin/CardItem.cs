using UnityEngine;
using UnityEngine.UI;

public class CardItem : MonoBehaviour
{
    [SerializeField] protected Image imgLock;
    [SerializeField] protected Image imgItem;
    public void Init(bool isLock , Sprite sprite)
    {
        AddImgItem(sprite);
        SetImgLock(isLock);
    }
    public void SetImgLock(bool isLock)
    {
        imgLock.gameObject.SetActive(isLock);
    }
    public void AddImgItem(Sprite sprite)
    {
        imgItem.sprite = sprite;
    }
}