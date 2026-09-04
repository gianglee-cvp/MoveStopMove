using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardItem : MonoBehaviour
{
    [SerializeField] protected Image imgLock;
    [SerializeField] protected Image imgItem;
    [SerializeField] protected Button button;
    [SerializeField] protected ScrollView owner;
    [SerializeField] protected RectTransform itemRect;
    protected string desText;
    protected int price;
    protected int itemIndex;
    public RectTransform Rect => itemRect;
    public void Init(bool isLock , Sprite sprite , ScrollView target, int index , string s , int price)
    {
        AddImgItem(sprite);
        SetImgLock(isLock);
        AddHolder(target);
        itemIndex = index;
        desText = s; 
        this.price = price;
        BindButton();
    }
    public void SetImgLock(bool isLock)
    {
        imgLock.gameObject.SetActive(isLock);
    }
    public void AddImgItem(Sprite sprite)
    {
        imgItem.sprite = sprite;
    }
    public void AddHolder(ScrollView target)
    {
         owner = target;
    }

    private void BindButton()
    {
        if (button == null) return;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickItem);
    }

    private void OnClickItem()
    {
        if (owner == null) return;
        owner.CallBackTryCloth(itemIndex , desText , price , itemRect);
    }
}
