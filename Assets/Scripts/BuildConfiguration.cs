using UnityEngine;

namespace SagoMini
{
    [CreateAssetMenu(menuName = "Build Configuration")]
    public class BuildConfiguration : ScriptableObject
    {
        public string assetBundleDirectoryPath = "AssetBundles";
        public string manifestFileName = "AssetManifest.json";
    }
}