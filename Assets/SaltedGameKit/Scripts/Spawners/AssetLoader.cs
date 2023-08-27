using UnityEngine;
using System.Collections.Generic;

namespace SaltedGameKit
{
    
    public class AssetLoader : MonoBehaviour
    {
        [SerializeField]
        protected List<GUIDReference> UnityGUIDs = new List<GUIDReference>();
        
        protected List<GameObject> spawnedPrefabs = new List<GameObject>();

        public List<GUIDReference> PrefabUniqueIDs
        {
            get => UnityGUIDs;
            set => UnityGUIDs = value;
        }

        private void Start()
        {
            if (AssetBundleManager.Instance)
            {
                foreach (var guid in UnityGUIDs)
                {
                    var prefab = AssetBundleManager.Instance.LoadPrefab(guid.Guid);
                    if (prefab)
                    {
                        spawnedPrefabs.Add(Instantiate(prefab, transform));
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if (AssetBundleManager.Instance)
            {
                foreach (var guid in UnityGUIDs)
                {
                    AssetBundleManager.Instance.UnloadPrefab(guid.Guid);
                }
            }
        }
    }
}
