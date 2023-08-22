using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SagoMini
{
    public class AssetBundleManager : MonoBehaviour
    {
        private static AssetBundleManager instance;
        private readonly Dictionary<string, int> assetBundleReferenceCounter = new();

        private readonly Dictionary<string, AssetBundle> loadedAssetBundles = new();

        // Manifest Dictionary to map unique IDs to their asset bundle paths
        private readonly Dictionary<string, string> manifestDictionary = new();

        private string assetBundlePath = "";
        private BuildConfiguration config;

        public static AssetBundleManager Instance
        {
            get
            {
                if (instance == null)
                {
                    var foundObjects = FindObjectsOfType<AssetBundleManager>();

                    if (foundObjects.Length == 0)
                    {
                        var go = new GameObject("AssetBundleManager");
                        instance = go.AddComponent<AssetBundleManager>();
                    }
                    else if (foundObjects.Length >= 1)
                    {
                        instance = foundObjects[0];
                        for (var i = 1; i < foundObjects.Length; i++) Destroy(foundObjects[i].gameObject);
                    }
                }

                return instance;
            }
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            config = Resources.Load<BuildConfiguration>(Path.Combine("Settings", "LocalConfig"));
            assetBundlePath = Path.Combine(Application.dataPath, config.assetBundleDirectoryPath);
            LoadManifest();
        }

        private void LoadManifest()
        {
            var jsonPath = Path.Combine(Application.dataPath, config.assetBundleDirectoryPath, config.manifestFileName);
            if (File.Exists(jsonPath))
            {
                var jsonData = File.ReadAllText(jsonPath);
                var pairs = JsonUtility.FromJson<AssetBundleLookupSerializableHelper>(jsonData)
                    .items;

                foreach (var pair in pairs) manifestDictionary[pair.uniqueid] = pair.assetbundle;
            }
        }

        public void LoadAssetBundle(string path)
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

        public void UnloadAssetBundle(string path)
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

        public GameObject GetPrefabFromAssetBundle(string id)
        {
            if (manifestDictionary.TryGetValue(id, out var path))
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

        public void UnloadPrefab(string id)
        {
            if (manifestDictionary.TryGetValue(id, out var path))
                UnloadAssetBundle(path);
        }
    }
}