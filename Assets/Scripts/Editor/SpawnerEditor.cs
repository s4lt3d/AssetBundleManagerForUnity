#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace SagoMini
{
    [CustomEditor(typeof(Spawner))]
    public class SpawnerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Spawner spawner = (Spawner)target;

            DrawDefaultInspector();

            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Delete &&
                GUI.GetNameOfFocusedControl() == "Prefab")
            {
                spawner.PrefabUniqueID = "";
                Event.current.Use();
                return;
            }

            GameObject matchingPrefab = FindPrefabByUniqueID(spawner.PrefabUniqueID);
            GUI.SetNextControlName("Prefab");
            GameObject tempDroppedPrefab =
                (GameObject)EditorGUILayout.ObjectField("Prefab", matchingPrefab, typeof(GameObject), false);

            if (matchingPrefab)
            {
                string assetPath = AssetDatabase.GetAssetPath(matchingPrefab);
                AssetImporter importer = AssetImporter.GetAtPath(assetPath);

                if (string.IsNullOrEmpty(importer.assetBundleName))
                {
                    EditorGUILayout.HelpBox("The linked prefab is not assigned to any asset bundles!",
                        MessageType.Warning);
                }
                else
                {
                    EditorGUILayout.Foldout(true, "Asset Bundle Info", true);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.LabelField("Asset Bundle:", importer.assetBundleName);
                    EditorGUI.indentLevel--;
                }
            }


            if (tempDroppedPrefab)
            {
                PrefabUniqueIdentifier identifier = tempDroppedPrefab.GetComponent<PrefabUniqueIdentifier>();

                if (!identifier)
                {

                    EditorGUILayout.HelpBox("This prefab does not have a PrefabUniqueIdentifier!", MessageType.Warning);


                    bool userWantsToAdd = EditorUtility.DisplayDialog("Missing PrefabUniqueIdentifier",
                        "Do you want to add a PrefabUniqueIdentifier to this prefab?", "OK", "Cancel");

                    if (userWantsToAdd)
                    {
                        identifier = tempDroppedPrefab.AddComponent<PrefabUniqueIdentifier>();
                        identifier.GenerateUniqueID();
                        EditorUtility.SetDirty(tempDroppedPrefab);
                        EditorUtility.DisplayDialog("Missing PrefabUniqueIdentifier",
                            "Component has been added for you! \n\nPlease try again.", "Ok");
                    }
                }
                else
                {
                    spawner.PrefabUniqueID = identifier.UniqueID;
                }
            }

            if (GUILayout.Button("Locate Prefab in Project") && matchingPrefab)
            {
                Selection.activeObject = matchingPrefab;
                EditorGUIUtility.PingObject(matchingPrefab);
            }
        }

        private GameObject FindPrefabByUniqueID(string id)
        {
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
}