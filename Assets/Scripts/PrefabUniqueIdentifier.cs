using System;
using UnityEngine;

namespace SagoMini
{
    [DisallowMultipleComponent]
    public class PrefabUniqueIdentifier : MonoBehaviour
    {
        [HideInInspector] [SerializeField] protected string uniqueID;

        public string UniqueID => uniqueID;

        private void OnDestroy()
        {
            if (AssetBundleManager.Instance)
                AssetBundleManager.Instance.UnloadPrefab(uniqueID);
        }

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