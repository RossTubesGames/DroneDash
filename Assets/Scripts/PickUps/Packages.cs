using UnityEngine;

public class Packages : MonoBehaviour
{
    [SerializeField] private float lifetime = 20f;

    void Start()
    {
        Destroy(gameObject, lifetime); // Destroy this package after 20s
    }
}
