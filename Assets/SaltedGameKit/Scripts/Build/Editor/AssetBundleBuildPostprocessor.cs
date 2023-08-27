using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace SaltedGameKit
{
    /// <summary>
    /// Not for production. Used for testing updating asset bundles without rebuilding the project. Windows only.
    /// </summary>
    public class AssetBundleBuildPostprocessor
    {
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {

            BuildConfiguration config = Resources.Load<BuildConfiguration>(Path.Combine("Settings", "LocalConfig"));

            if (!config)
            {
                Debug.LogError(
                    "Please create a Build Configuration File.");
                return;
            }

            string sourceDir = Path.Combine(Application.dataPath, config.assetBundleDirectoryPath);

            string destinationDir = Path.Combine(pathToBuiltProject.Replace(".exe", "_Data"), config.assetBundleDirectoryPath);
            
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            foreach (var filePath in Directory.GetFiles(sourceDir))
            {
                string destFilePath = Path.Combine(destinationDir, Path.GetFileName(filePath));
                File.Copy(filePath, destFilePath, true);
            }

            Debug.Log("Asset Bundles Copied.");
        }
    }
}