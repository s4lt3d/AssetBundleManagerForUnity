using System.IO;
using UnityEngine;

namespace SagoMini
{
    /// <summary>
    /// Local asset bundle manager. Loads and unloads asset bundles from file.
    /// Loads assets using PrefabUniqueID.
    /// </summary>
    public class LocalAssetBundleManager : AssetBundleManager
    {
        protected override void LoadManifest()
        {
            var jsonPath = Path.Combine(Application.dataPath, config.assetBundleDirectoryPath, config.manifestFileName);
            if (File.Exists(jsonPath))
            {
                var jsonData = File.ReadAllText(jsonPath);
                var pairs = JsonUtility.FromJson<AssetBundleLookupSerializableHelper>(jsonData)
                    .items;

                foreach (var pair in pairs) assetBundleManifest[pair.uniqueid] = pair.assetbundle;
            }
        }

        protected override void LoadAssetBundle(string path)
        {
            if (!loadedAssetBundles.ContainsKey(path))
            {
                var assetBundle =
                    AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, assetBundlePath, path));
                if (assetBundle == null)
                {
                    Debug.LogError("Failed to load AssetBundle from path: " + path);
                    return;
                }

                loadedAssetBundles[path] = assetBundle;
                assetBundleReferenceCounter[path] = 1;
            }
            else
            {
                assetBundleReferenceCounter[path]++;
            }
        }

        protected override void UnloadAssetBundle(string path)
        {
            if (loadedAssetBundles.ContainsKey(path))
            {
                assetBundleReferenceCounter[path]--;
                if (assetBundleReferenceCounter[path] <= 0)
                {
                    loadedAssetBundles[path].Unload(true);
                    loadedAssetBundles.Remove(path);
                    assetBundleReferenceCounter.Remove(path);
                }
            }
        }

        /// <summary>
        /// Loads prefab from asset bundle using PrefabUniqueID
        /// </summary>
        /// <param name="id">PrefabUniqueIdentifier</param>
        /// <returns>Prefab or null</returns>
        public override GameObject LoadPrefab(string id)
        {
            if (assetBundleManifest.TryGetValue(id, out var path))
            {
                LoadAssetBundle(path);
                var loadedAssetBundle = loadedAssetBundles[path];

                foreach (var prefab in loadedAssetBundle.LoadAllAssets<GameObject>())
                {
                    var identifier = prefab.GetComponent<PrefabUniqueIdentifier>();
                    if (identifier != null && identifier.UniqueID == id)
                        return prefab;
                }
            }
            else
            {
                Debug.LogWarning($"Failed to load prefab: {id}. Try rebuilding asset bundles.");
            }

            return null;
        }

        public override void UnloadPrefab(string id)
        {
            if (assetBundleManifest.TryGetValue(id, out var path))
                UnloadAssetBundle(path);
        }
    }
}