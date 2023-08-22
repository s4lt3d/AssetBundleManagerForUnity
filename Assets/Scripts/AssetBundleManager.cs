using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AssetBundleManager : MonoBehaviour
{
    public static string AssetBundlesPath = "AssetBundles";
    
    private static AssetBundleManager instance;

    private Dictionary<string, AssetBundle> loadedAssetBundles = new Dictionary<string, AssetBundle>();
    private Dictionary<string, int> assetBundleReferenceCounter = new Dictionary<string, int>();

    // Manifest Dictionary to map unique IDs to their asset bundle paths
    private Dictionary<string, string> manifestDictionary = new Dictionary<string, string>();

    public static AssetBundleManager Instance
    {
        get
        {
            if (instance == null)
            {
                var foundObjects = FindObjectsOfType<AssetBundleManager>();

                if (foundObjects.Length == 0)
                {
                    GameObject go = new GameObject("AssetBundleManager");
                    instance = go.AddComponent<AssetBundleManager>();
                }
                else if (foundObjects.Length >= 1)
                {
                    instance = foundObjects[0];
                    for (int i = 1; i < foundObjects.Length; i++)
                    {
                        Destroy(foundObjects[i].gameObject);
                    }
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        LoadManifest();
    }

    private void LoadManifest()
    {
        string jsonPath = Path.Combine(Application.dataPath, "AssetBundles/AssetManifest.json");
        if (File.Exists(jsonPath))
        {
            string jsonData = File.ReadAllText(jsonPath);
            List<AssetBundleLookup> pairs = JsonUtility.FromJson<AssetBundleLookupSerializableHelper>(jsonData).items;
            
            foreach (var pair in pairs)
            {
                manifestDictionary[pair.uniqueid] = pair.assetbundle;
            }
        }
    }

    public void LoadAssetBundle(string path)
    {
        if (!loadedAssetBundles.ContainsKey(path))
        {
            AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, AssetBundlesPath, path));
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
        if (manifestDictionary.TryGetValue(id, out string path))
        {
            LoadAssetBundle(path);
            AssetBundle loadedAssetBundle = loadedAssetBundles[path];

            foreach (var prefab in loadedAssetBundle.LoadAllAssets<GameObject>())
            {
                var identifier = prefab.GetComponent<PrefabUniqueIdentifier>();
                if (identifier != null && identifier.UniqueID == id)
                {
                    return prefab;
                }
            }
        }

        return null;
    }

    public void UnloadPrefab(string id)
    {
        if (manifestDictionary.TryGetValue(id, out string path))
            UnloadAssetBundle(path);
    }
}
