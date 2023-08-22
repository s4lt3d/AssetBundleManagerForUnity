using UnityEngine;

[DisallowMultipleComponent]
public class PrefabUniqueIdentifier : MonoBehaviour
{
    [HideInInspector, SerializeField]
    protected string uniqueID;

    public string UniqueID => uniqueID;
    
    private void OnDestroy()
    {
        if(AssetBundleManager.Instance)
            AssetBundleManager.Instance.UnloadPrefab(uniqueID);
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        GenerateUniqueID();
    }

    public void GenerateUniqueID()
    {
        if (string.IsNullOrEmpty(UniqueID))
        {
            // Set uniqueID to a new GUID.
            uniqueID = System.Guid.NewGuid().ToString();
        }
    }
#endif
}
