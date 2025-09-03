using UnityEngine;

public class DropZone : MonoBehaviour
{
    [SerializeField] private int requiredPackageID = 1;   // what this zone accepts
    [SerializeField] private string zoneName = "House 1";
    [SerializeField] private bool singleDelivery = true;  // only one package allowed here
    [SerializeField] private DeliveryManager deliveryManager; // assign in Inspector

    private bool fulfilled;

    public int RequiredPackageID => requiredPackageID;
    public string ZoneName => zoneName;
    public bool Fulfilled => fulfilled;

    /// <summary>
    /// Attempt to deliver a package ID to this zone.
    /// Returns true if accepted, false otherwise.
    /// </summary>
    public bool TryDeliver(int packageId)
    {
        if (singleDelivery && fulfilled) return false;        // already completed
        if (packageId != requiredPackageID) return false;     // wrong package

        fulfilled = true;

        // Tell the manager this delivery succeeded
        if (deliveryManager != null)
        {
            deliveryManager.OnSuccessfulDelivery();
        }

        // TODO: visual feedback (e.g., light turns green, UI checkmark, sound)
        return true;
    }
}
