using UnityEngine;

namespace SagoMini
{
    /// <summary>
    /// Spawns prefab on start using asset manager.
    /// Uses PrefabUniqueIdentifier to load assets. 
    /// </summary>
    public class AssetSpawner : MonoBehaviour
    {
        [HideInInspector, SerializeField] 
        protected string prefabUniqueID = "";

        protected GameObject spawnedPrefab;

        public string PrefabUniqueID
        {
            get => prefabUniqueID;
            set => prefabUniqueID = value;
        }

        private void Start()
        {
            if (AssetBundleManager.Instance)
                spawnedPrefab = AssetBundleManager.Instance.LoadPrefab(prefabUniqueID);

            if (spawnedPrefab)
                Instantiate(spawnedPrefab, transform);
        }

        private void OnDestroy()
        {
            if (AssetBundleManager.Instance)
                AssetBundleManager.Instance.UnloadPrefab(prefabUniqueID);
        }
    }
}