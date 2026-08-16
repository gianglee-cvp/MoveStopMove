using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NUnit.Framework.Constraints;

public class DataManager : Singleton<DataManager>
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    private FileDataHandler dataHandler;

    protected GameData gameData;
    protected List<IData> allDataObject;
    [SerializeField] private SOSkin soSkin;

    public void OnInit()
    {
        //Note : cac IData phai duoc tao truoc datamanager va init sau data manager neu co ham load trong init
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
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
    public Material GetMaterial(ColorType color) => soSkin.GetMaterial(color);
    public Texture2D GetPant(PantType type) => soSkin.GetPant(type);
    public Hair GetHair(HairType type) => soSkin.GetHair(type);
    public WeaponBase GetWeapon(WeaponType type) => soSkin.GetWeapon(type);
    public BulletBase GetBullet(WeaponType type) => soSkin.GetBullet(type);
    public string GetName(SkinType type , int index) => soSkin.GetName(type,index);
    private List<IData> FindAllDataObject()
    {
        IEnumerable<IData> objects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IData>();
        return new List<IData>(objects);
    }

}
