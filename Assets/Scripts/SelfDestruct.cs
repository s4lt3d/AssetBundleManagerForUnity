using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float destroyTime = 2;
    void Start()
    {
        Invoke("DestroyThis", destroyTime);
    }

    void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
