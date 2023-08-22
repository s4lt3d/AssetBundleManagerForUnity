using UnityEngine;

namespace SagoMini
{
    [CreateAssetMenu(menuName = "Build Configuration")]
    public class BuildConfiguration : ScriptableObject
    {
        [Tooltip("Path for asset bundles, relative to Application.dataPath")]
        public string assetBundleDirectoryPath = "AssetBundles";

        [Tooltip("Name of the manifest file to be generated.")]
        public string manifestFileName = "AssetManifest.json";
    }
}