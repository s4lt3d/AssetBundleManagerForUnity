using UnityEngine;

namespace SagoMini
{
    public class AssetSpawner : MonoBehaviour
    {
        [SerializeField] 
        protected string prefabUniqueID = "";

        protected GameObject prefabToSpawn;

        public string PrefabUniqueID
        {
            get => prefabUniqueID;
            set => prefabUniqueID = value;
        }

        private void Start()
        {
            prefabToSpawn = AssetBundleManager.Instance.GetPrefabFromAssetBundle(prefabUniqueID);

            if (prefabToSpawn)
                Instantiate(prefabToSpawn, transform);
        }

        private void OnDestroy()
        {
            if (AssetBundleManager.Instance)
                AssetBundleManager.Instance.UnloadPrefab(prefabUniqueID);
        }
    }
}