using UnityEditor;
using UnityEngine;

public static class GameDataEditorTools
{
    private const string GameDataFileName = "data.game";

    [MenuItem("Tools/Game Data/Create New Game Data")]
    private static void CreateNewGameData()
    {
        if (!EditorUtility.DisplayDialog(
            "Create New Game Data",
            "This replaces the current save data. Continue?",
            "Create New Data",
            "Cancel"))
        {
            return;
        }

        FileDataHandler dataHandler = new FileDataHandler(
            Application.persistentDataPath,
            GameDataFileName
        );
        dataHandler.Save(new GameData());
        Debug.Log("Created and saved new game data.");
    }
}
