using TMPro;  // for UI text
using UnityEngine;

public class PickUpDrop : MonoBehaviour
{
    [Header("Package Settings")]
    [SerializeField] private GameObject packagePrefab;
    [SerializeField] private int maxPackageCount = 5;
    [SerializeField] private float reloadTime = 10f;
    [SerializeField] private Transform spawnPosition;

    [Header("Reload Settings")]
    [SerializeField] private string reloadAreaTag = "Reload";

    [Header("Delivery System")]
    [SerializeField] private DeliveryManager deliveryManager;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI deliveryText;

    private int currentPackageCount;
    private float reloadTimer;
    private bool isReloading;

    private DropZone currentDropZone;
    private int carriedPackageID; // what the drone is currently carrying

    void Start()
    {
        currentPackageCount = maxPackageCount;

        if (deliveryManager != null && deliveryManager.CurrentZone != null)
        {
            carriedPackageID = deliveryManager.CurrentZone.RequiredPackageID;
            UpdateDeliveryUI();
        }
    }

    void Update()
    {
        HandleReloading();

        if (Input.GetKeyDown(KeyCode.F))
        {
            TryDropAtCurrentZone();
        }
    }

    private void TryDropAtCurrentZone()
    {
        if (currentDropZone == null)
        {
            Debug.Log("Not in a drop zone.");
            return;
        }

        // Ensure we’re at the correct zone in sequence
        if (deliveryManager != null && !deliveryManager.IsCurrentZone(currentDropZone))
        {
            Debug.Log($"This is {currentDropZone.ZoneName}, but it’s not the active delivery zone yet.");
            return;
        }

        if (currentDropZone.Fulfilled)
        {
            Debug.Log($"{currentDropZone.ZoneName} is already fulfilled.");
            return;
        }

        if (!currentDropZone.TryDeliver(carriedPackageID))
        {
            Debug.Log($"{currentDropZone.ZoneName} requires package {currentDropZone.RequiredPackageID}, not {carriedPackageID}.");
            return;
        }

        // Zone accepted → drop the package
        DropPackage(spawnPosition.position);
        Debug.Log($"Delivered package {carriedPackageID} to {currentDropZone.ZoneName}.");

        // Advance delivery
        if (deliveryManager != null)
        {
            deliveryManager.Advance();

            if (!deliveryManager.Completed && deliveryManager.CurrentZone != null)
            {
                carriedPackageID = deliveryManager.CurrentZone.RequiredPackageID;
                UpdateDeliveryUI();
            }
            else
            {
                deliveryText.text = "All deliveries complete!";
            }
        }
    }

    public void DropPackage(Vector3 dropPosition)
    {
        if (currentPackageCount > 0)
        {
            GameObject package = Instantiate(packagePrefab, dropPosition, Quaternion.identity);
            Destroy(package, 20f); // package lifetime
            currentPackageCount--;
            Debug.Log($"Dropped package. Remaining: {currentPackageCount}");
        }
        else
        {
            Debug.Log("No packages left. Reload needed.");
        }
    }

    private void HandleReloading()
    {
        if (!isReloading) return;

        reloadTimer -= Time.deltaTime;
        if (reloadTimer <= 0f)
        {
            currentPackageCount = maxPackageCount;
            isReloading = false;
            Debug.Log("Packages reloaded!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(reloadAreaTag) && !isReloading)
        {
            isReloading = true;
            reloadTimer = reloadTime;
            Debug.Log("Reloading started...");
        }

        if (other.TryGetComponent(out DropZone zone))
        {
            currentDropZone = zone;
            Debug.Log($"Entered {zone.ZoneName} (requires package {zone.RequiredPackageID})");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out DropZone zone) && currentDropZone == zone)
        {
            Debug.Log($"Left {zone.ZoneName}");
            currentDropZone = null;
        }
    }

    private void UpdateDeliveryUI()
    {
        if (deliveryManager != null && deliveryManager.CurrentZone != null)
        {
            deliveryText.text = $"Deliver package {carriedPackageID} → {deliveryManager.CurrentZone.ZoneName}";
        }
    }
}
