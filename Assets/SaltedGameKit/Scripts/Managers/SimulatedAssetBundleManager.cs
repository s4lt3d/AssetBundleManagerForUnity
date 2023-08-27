using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace SaltedGameKit
{
    /// <summary>
    /// Simulated asset bundle manager. Loads and unloads prefabs without going through asset bundle builds.
    /// Loads assets using PrefabUniqueID.
    /// </summary>
    public class SimulatedAssetBundleManager : AssetBundleManager
    {
        protected override void LoadManifest()
        {
            var jsonPath = Path.Combine(Application.dataPath, Config.assetBundleDirectoryPath, Config.manifestFileName);
            if (File.Exists(jsonPath))
            {
                var jsonData = File.ReadAllText(jsonPath);
                var pairs = JsonUtility.FromJson<AssetBundleLookupSerializableHelper>(jsonData)
                    .items;

                foreach (var pair in pairs)
                {
                    assetBundleManifest[pair.guid] = pair.path;
                }
            }
        }

        protected override void LoadAssetBundle(string path)
        {
            // empty
        }

        protected override void UnloadAssetBundle(string path)
        {
            // empty
        }

        /// <summary>
        /// Loads prefab from asset bundle using the unity guid
        /// </summary>
        /// <param name="id">PrefabUniqueIdentifier</param>
        /// <returns>Prefab or null</returns>
        public override GameObject LoadPrefab(string id)
        {
            GameObject prefab = LoadPrefabFromGuid(id);
            if(prefab == null)
                Debug.LogWarning($"Failed to load prefab: {id}. Try rebuilding asset bundles.");
            return prefab;
        }

        private GameObject LoadPrefabFromGuid(string path)
        {
#if UNITY_EDITOR

            path = AssetDatabase.GUIDToAssetPath(path);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            return prefab;
#else
            Debug.LogWarning("Cannot simulate asset bundles outside of the editor.");
            return null;
#endif
        }

        public override void UnloadPrefab(string id)
        {
           // empty
        }
    }
}