using System;
using System.Collections.Generic;

namespace SagoMini
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
        public string uniqueid;
        public string assetbundle;
        public string assetname;
        public string guid;
        public string path;

        public AssetBundleLookup(string id, string bundle, string name, string unityguid, string datapath)
        {
            uniqueid = id;
            assetbundle = bundle;
            assetname = name;
            guid = unityguid;
            path = datapath;
        }
    }
}