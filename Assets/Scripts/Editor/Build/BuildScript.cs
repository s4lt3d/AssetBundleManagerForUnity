using UnityEditor;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BuildScript
{
    [MenuItem("Assets/* Build AssetBundles and Manifest")]
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
    /// Generates the manifest for the {prefab id, bundle name} pairs. This is used to lookup game objects by id
    /// without having to know ahead of time how the objects are packed into asset bundles.
    /// </summary>
    /// <param name="assetBundleDirectory"></param>
    private static void GenerateAssetManifest(string assetBundleDirectory)
    {
        List<AssetBundleLookup> prefabToAssetBundleMap = new List<AssetBundleLookup>();

        // Get all prefabs in the project with PrefabUniqueIdentifier
        string[] allPrefabsGuids = AssetDatabase.FindAssets("t:GameObject");
        foreach (string prefabGuid in allPrefabsGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(prefabGuid);
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
                    prefabToAssetBundleMap.Add(new AssetBundleLookup(identifier.UniqueID, assetBundleName, go.name, prefabGuid));
                    Debug.Log($"Added to map: {go.name} with AssetBundle: {assetBundleName}");
                }
            }
        }

        AssetBundleLookupSerializableHelper assetBundleLookupSerializable = new AssetBundleLookupSerializableHelper { items = prefabToAssetBundleMap };

        string jsonPath = assetBundleDirectory + "/AssetManifest.json";
        string jsonContent = JsonUtility.ToJson(assetBundleLookupSerializable, true);
        File.WriteAllText(jsonPath, jsonContent);

        Debug.Log("Asset bundle building completed and index file generated.");
    }
}
