using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Editor Tool: Chụp icon weapon từ RawImage đang hiển thị RenderTexture.
/// Menu: Tools/Weapon Icon Generator
/// </summary>
public class WeaponIconGenerator : EditorWindow
{
    private RawImage targetImage;

    private string saveFolder = "Assets/_Game/Sprites/WeaponIcons";
    private string fileName = "Weapon_Arrow";
    private float alphaThreshold = 0.01f;

    private bool assignToSOItem = false;
    private SOItem soItem;
    private int weaponIndex = 0;

    [MenuItem("Tools/Weapon Icon Generator")]
    public static void ShowWindow()
    {
        GetWindow<WeaponIconGenerator>("Weapon Icon Gen");
    }

    private void OnGUI()
    {
        GUILayout.Label("Weapon Icon Generator", EditorStyles.boldLabel);

        GUILayout.Space(6);

        targetImage = (RawImage)EditorGUILayout.ObjectField(
            "Target Image (RawImage)", targetImage, typeof(RawImage), true);

        GUILayout.Space(6);
        GUILayout.Label("Output", EditorStyles.boldLabel);

        fileName = EditorGUILayout.TextField("File Name", fileName);
        saveFolder = EditorGUILayout.TextField("Save Folder", saveFolder);
        alphaThreshold = EditorGUILayout.Slider("Alpha Threshold", alphaThreshold, 0f, 1f);

        GUILayout.Space(6);

        assignToSOItem = EditorGUILayout.Toggle("Assign to SOItem", assignToSOItem);
        if (assignToSOItem)
        {
            soItem = (SOItem)EditorGUILayout.ObjectField(
                "SO Item", soItem, typeof(SOItem), false);
            weaponIndex = EditorGUILayout.IntField("Weapon Index", weaponIndex);
        }

        GUILayout.Space(12);

        GUI.enabled = targetImage != null && targetImage.texture is RenderTexture;
        if (GUILayout.Button("Capture & Save", GUILayout.Height(36)))
        {
            Capture();
        }
        GUI.enabled = true;
    }

    private void Capture()
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            Debug.LogError("File Name is empty.");
            return;
        }

        Directory.CreateDirectory(saveFolder);

        RenderTexture rt = targetImage.texture as RenderTexture;
        if (rt == null)
        {
            Debug.LogError("Target Image không chứa RenderTexture.");
            return;
        }

        RenderTexture previousRT = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        RenderTexture.active = previousRT;

        ConvertLinearToSrgbIfNeeded(tex);
        Texture2D cropped = CropByAlpha(tex, alphaThreshold);
        DestroyImmediate(tex);

        string path = Path.Combine(saveFolder, fileName + ".png").Replace("\\", "/");
        path = AssetDatabase.GenerateUniqueAssetPath(path);

        File.WriteAllBytes(path, cropped.EncodeToPNG());
        DestroyImmediate(cropped);

        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);

        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.alphaIsTransparency = true;
            importer.mipmapEnabled = false;
            importer.SaveAndReimport();
        }

        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);

        if (assignToSOItem && soItem != null && sprite != null)
        {
            AssignToSOItem(sprite);
        }

        Debug.Log("Saved: " + path);
    }

    private void AssignToSOItem(Sprite sprite)
    {
        SerializedObject serializedSO = new SerializedObject(soItem);
        serializedSO.Update();

        SerializedProperty listWeapon = serializedSO.FindProperty("listWeapon");
        if (listWeapon == null || !listWeapon.isArray)
        {
            Debug.LogWarning("Cannot find 'listWeapon' in SOItem.");
            return;
        }

        if (weaponIndex < 0 || weaponIndex >= listWeapon.arraySize)
        {
            Debug.LogWarning("weaponIndex " + weaponIndex + " out of range (0-" + (listWeapon.arraySize - 1) + ").");
            return;
        }

        SerializedProperty element = listWeapon.GetArrayElementAtIndex(weaponIndex);
        SerializedProperty iconProp = element.FindPropertyRelative("icon");

        if (iconProp == null)
        {
            Debug.LogWarning("Cannot find 'icon' property in WeaponItemData[" + weaponIndex + "].");
            return;
        }

        iconProp.objectReferenceValue = sprite;
        serializedSO.ApplyModifiedProperties();
        EditorUtility.SetDirty(soItem);
        AssetDatabase.SaveAssets();

        Debug.Log("Assigned icon to listWeapon[" + weaponIndex + "] in SOItem.");
    }

    private static void ConvertLinearToSrgbIfNeeded(Texture2D texture)
    {
        if (QualitySettings.activeColorSpace != ColorSpace.Linear) return;
        Color[] pixels = texture.GetPixels();
        for (int i = 0; i < pixels.Length; i++) pixels[i] = pixels[i].gamma;
        texture.SetPixels(pixels);
        texture.Apply();
    }

    private static Texture2D CropByAlpha(Texture2D source, float threshold)
    {
        Color[] pixels = source.GetPixels();
        int minX = source.width, minY = source.height, maxX = -1, maxY = -1;

        for (int y = 0; y < source.height; y++)
        {
            for (int x = 0; x < source.width; x++)
            {
                if (pixels[y * source.width + x].a <= threshold) continue;
                if (x < minX) minX = x;
                if (x > maxX) maxX = x;
                if (y < minY) minY = y;
                if (y > maxY) maxY = y;
            }
        }

        if (maxX < minX || maxY < minY)
        {
            Texture2D fallback = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            fallback.SetPixel(0, 0, Color.clear);
            fallback.Apply();
            return fallback;
        }

        int w = maxX - minX + 1;
        int h = maxY - minY + 1;
        Texture2D cropped = new Texture2D(w, h, TextureFormat.RGBA32, false);
        cropped.SetPixels(source.GetPixels(minX, minY, w, h));
        cropped.Apply();
        return cropped;
    }
}
