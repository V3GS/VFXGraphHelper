using UnityEditor;
using UnityEditor.Experimental.VFX.Utility;
using UnityEngine;
using UnityEngine.VFX;

public class PointCacheVFXHelperEditor : EditorWindow
{
    private PointCacheAsset source;
    private Mesh mesh;
    private bool[] surfacesSelected = null;
    private Texture2D.EXRFlags exrFlagsSelected = Texture2D.EXRFlags.None;

    [MenuItem("Tools/Visual Effect Graph/Generate Point Cache textures")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(PointCacheVFXHelperEditor));
    }

    void OnGUI()
    {
        GUILayout.Label("Point Cache asset to Textures", EditorStyles.boldLabel);
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Point Cache asset");
        GUILayout.Space(-140);
        // Object field for loading a .pcache file
        source = (PointCacheAsset)EditorGUILayout.ObjectField(source, typeof(PointCacheAsset), true);
        GUILayout.EndHorizontal();

        if (source)
        {
            // Now you can select the type of encoding to an EXR file
            exrFlagsSelected = (Texture2D.EXRFlags)EditorGUILayout.EnumPopup("Texture2D format:", exrFlagsSelected);

            int surfacesLenght = source.surfaces.Length;

            // An array is created based on the surfaces storaged in the .pcache file
            if (surfacesSelected == null)
                surfacesSelected = new bool[surfacesLenght];

            // A set of checkboxes is displayed for selecting the surface images that will be created
            // i.e.: position, normal, color, uv (this information depends of how the Point Cache file was created)
            for (int i = 0; i < surfacesLenght; i++)
            {
                surfacesSelected[i] = EditorGUILayout.Toggle(source.surfaces[i].name, surfacesSelected[i]);
            }

            if (GUILayout.Button("Generate image files"))
            {
                string pointCachePath = AssetDatabase.GetAssetPath(source);
                string path = pointCachePath.Replace(source.name + ".pcache", "");

                for (int i = 0; i < surfacesLenght; i++)
                {
                    // If the current checkbox is enabled, it should be created an image file
                    if (surfacesSelected[i])
                    {
                        string filePath = SaveImageFile(source.name, path, source.surfaces[i]);
                        ApplyTextureImportSettings(filePath);
                    }
                }
            }
        }
        else
            surfacesSelected = null;
    }

    RenderTexture m_Input;

    /// <summary>
    /// Method in charge of creating an image located in a defined path
    /// </summary>
    /// <param name="pointCacheAssetName">Point cache name</param>
    /// <param name="path">Location where the image will be created</param>
    /// <param name="texture">Current texture that will be created</param>
    /// <returns>Returns the file path of the texture created</returns>
    private string SaveImageFile(string pointCacheAssetName, string path, Texture2D texture)
    {
        byte[] bytes;
        bytes = texture.EncodeToEXR(exrFlagsSelected);

        string newFilePath = path + pointCacheAssetName + "_" + texture.name + ".exr";
        System.IO.File.WriteAllBytes(newFilePath, bytes);
        AssetDatabase.ImportAsset(newFilePath);

        return newFilePath;
    }

    /// <summary>
    /// This method applies a set of setting where the texture is imported for visualize the VFX
    /// similar to the Point Cache approach
    /// </summary>
    /// <param name="path">Texture location</param>
    private void ApplyTextureImportSettings(string path)
    {
        TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;

        if (ti)
        {
            ti.npotScale = TextureImporterNPOTScale.None;

            TextureImporterPlatformSettings tips = new TextureImporterPlatformSettings {
                resizeAlgorithm = TextureResizeAlgorithm.Bilinear,
                textureCompression = TextureImporterCompression.Uncompressed,
            };

            ti.SetPlatformTextureSettings(tips);
            AssetDatabase.ImportAsset(path);
        }
    }
}
