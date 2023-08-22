using UnityEngine;

[DisallowMultipleComponent]
public class PrefabIdentifier : MonoBehaviour
{
    public string uniqueID;

#if UNITY_EDITOR
    // This method is called in the editor when the script is loaded or a value is changed in the Inspector.
    void OnValidate()
    {
        if (string.IsNullOrEmpty(uniqueID))
        {
            // Set uniqueID to a new GUID.
            uniqueID = System.Guid.NewGuid().ToString();
        }
    }
#endif
}
