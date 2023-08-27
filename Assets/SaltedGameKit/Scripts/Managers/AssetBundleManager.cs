using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SaltedGameKit
{
    /// <summary>
    /// Base class for Asset Bundle Management. Configuration Pattern
    /// Allows for asset bundle managers to be loaded. 
    /// Use cases: managers can provide local, remote, or simulated asset bundles, or change to an entirely new system
    ///            without needing to change classes which use the asset bundle manager. 
    /// </summary>
    public class AssetBundleManager : MonoBehaviour
    {
        protected static AssetBundleManager instance;
        protected static bool destroyed;

        protected static BuildConfiguration Config;

        // Map unique IDs to the asset's asset bundle to load. 
        protected readonly Dictionary<string, string> assetBundleManifest = new();
        protected readonly Dictionary<string, int> assetBundleReferenceCounter = new();
        protected readonly Dictionary<string, AssetBundle> loadedAssetBundles = new();
        protected string assetBundlePath = "";

        
        #if SIMULATEDASSETS
        protected const string CONFIG = "SimulatedConfig";
        #elif LOCALASSETS
        protected const string CONFIG = "LocalConfig";
        #else
        protected const string CONFIG = "LocalConfig";
        #endif
        
        
        public static AssetBundleManager Instance
        {
            get
            {
                if (instance == null && !destroyed)
                {
                    Config = LoadConfig();

                    if (Config == null || Config.ManagerPrefab == null)
                    {
                        Debug.LogError("No valid AssetBundleManagerConfig found!");
                        return null;
                    }

                    instance = Instantiate(Config.ManagerPrefab);
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

            Config = LoadConfig();
            assetBundlePath = Path.Combine(Application.dataPath, Config.assetBundleDirectoryPath);
            LoadManifest();
        }

        protected void OnApplicationQuit()
        {
            destroyed = true;
        }

        private static BuildConfiguration LoadConfig()
        {
            if (Config == null)
            {
                return Resources.Load<BuildConfiguration>(Path.Combine("Settings", CONFIG));
            }

            return Config;
        }

        protected virtual void LoadManifest()
        {
            // empty
        }

        protected virtual void LoadAssetBundle(string path)
        {
            // empty
        }

        protected virtual void UnloadAssetBundle(string path)
        {
            // empty
        }

        public virtual GameObject LoadPrefab(string id)
        {
            // empty
            return null;
        }

        public virtual void UnloadPrefab(string id)
        {
            // empty
        }
    }
}