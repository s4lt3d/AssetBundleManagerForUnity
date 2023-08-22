using UnityEngine;

public class AssetBundleManager : MonoBehaviour
{
    private static AssetBundleManager instance;
    private AssetBundle loadedAssetBundle;

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
    }

    public void LoadAssetBundle(string path)
    {
        if (loadedAssetBundle != null)
        {
            loadedAssetBundle.Unload(true);
        }

        loadedAssetBundle = AssetBundle.LoadFromFile(path);
        if (loadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
        }
    }

    public GameObject GetPrefabFromAssetBundle(string id)
    {
        foreach (var prefab in loadedAssetBundle.LoadAllAssets<GameObject>())
        {
            var identifier = prefab.GetComponent<PrefabUniqueIdentifier>();
            if (identifier != null && identifier.UniqueID == id)
            {
                return prefab;
            }
        }
        return null;
    }
}