#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace SagoMini
{
    [CustomEditor(typeof(AssetSpawner))]
    public class AssetSpawnerEditor : Editor
    {
        private string cachedID = "";
        private GameObject cahcedGameObject;
        private bool shouldSerialize = false;

        public override void OnInspectorGUI()
        {
            AssetSpawner assetSpawner = (AssetSpawner)target;

            DrawDefaultInspector();

            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Delete &&
                GUI.GetNameOfFocusedControl() == "Prefab")
            {
                assetSpawner.PrefabUniqueID = "";
                Event.current.Use();
                return;
            }

            GameObject matchingPrefab = FindPrefabByUniqueID(assetSpawner.PrefabUniqueID);

            GUI.SetNextControlName("Prefab");
            GameObject tempDroppedPrefab =
                (GameObject)EditorGUILayout.ObjectField("Prefab", matchingPrefab, typeof(GameObject), false);

            if (matchingPrefab)
            {
                if (GUILayout.Button("Locate in Project View") && matchingPrefab)
                {
                    Selection.activeObject = matchingPrefab;
                    EditorGUIUtility.PingObject(matchingPrefab);
                }

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


            if (tempDroppedPrefab && IsPrefab(tempDroppedPrefab))
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
                    assetSpawner.PrefabUniqueID = identifier.UniqueID;

                    if (shouldSerialize)
                    {
                        EditorUtility.SetDirty(assetSpawner);
                        shouldSerialize = false;
                    }
                }
            }
        }

        protected GameObject FindPrefabByUniqueID(string id)
        {
            if (id == cachedID)
                return cahcedGameObject;
            string[] allPrefabs = AssetDatabase.FindAssets("t:GameObject", null);

            foreach (string prefab in allPrefabs)
            {
                string path = AssetDatabase.GUIDToAssetPath(prefab);
                GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                PrefabUniqueIdentifier identifier = go.GetComponent<PrefabUniqueIdentifier>();
                if (identifier && identifier.UniqueID == id)
                {
                    cachedID = identifier.UniqueID;
                    cahcedGameObject = go;
                    shouldSerialize = true;
                    return go;
                }
            }

            return null;
        }

        protected bool IsPrefab(GameObject go)
        {
            return !string.IsNullOrEmpty(AssetDatabase.GetAssetPath(go));
        }
    }
#endif
}