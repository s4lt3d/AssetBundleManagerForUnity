#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Spawner))]
public class SpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        bool showAssetBundleInfo = false;
        
        Spawner spawner = (Spawner)target;

        // Draw the default inspector
        DrawDefaultInspector();

        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Delete && GUI.GetNameOfFocusedControl() == "Prefab")
        {
            spawner.PrefabUniqueID = "";
            Event.current.Use();
            return; // Exit early
        }
        
        GameObject matchingPrefab = FindPrefabByUniqueID(spawner.PrefabUniqueID);
        GUI.SetNextControlName("Prefab");
        GameObject tempDroppedPrefab = (GameObject)EditorGUILayout.ObjectField("Prefab", matchingPrefab, typeof(GameObject), false);

        // Display Asset Bundle information
        if (matchingPrefab)
        {
            string assetPath = AssetDatabase.GetAssetPath(matchingPrefab);
            AssetImporter importer = AssetImporter.GetAtPath(assetPath);
            
            if (string.IsNullOrEmpty(importer.assetBundleName))
            {
                EditorGUILayout.HelpBox("The linked prefab is not assigned to any asset bundles!", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.Foldout(true, "Asset Bundle Info", true);
                EditorGUI.indentLevel++;  // Increase the indent for a clear hierarchy view
                EditorGUILayout.LabelField("Asset Bundle:", importer.assetBundleName);
                EditorGUI.indentLevel--;  // Reset the indent
            }
        }

        // If the user has dropped a new GameObject onto the field
        if (tempDroppedPrefab)
        {
            PrefabUniqueIdentifier identifier = tempDroppedPrefab.GetComponent<PrefabUniqueIdentifier>();

            if (!identifier)
            {
                // Display warning if the PrefabUniqueIdentifier is missing
                EditorGUILayout.HelpBox("This prefab does not have a PrefabUniqueIdentifier!", MessageType.Warning);

                // Ask the user if they want to add the PrefabUniqueIdentifier component to the prefab
                bool userWantsToAdd = EditorUtility.DisplayDialog("Missing PrefabUniqueIdentifier", 
                    "Do you want to add a PrefabUniqueIdentifier to this prefab?", "OK", "Cancel");

                if (userWantsToAdd)
                {
                    identifier = tempDroppedPrefab.AddComponent<PrefabUniqueIdentifier>();
                    identifier.GenerateUniqueID();
                    EditorUtility.SetDirty(tempDroppedPrefab); // Mark the prefab as dirty to save changes
                    EditorUtility.DisplayDialog("Missing PrefabUniqueIdentifier","Component has been added for you! \n\nPlease try again.", "Thanks");
                }
            }
            else
            {
                // Update the PrefabUniqueID field in the Spawner script with the unique ID of the dropped prefab
                spawner.PrefabUniqueID = identifier.UniqueID;
                EditorGUILayout.HelpBox("Unique ID updated!", MessageType.Info);
            }
        }

        // Button to select the located prefab in Project view
        if (GUILayout.Button("Locate Prefab in Project") && matchingPrefab)
        {
            Selection.activeObject = matchingPrefab;
            EditorGUIUtility.PingObject(matchingPrefab);
        }
    }

    private GameObject FindPrefabByUniqueID(string id)
    {
        // Get all prefab asset paths in the project
        string[] allPrefabs = AssetDatabase.FindAssets("t:GameObject", null);

        foreach (string prefab in allPrefabs)
        {
            string path = AssetDatabase.GUIDToAssetPath(prefab);
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            PrefabUniqueIdentifier identifier = go.GetComponent<PrefabUniqueIdentifier>();
            if (identifier && identifier.UniqueID == id)
            {
                return go;
            }
        }
        return null;
    }
}
#endif
