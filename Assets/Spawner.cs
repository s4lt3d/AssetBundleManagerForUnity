using UnityEngine;

public class Spawner : MonoBehaviour
{
    public string prefabUniqueID = "f6acd2e9-99ea-434a-9f69-dacaf873b8ce";

    public GameObject prefabToSpawn;

    public AssetBundle myLoadedAssetBundle;
    
    void Awake() {
    
        var path = Application.dataPath+ "/AssetBundles/circle";
        Debug.Log(path);
        myLoadedAssetBundle 
            = AssetBundle.LoadFromFile(path);
        if (myLoadedAssetBundle == null) {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }
        
        //var prefabToSpawn = myLoadedAssetBundle.LoadAsset<GameObject>("Circle");
        //Instantiate(prefabToSpawn);
        
        prefabToSpawn = LoadPrefabFromAssetBundle(prefabUniqueID);
    }

    void Start()
    {
        if(prefabToSpawn)
            Instantiate(prefabToSpawn);
    }
    
    private GameObject LoadPrefabFromAssetBundle(string id)
    {
        foreach (var prefab in myLoadedAssetBundle.LoadAllAssets<GameObject>())
        {
            var identifier = prefab.GetComponent<PrefabIdentifier>();
            if(identifier != null && identifier.uniqueID == id)
            {
                return prefab;
            }
        }
        return null;
    }
}
