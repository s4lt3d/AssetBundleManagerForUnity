using UnityEngine;

namespace SaltedGameKit
{
    [System.Serializable]
    public class GUIDReference
    {
        [SerializeField]
        private string guid;
    
        public string Guid { get { return guid; } set { guid = value; } }
    }
}