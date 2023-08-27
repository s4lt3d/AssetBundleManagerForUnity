using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SaltedGameKit
{
    /// <summary>
    /// Abstract base class for Asset Bundle Management. Configuration Pattern
    /// Allows for asset bundle managers to be loaded. 
    /// Use cases: managers can provide local, remote, or simulated asset bundles, or change to an entirely new system
    ///            without needing to change classes which use the asset bundle manager. 
    /// </summary>
    public abstract class AssetBundleManager : MonoBehaviour
    {
        protected static AssetBundleManager instance;
        protected static bool destroyed;

        protected static BuildConfiguration config;


        // Map unique IDs to the asset's asset bundle to load. 
        protected readonly Dictionary<string, string> assetBundleManifest = new();
        protected readonly Dictionary<string, int> assetBundleReferenceCounter = new();
        protected readonly Dictionary<string, AssetBundle> loadedAssetBundles = new();
        protected string assetBundlePath = "";

        public static AssetBundleManager Instance
        {
            get
            {
                if (instance == null && !destroyed)
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

        protected void OnApplicationQuit()
        {
            destroyed = true;
        }

        private static BuildConfiguration LoadConfig()
        {
            if (config == null)
                return Resources.Load<BuildConfiguration>(Path.Combine("Settings", "LocalConfig"));

            return config;
        }

        protected abstract void LoadManifest();
        protected abstract void LoadAssetBundle(string path);
        protected abstract void UnloadAssetBundle(string path);
        public abstract GameObject LoadPrefab(string id);
        public abstract void UnloadPrefab(string id);
    }
}