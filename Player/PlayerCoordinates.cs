using System.Collections;
using DuperMod.UI;
using UnityEngine;

namespace DuperMod.Player;

public class PlayerCoordinates : MonoBehaviour
{
    private bool isMenuActive; // Flag to track if the menu is active
    private MovablePopup movablePopup;
    private GameObject player;

    private void Start()
    {
        StartCoroutine(FindPlayer());
    }

    private void Update()
    {
        // Check if movablePopup has been initialized
        if (movablePopup != null)
            // Detect the Insert key press to toggle the menu visibility
            if (Input.GetKeyDown(KeyCode.Insert) && player != null)
            {
                isMenuActive = !isMenuActive; // Toggle menu active state
                movablePopup.gameObject.SetActive(isMenuActive);

                // Capture or release the mouse cursor based on the menu visibility
                Cursor.visible = isMenuActive;
                Cursor.lockState = isMenuActive ? CursorLockMode.None : CursorLockMode.Locked;

                // Disable/enable inputs globally based on menu state
                ToggleInput(isMenuActive);
            }
    }

    private IEnumerator FindPlayer()
    {
        while (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Plugin.Logger.LogWarning("Player GameObject not found, trying again...");
                yield return new WaitForSeconds(1f);
            }
            else
            {
                Plugin.Logger.LogInfo("Player GameObject found!");
                CreatePopup(); // Moved here to ensure popup is created after finding the player
                movablePopup.Initialize(player); // Initialize the popup with the player
            }
        }
    }

    private void CreatePopup()
    {
        // Ensure the MovablePopup is created only once
        if (movablePopup == null)
        {
            var popupObject = new GameObject("MovablePopup");
            movablePopup = popupObject.AddComponent<MovablePopup>();
            movablePopup.Initialize(player); // Initialize immediately after creation
            popupObject.SetActive(false); // Start with the popup hidden
        }
    }


    private void ToggleInput(bool disable)
    {
        if (disable)
        {
            // Disable player movement and other controls
            // Implement your logic to disable inputs here
            // This could involve setting flags or directly disabling components
        }
        // Enable player movement and other controls
        // Implement your logic to enable inputs here
    }
}