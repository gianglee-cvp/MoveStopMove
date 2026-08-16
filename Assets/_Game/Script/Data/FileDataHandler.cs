

using System;
using UnityEngine;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    public FileDataHandler(string dataDirPath , string dataFileName)
    {
        this.dataDirPath = dataDirPath; 
        this.dataFileName = dataFileName;
    }
    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
            Debug.LogError("Error loading Data to file : " + fullPath +"\n" + e );
            }
        }
        return loadedData;

    }
    public void Save(GameData gameData)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            // create path file 
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            // save game 
            string dataToStore = JsonUtility.ToJson(gameData, true);
            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Error saving Data to file : " + fullPath +"\n" + e );
        }
    }
}