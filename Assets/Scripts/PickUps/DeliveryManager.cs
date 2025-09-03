using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    [SerializeField] private List<DropZone> sequence = new(); // drag House 1, House 2, ... in order
    private int index = 0;

    public DropZone CurrentZone => index < sequence.Count ? sequence[index] : null;

    public bool IsCurrentZone(DropZone zone) => zone == CurrentZone;

    public void Advance()
    {
        if (index < sequence.Count) index++;
    }

    public bool Completed => index >= sequence.Count;
}