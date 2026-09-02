
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.Android.Gradle.Manifest;
public class WeaponInfor
{
    protected string name;
    protected string des; 
    protected int price;
    protected GameObject prefab;
    public WeaponInfor(string name , string des , int price , GameObject prefab)
    {
        this.name = name; 
        this.des = des; 
        this.price = price;
        this.prefab = prefab;
    }
    public string Name => name;
    public string Des => des; 
    public int Price => price;
    public GameObject Prefab => prefab;
}
public class CanvasShopWeapon : UICanvas,IData
{
    [SerializeField] protected TextMeshProUGUI priceText;
    [SerializeField] protected TextMeshProUGUI nameWeapon;
    [SerializeField] protected TextMeshProUGUI desWeapon;
    [SerializeField] protected TextMeshProUGUI unlock; //TODO add logic
    [SerializeField] protected Transform spawnPoint;
    protected int currentIndex;
    protected IReadOnlyList<Enum_ShopState> weaponShopState;
    protected List<WeaponInfor> weaponItemDatas = new List<WeaponInfor>();
    protected Dictionary<int , GameObject> objectToShow = new Dictionary<int, GameObject>();
    [SerializeField] protected List<Button> buttonsSelect = new List<Button>();

    public void Awake()
    {
        weaponItemDatas.Clear();
        objectToShow.Clear();
        DataManager.Instance.LoadGame(this);
        ItemData[] items = DataManager.Instance.GetListData(SkinType.Weapon);

        for(int i = 0 ; i < items.Length; i++)
        {
            WeaponItemData item = (WeaponItemData)items[i];
            WeaponInfor infor = new WeaponInfor(item.ItemName , item.Des , item.Price , item.PrefabShop );
            weaponItemDatas.Add(infor);
            InitObject( i , item.PrefabShop);
        }
        if(weaponShopState.Count != weaponItemDatas.Count)
        {
            Debug.LogError("miss count " + weaponShopState.Count + " " + weaponItemDatas.Count );
        }
        currentIndex = 0;
        ChangeIndex(currentIndex);
    }
    public void LoadGame(GameData gameData)
    {
        weaponShopState = gameData.GetShopStateByType(SkinType.Weapon);
    }
    public void SaveGame(GameData gameData)
    {
        
    }
    public void ChangeIndex(int index)
    {
        if(index < 0 || index >= weaponShopState.Count) return;

        objectToShow[currentIndex].SetActive(false);
        objectToShow[index].SetActive(true);

        WeaponInfor wp  = weaponItemDatas[index];
        priceText.text = wp.Price.ToString();
        nameWeapon.text = wp.Name;
        desWeapon.text = wp.Des;
        
        ChangeButtonBuy(weaponShopState[index]);
        currentIndex = index;
    }
    public void ButtonChangeIndex(bool next)
    {
        if (next) ChangeIndex(currentIndex + 1);
        else ChangeIndex(currentIndex - 1);
    }
    public void InitObject(int index , GameObject prefab)
    {
        if (!objectToShow.ContainsKey(index))
        {
            GameObject obj = Instantiate(prefab , spawnPoint);
            Transform TF = obj.transform;
            TF.localPosition = Vector3.zero;
            TF.localRotation = Quaternion.identity;
            objectToShow[index] = obj;
            obj.SetActive(false);
        }
    }
    public void CloseButton()
    {
        Close(0);
        GameManager.Instance.ChangeGameState(Enum_GameState.MainMenu);
        UIManager.Instance.OpenUI<CanvasMainMenu>();
        LevelManager.Instance.ReloadCloth();
    }
    public void ButtonBuy()
    {
        if (DataManager.Instance.BuyItem(SkinType.Weapon, currentIndex))
        {
            ChangeButtonBuy(Enum_ShopState.equipped);
        }
    }
    public void ButtonSelect()
    {
        if (DataManager.Instance.SelectItem(SkinType.Weapon, currentIndex))
        {
            ChangeButtonBuy(Enum_ShopState.equipped);
        }
    }
    public void ChangeButtonBuy(Enum_ShopState type)
    {
        for(int i = 0 ; i < buttonsSelect.Count ; i++)
        {
            buttonsSelect[i].gameObject.SetActive(false);
        }
        buttonsSelect[(int)type].gameObject.SetActive(true);
    }
}
