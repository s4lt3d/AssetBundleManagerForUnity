using System;
using System.Collections.Generic;

namespace SaltedGameKit
{
    /// <summary>
    /// Helper class for unity json serializer
    /// </summary>
    [Serializable]
    public class AssetBundleLookupSerializableHelper
    {
        public List<AssetBundleLookup> items;
    }

    /// <summary>
    /// Manifest entries for precomputed asset bundle lookups. 
    /// </summary>
    [Serializable]
    public class AssetBundleLookup
    {
        public string assetbundle;
        public string assetname;
        public string guid;
        public string path;

        public AssetBundleLookup(string bundle, string name, string unityguid, string datapath)
        {
            assetbundle = bundle;
            assetname = name;
            guid = unityguid;
            path = datapath;
        }
    }
}