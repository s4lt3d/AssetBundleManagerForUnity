# Sago Mini Tech Test

This demo project demonstrates an asset loading system which 

## Class Descriptions

### PrefabUniqueIdentifier
This unique identifier component is intended to offer advantages over Unity's built-in GUIDs, as it is serialized and doesn't get recomputed based on various factors such as the Unity version, environment variations, library issues, or merge conflicts.

### AssetBundleManager (abstract)
Abstract base class for managing asset bundles in Unity. Its design follows the Configuration Pattern, allowing flexibility in the implementation of asset bundle managers. This could range from local, remote, to simulated asset bundles or even a shift to an entirely different system. The architecture ensures that any changes in the way assets are managed will not affect classes that depend on the asset bundle manager.

### LocalAssetBundleManager (concrete)
Concrete class for managing local asset bundles and loading/unloading prefabs using the PrefabUniqueIdentifier. 

Loads the entire asset bundle and tracks references to the load. Unloads the asset bundle when reference count reaches zero. This follows Unity's recommended practice of loading the full asset bundle to help with loading performance. See discussion below for more details. 

### AssetSpawner
Component class which allows for the spawning of a specific prefab during the Start phase of the game object's lifecycle. This prefab is identified using a prefab unique identifier. 

### AssetSpawnerEditor
A custom inspector for the AssetSpawner component. This enhances the user experience in Unity's Editor allowing designers to drag and drop prefabs directly and offers helpful UX features such as warnings for misconfigurations and suggests automatic fixes.

### AssetBundleBuildScript
The AssetBundleBuildScript class builds asset bundles and generate a manifest that maps PrefabUniqueID to precomputed paths for easier loading of asset bundles.

Expects: Resources/Settings/LocalConfig.asset

### AssetBundleBuildPostprocessor
After a build is complete, copies asset bundles to the project's build data directory. Used for testing on Windows only.

### BuildConfiguration 
This scriptable object allows for easy customization and management of various build settings without requiring direct code changes.

### AssetBundleLookup classes
Helper classes for serialize / deserialize the manifest file used for precomputed asset bundle data. 
