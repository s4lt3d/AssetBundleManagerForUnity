using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

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
        protected string uniqueID;

        public string UniqueID => uniqueID;

#if UNITY_EDITOR
        private void OnValidate()
        {
            GenerateUniqueID();
        }

        public void GenerateUniqueID()
        {
            if (string.IsNullOrEmpty(UniqueID))
            {
                string assetPath = AssetDatabase.GetAssetPath(gameObject);
                uniqueID = AssetDatabase.AssetPathToGUID(assetPath);
            }
        }
#endif
    }
}