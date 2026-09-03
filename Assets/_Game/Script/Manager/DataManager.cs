using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DataManager : Singleton<DataManager>
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    private FileDataHandler dataHandler;

    protected GameData gameData;
    protected List<IData> allDataObject;
    [SerializeField] private SOItem itemData;

    public void OnInit()
    {
        //Note : cac IData phai duoc tao truoc datamanager va init sau data manager neu co ham load trong init
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        Debug.Log(Application.persistentDataPath);
        itemData.OnInit();
        allDataObject = FindAllDataObject();
        Debug.Log("All object" + allDataObject.Count);
        LoadGame();
    }


    public void NewGame()
    {
        gameData = new GameData();
    }
    public void LoadGame()
    {
        //TODO load tu json 
        gameData = dataHandler.Load();
        if(gameData == null)
        {
            Debug.Log("No Data");
            NewGame();
        }
        // push sang cac object data 
        foreach(IData dataObject in allDataObject)
        {
            dataObject.LoadGame(gameData);
        }
    }
    public void LoadGame(IData target)
    {
        target.LoadGame(gameData);
    }
    public void SaveGame()
    {
        Debug.Log("Save Game");
        // lay data ve
        foreach(IData dataObject in allDataObject)
        {
            dataObject.SaveGame(gameData);
        }
        //luu vao json
        dataHandler.Save(gameData);
    }
    public int Gold => gameData != null ? gameData.GetGold() : 0;
    public Skin GetSkinEquipped() => gameData.GetSkinEquipped();
    public void OnApplicationQuit()
    {
        SaveGame();
    }
    public T GetItemData<T>(SkinType skinType, int index) where T : ItemData => itemData.GetData<T>(skinType, index);
    public ItemData[] GetListData(SkinType type)=> itemData.GetListData(type);

    public BoosterData GetBooster(SkinType skinType) => itemData.GetBooster(skinType);
    #region BuyItem
    public void AddGold(int amount)
    {
        gameData.SetGold(gameData.GetGold() + amount);
    }
    public bool TrySpendGold(int amount)
    {
        if (gameData.GetGold() < amount) return false;

        gameData.SetGold(gameData.GetGold() - amount);
        return true;
    }
    public int GetEquippedID(SkinType type)
    {
        return gameData.GetEquippedID(type);
    }
    public IReadOnlyList<Enum_ShopState> GetShopStateByType(SkinType type)
    {
        return gameData.GetShopStateByType(type);
    }
    public void SetShopState(SkinType type, int index, Enum_ShopState state)
    {
        gameData.SetShopState(type, index, state);
    }
    public void EnsureShopStateCount(SkinType type, int requiredCount)
    {
        gameData.EnsureShopStateCount(type, requiredCount);
    }
    public ItemData UnlockRandomItem()
    {
        SkinType[] unlockableTypes = { SkinType.Pant, SkinType.Hair, SkinType.Shield, SkinType.Weapon };

        // Gom tất cả (type, index) đang bị khoá
        var locked = new List<(SkinType type, int index)>();
        foreach (SkinType type in unlockableTypes)
        {
            IReadOnlyList<Enum_ShopState> states = gameData.GetShopStateByType(type);
            if (states == null) continue;
            for (int i = 0; i < states.Count; i++)
            {
                if (states[i] == Enum_ShopState.buy)
                    locked.Add((type, i));
            }
        }

        if (locked.Count == 0) return null;

        // Chọn ngẫu nhiên 1 item
        var chosen = locked[UnityEngine.Random.Range(0, locked.Count)];
        gameData.SetShopState(chosen.type, chosen.index, Enum_ShopState.bought);
        SaveGame();

        return GetItemData<ItemData>(chosen.type, chosen.index);
    }

    public bool BuyItem(ItemData item)
    {
        IReadOnlyList<Enum_ShopState> shopStates = gameData.GetShopStateByType(item.Type);

        if (shopStates == null || shopStates[item.Index] != Enum_ShopState.buy)
            return false;

        if (!TrySpendGold(item.Price))
            return false;

        gameData.SetShopState(item.Type, item.Index, Enum_ShopState.bought);

        SelectItem(item);
        return true;
    }
    public bool BuyItem(SkinType type , int index)
    {
        ItemData item = GetItemData<ItemData>(type , index);
        return BuyItem(item); 
    }
    public bool SelectItem(ItemData item)
    {
        IReadOnlyList<Enum_ShopState> shopStates = gameData.GetShopStateByType(item.Type);

        if (shopStates == null || shopStates[item.Index] != Enum_ShopState.bought)
            return false;

        int oldEquippedIndex = gameData.GetEquippedID(item.Type);

        if (oldEquippedIndex >= 0 &&
            oldEquippedIndex < shopStates.Count &&
            oldEquippedIndex != item.Index)
        {
            gameData.SetShopState(item.Type, oldEquippedIndex, Enum_ShopState.bought);
        }

        gameData.SetShopState(item.Type, item.Index, Enum_ShopState.equipped);
        gameData.SetEquippedID(item.Type, item.Index);
        
        return true;
    }
    public bool SelectItem(SkinType type , int index)
    {
        return SelectItem(GetItemData<ItemData>(type , index));
    }
    #endregion
    private List<IData> FindAllDataObject()
    {
        IEnumerable<IData> objects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IData>();
        return new List<IData>(objects);
    }

}
