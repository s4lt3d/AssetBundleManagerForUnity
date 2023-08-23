using UnityEditor;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SagoMini
{
    /// <summary>
    /// Asset Bundle Build Script
    /// Builds Asset Bundles
    /// Builds a manifest PrefabUniqueID to Path for loading asset bundles. 
    /// </summary>
    public class AssetBundleBuildScript
    {
        static BuildConfiguration config;

        [MenuItem("Assets/* SagoMini * Build AssetBundles and Manifest")]
        static void BuildAllAssetBundles()
        {
            config = Resources.Load<BuildConfiguration>(Path.Combine("Settings", "LocalConfig"));

            if (!config)
            {
                Debug.LogError(
                    "Build Configuration not found!\nPlease set up a Build Configuration first. \nRight click in the project view, create, build configuration. \nResources/Settings/LocalConfig.asset");
                return;
            }

            var assetBundleDirectory = BuildAssetBundles();
            GenerateAssetManifest(assetBundleDirectory);
        }

        /// <summary>
        /// Builds the asset bundles
        /// </summary>
        private static string BuildAssetBundles()
        {
            string assetBundleDirectory = Path.Combine(Application.dataPath, config.assetBundleDirectoryPath);
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
                    string assetBundleName = AssetDatabase.GetImplicitAssetBundleName(path);
                    if (string.IsNullOrEmpty(assetBundleName))
                    {
                        Debug.LogWarning(
                            $"Prefab '{go.name}' with UniqueID '{identifier.UniqueID}' is not assigned to any asset bundle!");
                    }
                    else
                    {
                        prefabToAssetBundleMap.Add(new AssetBundleLookup(identifier.UniqueID, assetBundleName, go.name,
                            prefabGuid, path));
                    }
                }
            }

            AssetBundleLookupSerializableHelper assetBundleLookupSerializable = new AssetBundleLookupSerializableHelper
                { items = prefabToAssetBundleMap };
            string jsonPath = Path.Combine(Application.dataPath, config.assetBundleDirectoryPath,
                config.manifestFileName);
            string jsonContent = JsonUtility.ToJson(assetBundleLookupSerializable, true);
            File.WriteAllText(jsonPath, jsonContent);
            Debug.Log("Asset bundle building completed and manifest file generated.");
        }
    }
}