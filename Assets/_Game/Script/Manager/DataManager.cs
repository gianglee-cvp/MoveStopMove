using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

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
            dataObject.SaveGame(ref gameData);
        }
        //luu vao json
        dataHandler.Save(gameData);
    }
    public void OnApplicationQuit()
    {
        SaveGame();
    }
    public T GetItemData<T>(SkinType skinType, int index) where T : ItemData => itemData.GetData<T>(skinType, index);
    public ItemData[] GetListData(SkinType type)=> itemData.GetListData(type);

    public BoosterData GetBooster(SkinType skinType) => itemData.GetBooster(skinType);
    private List<IData> FindAllDataObject()
    {
        IEnumerable<IData> objects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IData>();
        return new List<IData>(objects);
    }

}
