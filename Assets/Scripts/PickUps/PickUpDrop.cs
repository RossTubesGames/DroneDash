using UnityEngine;

public class PickUpDrop : MonoBehaviour
{
    [Header("Package Settings")]
    [SerializeField] private GameObject packagePrefab;
    [SerializeField] private int maxPackageCount = 5;
    [SerializeField] private float reloadTime = 10f;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private int carriedPackageID = 1; // what the drone is currently carrying

    [Header("Reload Settings")]
    [SerializeField] private string reloadAreaTag = "Reload";

    private int currentPackageCount;
    private float reloadTimer;
    private bool isReloading;

    private DropZone currentDropZone; // zone we are inside

    void Start()
    {
        currentPackageCount = maxPackageCount;
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

        // Prevent repeat deliveries to the same zone
        if (currentDropZone.Fulfilled)
        {
            Debug.Log($"{currentDropZone.ZoneName} is already fulfilled.");
            return;
        }

        // Check that this zone accepts our carried package type/ID
        if (!currentDropZone.TryDeliver(carriedPackageID))
        {
            Debug.Log($"{currentDropZone.ZoneName} requires package {currentDropZone.RequiredPackageID}, not {carriedPackageID}.");
            return;
        }

        // Zone accepted the delivery → actually spawn & consume one package
        DropPackage(spawnPosition.position);
        Debug.Log($"Delivered to {currentDropZone.ZoneName}. This zone is now complete.");
    }

    public void DropPackage(Vector3 dropPosition)
    {
        if (currentPackageCount > 0)
        {
            GameObject package = Instantiate(packagePrefab, dropPosition, Quaternion.identity);
            Destroy(package, 20f); // your 20s lifetime
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
}
