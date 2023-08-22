using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    protected string prefabUniqueID = "f6acd2e9-99ea-434a-9f69-dacaf873b8ce";
    
    protected GameObject prefabToSpawn;

    public string PrefabUniqueID
    {
        get => prefabUniqueID;
        set => prefabUniqueID = value;
    }

    void Awake()
    {
        prefabToSpawn = AssetBundleManager.Instance.GetPrefabFromAssetBundle(prefabUniqueID);
    }

    void Start()
    {
        if (prefabToSpawn)
            Instantiate(prefabToSpawn);
    }
}