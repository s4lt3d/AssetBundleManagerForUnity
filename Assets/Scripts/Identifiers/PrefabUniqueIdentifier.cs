using System;
using UnityEngine;

namespace SagoMini
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
                // Set uniqueID to a new GUID.
                uniqueID = Guid.NewGuid().ToString();
        }
#endif
    }
}