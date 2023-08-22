using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    protected string prefabUniqueID = "f6acd2e9-99ea-434a-9f69-dacaf873b8ce";
    
    protected GameObject prefabToSpawn;
    protected AssetBundle myLoadedAssetBundle;

    public string PrefabUniqueID
    {
        get => prefabUniqueID;
        set => prefabUniqueID = value;
    }

    void Awake() {
    
        var path = Application.dataPath+ "/AssetBundles/circle";
        Debug.Log(path);
        myLoadedAssetBundle 
            = AssetBundle.LoadFromFile(path);
        if (myLoadedAssetBundle == null) {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }
        
        prefabToSpawn = LoadPrefabFromAssetBundle(PrefabUniqueID);
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
            var identifier = prefab.GetComponent<PrefabUniqueIdentifier>();
            if(identifier != null && identifier.UniqueID == id)
            {
                return prefab;
            }
        }
        return null;
    }
}
