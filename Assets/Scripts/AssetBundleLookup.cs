using System.Collections.Generic;
using UnityEngine.Serialization;

[System.Serializable]
public class AssetBundleLookupSerializableHelper
{
    public List<AssetBundleLookup> items;
}

[System.Serializable]
public class AssetBundleLookup
{
    public string uniqueid;
    public string assetbundle;
    public string assetname;
    public string guid;

    public AssetBundleLookup(string id, string bundle, string name, string unityguid)
    {
        uniqueid = id;
        assetbundle = bundle;
        assetname = name;
        guid = unityguid;
    }
}