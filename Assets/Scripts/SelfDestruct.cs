using UnityEngine;

namespace SagoMini
{
    public class SelfDestruct : MonoBehaviour
    {
        public float destroyTime = 2;

        private void Start()
        {
            Invoke("DestroyThis", destroyTime);
        }

        private void DestroyThis()
        {
            Destroy(gameObject);
        }
    }
}