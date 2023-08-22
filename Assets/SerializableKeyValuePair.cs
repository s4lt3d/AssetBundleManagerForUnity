using System.Collections.Generic;

[System.Serializable]
public class SerializableKeyValuePairList
{
    public List<SerializableKeyValuePair> items;
}

[System.Serializable]
public class SerializableKeyValuePair
{
    public string key;
    public string value;

    public SerializableKeyValuePair(string k, string v)
    {
        key = k;
        value = v;
    }
}