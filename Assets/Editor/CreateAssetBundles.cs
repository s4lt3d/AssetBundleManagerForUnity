using UnityEditor;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        var assetBundleDirectory = BuildAssetBundles();
        GenerateAssetManifest(assetBundleDirectory);
    }

    /// <summary>
    /// Builds the asset bundles
    /// </summary>
    private static string BuildAssetBundles()
    {
        string assetBundleDirectory = Path.Combine(Application.dataPath, AssetBundleManager.AssetBundlesPath);
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }

        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
            BuildAssetBundleOptions.None,
            BuildTarget.StandaloneWindows);
        return assetBundleDirectory;
    }

    /// <summary>
    /// Generates the manifest for the prefab id, bundle name pairs
    /// </summary>
    /// <param name="assetBundleDirectory"></param>
    private static void GenerateAssetManifest(string assetBundleDirectory)
    {
        List<SerializableKeyValuePair> prefabToAssetBundleMap = new List<SerializableKeyValuePair>();

        // Get all prefabs in the project
        string[] allPrefabs = AssetDatabase.FindAssets("t:GameObject");
        foreach (string prefab in allPrefabs)
        {
            string path = AssetDatabase.GUIDToAssetPath(prefab);
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (!go)
            {
                Debug.Log($"Failed to load GameObject at path: {path}");
                continue;
            }

            var identifier = go.GetComponent<PrefabUniqueIdentifier>();
            if (identifier)
            {
                Debug.Log($"GameObject with identifier found: {go.name}");

                string assetBundleName = AssetDatabase.GetImplicitAssetBundleName(path);
                if (string.IsNullOrEmpty(assetBundleName))
                {
                    Debug.LogWarning(
                        $"Prefab '{go.name}' with UniqueID '{identifier.UniqueID}' is not assigned to any asset bundle!");
                }
                else
                {
                    prefabToAssetBundleMap.Add(new SerializableKeyValuePair(identifier.UniqueID, assetBundleName));
                    Debug.Log($"Added to map: {go.name} with AssetBundle: {assetBundleName}");
                }
            }
        }

        SerializableKeyValuePairList serializable = new SerializableKeyValuePairList { items = prefabToAssetBundleMap };

        string jsonPath = assetBundleDirectory + "/AssetManifest.json";
        string jsonContent = JsonUtility.ToJson(serializable, true);
        File.WriteAllText(jsonPath, jsonContent);

        Debug.Log("Asset bundle building completed and index file generated.");
    }
}
