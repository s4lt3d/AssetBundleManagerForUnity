using UnityEditor;

namespace SaltedGameKit
{
    public class AssetLoadingOptions
    {
        private const string LocalAssetsDefineSymbole = "LOCALASSETS";
        private const string SimulatedAssetsDefineSymbole = "SIMULATEDASSETS";

        [MenuItem("Config/Use Local Config")]
        public static void UseLocalConfig()
        {
            SetScriptingDefineSymbols(LocalAssetsDefineSymbole);
        }

        [MenuItem("Config/Use Local Config", true)]
        public static bool UseLocalConfigValidate()
        {
            UpdateMenuCheckmarks(LocalAssetsDefineSymbole);
            return true;
        }

        [MenuItem("Config/Use Simulated Config")]
        public static void UseSimulatedConfig()
        {
            SetScriptingDefineSymbols(SimulatedAssetsDefineSymbole);
        }

        [MenuItem("Config/Use Simulated Config", true)]
        public static bool UseSimulatedConfigValidate()
        {
            UpdateMenuCheckmarks(SimulatedAssetsDefineSymbole);
            return true;
        }

        private static void UpdateMenuCheckmarks(string selectedDefine)
        {
            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);

            Menu.SetChecked("Config/Use Local Config", defines.Contains(LocalAssetsDefineSymbole));
            Menu.SetChecked("Config/Use Simulated Config", defines.Contains(SimulatedAssetsDefineSymbole));
        }

        private static void SetScriptingDefineSymbols(string define)
        {
            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);

            if (define == LocalAssetsDefineSymbole)
            {
                defines = defines.Replace(SimulatedAssetsDefineSymbole, "");
                defines += ";" + LocalAssetsDefineSymbole;
            }
            else if (define == SimulatedAssetsDefineSymbole)
            {
                defines = defines.Replace(LocalAssetsDefineSymbole, "");
                defines += ";" + SimulatedAssetsDefineSymbole;
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines);
        }
    }
}