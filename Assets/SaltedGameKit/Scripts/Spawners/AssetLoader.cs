using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace SaltedGameKit
{
    public class AssetLoader : MonoBehaviour
    {
        [SerializeField]
        protected List<GUIDReference> UnityGUIDs = new List<GUIDReference>();
        
        protected List<GameObject> prefabs = new List<GameObject>();
        protected List<GameObject> instantiatedObjects = new List<GameObject>();

        [SerializeField]
        public UnityEvent OnStart;
        
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
                        prefabs.Add(prefab);
                    }
                }
            }

            OnStartAction();
        }

        public void OnStartAction()
        {
            OnStart.Invoke();
        }
        
        public void InstantiatePrefabs()
        {
            foreach (var prefab in prefabs)
            {
                instantiatedObjects.Add(Instantiate(prefab, transform));
            }
        }

        private void OnDestroy()
        {
            foreach (var obj in instantiatedObjects)
            {
                if(obj)
                    Destroy(obj);
            }
            
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
