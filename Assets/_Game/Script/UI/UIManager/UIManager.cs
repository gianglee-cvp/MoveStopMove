using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager> 
{
    Dictionary<System.Type , UICanvas> canvasActives  = new Dictionary<System.Type, UICanvas>() ;
    Dictionary<System.Type , UICanvas> canvasPrefabs  = new Dictionary<System.Type, UICanvas>() ;
    [SerializeField] Transform parent;

    public void OnInit()
    {
        UICanvas[] prefabs = Resources.LoadAll<UICanvas>("UI/");
        Debug.Log("Load UI Prefabs : " + prefabs.Length);
        for(int i = 0 ; i < prefabs.Length ; i++)
        {
            canvasPrefabs.Add(prefabs[i].GetType(), prefabs[i]);
        }
    }
    // mo canvas 
    public T OpenUI<T>() where T : UICanvas
    {
        T canvas = GetUI<T>() ;


        canvas.Setup();
        canvas.Open();  

        return canvas as T ; 
    }
    // dong canvas sau time t       
    public void CloseUI<T>(float time) where T : UICanvas
    {
        if (IsLoaded<T>())
        {
        canvasActives[typeof(T)].Close(time);           
        }
    }
    // dong canvas lap tuc 
    public void CloseUIDirectly<T>() where T : UICanvas
    {
        if (IsLoaded<T>())
        {
        canvasActives[typeof(T)].CloseDirectly();           
        }
    }
    // kiem tra canvas da duoc tao chua 
    public bool IsLoaded<T>() where T : UICanvas
    {
        return canvasActives.ContainsKey(typeof(T)) && canvasActives[typeof(T)] != null; 
    }
    // kiem tra canvas duojc active chua 
    public bool IsOpened<T>() where T : UICanvas
    {
        return IsLoaded<T>() && canvasActives[typeof(T)].gameObject.activeSelf; 
    }
    // lay active canvas 
    public T GetUI<T>() where T : UICanvas
    {
        if (!IsLoaded<T>())
        {
            T prefab = GetUIPrefab<T>(); 
            T canvas = Instantiate(prefab , parent);
            canvasActives[typeof(T)] = canvas ; 
        }
        return canvasActives[typeof(T)] as T  ; 
    }

    public T GetUIPrefab<T>() where T : UICanvas
    {
        return canvasPrefabs[typeof(T)] as T ; 
    }
    // dong tat ca canvas 
    public void CloseAllUI()
    {
        foreach( var t in canvasActives)
        {
            if((t.Value != null) && (t.Value.gameObject.activeSelf) )
            {
                t.Value.Close(0); 
            }
        }
    }
}
