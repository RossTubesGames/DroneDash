using UnityEngine;

public class ResetArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CountdownTimer timer = other.GetComponent<CountdownTimer>();
        if (timer != null)
        {
            timer.ResetTimer();
        }
    }
}