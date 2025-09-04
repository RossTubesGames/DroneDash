using UnityEngine;
using TMPro; // only needed if you use TextMeshPro for text

public class PopUpManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject popUpPanel;   // your PopUpPanel (with background image set in the Inspector)
    [SerializeField] private TMP_Text popUpText;      // optional: assign text if you want to show messages

    [Header("Settings")]
    [SerializeField] private float displayTime = 3f;  // how long the pop-up shows

    private void Start()
    {
        if (popUpPanel != null)
            popUpPanel.SetActive(false); // start hidden
    }

    private void Update()
    {
        // TEST: show pop-up when pressing F
        if (Input.GetKeyDown(KeyCode.F))
        {
            ShowPopUp("Thank you for the delivery!");
        }
    }

    public void ShowPopUp(string message = "")
    {
        if (popUpPanel == null) return;

        if (popUpText != null && !string.IsNullOrEmpty(message))
            popUpText.text = message;

        popUpPanel.SetActive(true);

        CancelInvoke(nameof(HidePopUp));
        Invoke(nameof(HidePopUp), displayTime);
    }

    private void HidePopUp()
    {
        if (popUpPanel != null)
            popUpPanel.SetActive(false);
    }
}
