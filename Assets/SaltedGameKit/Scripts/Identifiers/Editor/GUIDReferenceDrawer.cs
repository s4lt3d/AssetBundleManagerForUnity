#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace SaltedGameKit
{
    [CustomPropertyDrawer(typeof(GUIDReference))]
    public class GUIDReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Get the current GUID
            SerializedProperty guidProperty = property.FindPropertyRelative("guid");
            string currentGuid = guidProperty.stringValue;

            
            string path = AssetDatabase.GUIDToAssetPath(currentGuid);
            
            // Convert GUID to GameObject
            GameObject currentObject = string.IsNullOrEmpty(path)
                ? null
                : AssetDatabase.LoadAssetAtPath<GameObject>(path);
            

            string sidelabel = "GameObject";

            AssetImporter importer = AssetImporter.GetAtPath(path);
            

            if (importer && string.IsNullOrEmpty(importer.assetBundleName))
            {
                EditorGUILayout.HelpBox("Not assigned to any asset bundles!",
                    MessageType.Warning);
            }

            if (currentObject)
            {
                PrefabUniqueIdentifier identifier = currentObject.GetComponent<PrefabUniqueIdentifier>();

                if (!identifier)
                {
                    EditorGUILayout.HelpBox("This prefab does not have a PrefabUniqueIdentifier!", MessageType.Warning);

                    bool userWantsToAdd = EditorUtility.DisplayDialog("Missing Serialized GUID",
                        $"Missing GUID prefab on object {currentObject.name}. Will attempt to fix automatically.",
                        "OK");

                    if (userWantsToAdd)
                    {
                        identifier = currentObject.AddComponent<PrefabUniqueIdentifier>();
                        identifier.GenerateUniqueID();
                        EditorUtility.SetDirty(currentObject);
                    }
                }
            }

            // Draw ObjectField
            GameObject newObject =
                EditorGUI.ObjectField(position, sidelabel, currentObject, typeof(GameObject), false) as GameObject;

            // If the selected GameObject changed, update the GUID
            if (newObject != currentObject)
            {
                if (newObject != null)
                {
                    string newGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(newObject));
                    guidProperty.stringValue = newGuid;
                }
                else
                {
                    guidProperty.stringValue = string.Empty;
                }
            }

            EditorGUI.EndProperty();
        }
    }
}
#endif
