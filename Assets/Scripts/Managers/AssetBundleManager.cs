using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SagoMini
{
    public abstract class AssetBundleManager : MonoBehaviour
    {
        protected static AssetBundleManager instance;
        protected static BuildConfiguration config;
        protected string assetBundlePath = "";
        
        // Map unique IDs to the asset's asset bundle to load. 
        protected readonly Dictionary<string, string> assetBundleManifest = new();
        protected readonly Dictionary<string, int> assetBundleReferenceCounter = new();
        protected readonly Dictionary<string, AssetBundle> loadedAssetBundles = new();
        
        public static AssetBundleManager Instance
        {
            get
            {
                if (instance == null)
                {
                    config = LoadConfig();

                    if (config == null || config.ManagerPrefab == null)
                    {
                        Debug.LogError("No valid AssetBundleManagerConfig found!");
                        return null;
                    }

                    instance = Instantiate(config.ManagerPrefab);
                }
                
                return instance;
            }
        }

        protected static BuildConfiguration LoadConfig()
        {
            if(config == null)
                return Resources.Load<BuildConfiguration>(Path.Combine("Settings", "LocalConfig"));
            
            return config;
        }

        protected void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            config = LoadConfig();
            assetBundlePath = Path.Combine(Application.dataPath, config.assetBundleDirectoryPath);
            LoadManifest();
        }

        protected abstract void LoadManifest();
        public abstract void LoadAssetBundle(string path);
        public abstract void UnloadAssetBundle(string path);
        public abstract GameObject GetPrefabFromAssetBundle(string id);
        public abstract void UnloadPrefab(string id);
    }
}