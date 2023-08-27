using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace SaltedGameKit
{
    /// <summary>
    /// Creates a unique id to identify game objects.
    /// Benefits over Unity GUIDs:
    ///     Serialized and not recomputed based on environment, unity version, library issues, or merge conflicts. 
    /// </summary>
    [DisallowMultipleComponent]
    public class PrefabUniqueIdentifier : MonoBehaviour
    {
        [HideInInspector] [SerializeField] 
        protected GUIDReference uniqueID;

        public string Guid => uniqueID.Guid;

#if UNITY_EDITOR
        private void OnValidate()
        {
            GenerateUniqueID();
        }

        public void GenerateUniqueID()
        {
            if (string.IsNullOrEmpty(uniqueID.Guid))
            {
                string assetPath = AssetDatabase.GetAssetPath(this);
                GUIDReference guid = new GUIDReference();
                uniqueID.Guid = AssetDatabase.AssetPathToGUID(assetPath);
            }
        }
#endif
    }
}