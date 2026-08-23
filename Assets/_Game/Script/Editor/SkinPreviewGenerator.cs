using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SkinPreviewGenerator : EditorWindow
{
    private Camera previewCamera;
    private RenderTexture renderTexture;
    private Image targetImage;
    private string folder = "Assets/SkinPreview/Textures";
    private string fileName = "Skin_001";
    private float alphaThreshold = 0.01f;

    [MenuItem("Tools/Skin Preview Generator")]
    public static void ShowWindow()
    {
        GetWindow<SkinPreviewGenerator>("Skin Preview");
    }

    private void OnGUI()
    {
        GUILayout.Label("Skin Preview Generator", EditorStyles.boldLabel);

        previewCamera = (Camera)EditorGUILayout.ObjectField(
            "Preview Camera",
            previewCamera,
            typeof(Camera),
            true
        );

        renderTexture = (RenderTexture)EditorGUILayout.ObjectField(
            "Render Texture",
            renderTexture,
            typeof(RenderTexture),
            false
        );

        targetImage = (Image)EditorGUILayout.ObjectField(
            "Target Image",
            targetImage,
            typeof(Image),
            true
        );

        fileName = EditorGUILayout.TextField(
            "File Name",
            fileName
        );

        folder = EditorGUILayout.TextField(
            "Save Folder",
            folder
        );

        alphaThreshold = EditorGUILayout.Slider(
            "Alpha Threshold",
            alphaThreshold,
            0f,
            1f
        );

        GUILayout.Space(10);

        if (GUILayout.Button("Save Preview", GUILayout.Height(30)))
        {
            SavePreview();
        }
    }

    private void SavePreview()
    {
        if (previewCamera == null)
        {
            Debug.LogError("Preview Camera is missing.");
            return;
        }

        if (renderTexture == null)
        {
            Debug.LogError("Render Texture is missing.");
            return;
        }

        if (string.IsNullOrWhiteSpace(fileName))
        {
            Debug.LogError("File Name cannot be empty.");
            return;
        }

        Directory.CreateDirectory(folder);

        RenderTexture previousRT = RenderTexture.active;
        RenderTexture previousCameraTarget = previewCamera.targetTexture;

        RenderTexture.active = renderTexture;
        previewCamera.targetTexture = renderTexture;

        GL.Clear(
            true,
            true,
            new Color(0f, 0f, 0f, 0f)
        );

        previewCamera.Render();

        Texture2D texture = new Texture2D(
            renderTexture.width,
            renderTexture.height,
            TextureFormat.RGBA32,
            false
        );

        texture.ReadPixels(
            new Rect(
                0,
                0,
                renderTexture.width,
                renderTexture.height
            ),
            0,
            0
        );

        texture.Apply();
        ConvertLinearToSrgbIfNeeded(texture);

        Texture2D croppedTexture = CropByAlpha(texture, alphaThreshold);

        RenderTexture.active = previousRT;
        previewCamera.targetTexture = previousCameraTarget;

        string path = Path.Combine(
            folder,
            fileName + ".png"
        ).Replace("\\", "/");
        path = AssetDatabase.GenerateUniqueAssetPath(path);

        File.WriteAllBytes(
            path,
            croppedTexture.EncodeToPNG()
        );

        Object.DestroyImmediate(texture);
        Object.DestroyImmediate(croppedTexture);

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
        if (targetImage != null && sprite != null)
        {
            targetImage.sprite = sprite;
            EditorUtility.SetDirty(targetImage);
        }

        Debug.Log($"Saved preview, imported as sprite, and assigned image if available: {path}");
    }

    private static void ConvertLinearToSrgbIfNeeded(Texture2D texture)
    {
        if (QualitySettings.activeColorSpace != ColorSpace.Linear)
        {
            return;
        }

        Color[] pixels = texture.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = pixels[i].gamma;
        }

        texture.SetPixels(pixels);
        texture.Apply();
    }

    private static Texture2D CropByAlpha(Texture2D source, float threshold)
    {
        Color[] pixels = source.GetPixels();

        int minX = source.width;
        int minY = source.height;
        int maxX = -1;
        int maxY = -1;

        for (int y = 0; y < source.height; y++)
        {
            for (int x = 0; x < source.width; x++)
            {
                Color pixel = pixels[y * source.width + x];
                if (pixel.a <= threshold)
                {
                    continue;
                }

                if (x < minX) minX = x;
                if (x > maxX) maxX = x;
                if (y < minY) minY = y;
                if (y > maxY) maxY = y;
            }
        }

        if (maxX < minX || maxY < minY)
        {
            Texture2D fallbackTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            fallbackTexture.SetPixel(0, 0, Color.clear);
            fallbackTexture.Apply();
            return fallbackTexture;
        }

        int croppedWidth = maxX - minX + 1;
        int croppedHeight = maxY - minY + 1;

        Texture2D croppedTexture = new Texture2D(croppedWidth, croppedHeight, TextureFormat.RGBA32, false);
        croppedTexture.SetPixels(source.GetPixels(minX, minY, croppedWidth, croppedHeight));
        croppedTexture.Apply();

        return croppedTexture;
    }
}
