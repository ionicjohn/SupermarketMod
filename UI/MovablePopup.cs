using UnityEngine;
using UnityEngine.AI;

namespace DuperMod.UI;

public class MovablePopup : MonoBehaviour
{
    private Vector2 dragOffset;
    private string inputDay = "";
    private string inputFranchiseExperience = "";
    private string inputFranchiseLevel = "";
    private string inputFranchisePoints = "";

    // Fields for modifying GameData values
    private string inputFunds = "";

    // New field for NPC speed input
    private string inputNPCSpeed = "";
    private string inputX = "", inputY = "", inputZ = "";
    private bool isDragging = false;
    private GameObject player;
    private Rect windowRect = new(10, 10, 300, 500); // Adjusted size for added content

    private void OnGUI()
    {
        windowRect = GUILayout.Window(0, windowRect, DrawWindow, "Player Menu");
    }

    // Initialize the popup with a reference to the player
    public void Initialize(GameObject player)
    {
        this.player = player;
    }

    private void DrawWindow(int windowID)
    {
        // Display current player coordinates
        if (player != null)
        {
            var playerPosition = player.transform.position;
            GUILayout.Label($"X: {playerPosition.x:F2}");
            GUILayout.Label($"Y: {playerPosition.y:F2}");
            GUILayout.Label($"Z: {playerPosition.z:F2}");

            // Display and modify GameData values
            if (GameData.Instance != null)
            {
                GUILayout.Label($"Funds: ${GameData.Instance.gameFunds:F2}");
                inputFunds = GUILayout.TextField(inputFunds, 50); // Input field for game funds

                GUILayout.Label($"Franchise Points: {GameData.Instance.gameFranchisePoints}");
                inputFranchisePoints =
                    GUILayout.TextField(inputFranchisePoints, 50); // Input field for franchise points

                GUILayout.Label($"Current Day: {GameData.Instance.gameDay}");
                inputDay = GUILayout.TextField(inputDay, 50); // Input field for current day

                GUILayout.Label($"Franchise Level: {GameData.Instance.gameFranchiseLevel}");
                inputFranchiseLevel = GUILayout.TextField(inputFranchiseLevel, 50); // Input field for franchise level

                GUILayout.Label($"Franchise Experience: {GameData.Instance.gameFranchiseExperience}");
                inputFranchiseExperience =
                    GUILayout.TextField(inputFranchiseExperience, 50); // Input field for franchise experience

                GUILayout.Label("NPC Speed Multiplier:");
                inputNPCSpeed = GUILayout.TextField(inputNPCSpeed, 50); // Input field for NPC speed
            }
            else
            {
                GUILayout.Label("GameData not initialized.");
            }

            // Button to apply modifications
            if (GUILayout.Button("Apply Changes")) ApplyChanges();

            // Input fields for new coordinates
            GUILayout.Label("New X:");
            inputX = GUILayout.TextField(inputX, 50);
            GUILayout.Label("New Y:");
            inputY = GUILayout.TextField(inputY, 50);
            GUILayout.Label("New Z:");
            inputZ = GUILayout.TextField(inputZ, 50);

            // Teleport button
            if (GUILayout.Button("Teleport")) TeleportPlayer();

            // Close button to hide the menu
            if (GUILayout.Button("Close Menu")) gameObject.SetActive(false);

            // Allow window dragging
            GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }
    }

    private void ModifyNPCSpeed(float npcSpeedMultiplier)
    {
        if (NPC_Manager.Instance != null)
        {
            // Modify the speed of all NPCs
            foreach (Transform npc in NPC_Manager.Instance.customersnpcParentOBJ.transform)
            {
                var agent = npc.GetComponent<NavMeshAgent>();
                if (agent != null) agent.speed *= npcSpeedMultiplier; // Apply the speed multiplier
            }

            foreach (Transform npc in NPC_Manager.Instance.employeeParentOBJ.transform)
            {
                var agent = npc.GetComponent<NavMeshAgent>();
                if (agent != null) agent.speed *= npcSpeedMultiplier; // Apply the speed multiplier
            }

            Plugin.Logger.LogInfo($"NPC speed multiplier set to {npcSpeedMultiplier}");
        }
        else
        {
            Plugin.Logger.LogWarning("NPC_Manager instance not found.");
        }
    }


    private void ApplyChanges()
    {
        // Update GameData values based on user input
        if (GameData.Instance != null)
        {
            if (float.TryParse(inputFunds, out var funds))
                GameData.Instance.gameFunds = funds; // Update game funds
            else
                Plugin.Logger.LogWarning("Invalid input for game funds");

            if (int.TryParse(inputFranchisePoints, out var franchisePoints))
                GameData.Instance.gameFranchisePoints = franchisePoints; // Update franchise points
            else
                Plugin.Logger.LogWarning("Invalid input for franchise points");

            if (int.TryParse(inputDay, out var day))
                GameData.Instance.gameDay = day; // Update game day
            else
                Plugin.Logger.LogWarning("Invalid input for current day");

            if (int.TryParse(inputFranchiseLevel, out var franchiseLevel))
                GameData.Instance.gameFranchiseLevel = franchiseLevel; // Update franchise level
            else
                Plugin.Logger.LogWarning("Invalid input for franchise level");

            if (int.TryParse(inputFranchiseExperience, out var franchiseExperience))
                GameData.Instance.gameFranchiseExperience = franchiseExperience; // Update franchise experience
            else
                Plugin.Logger.LogWarning("Invalid input for franchise experience");

            if (float.TryParse(inputNPCSpeed, out var npcSpeedMultiplier))
                ModifyNPCSpeed(npcSpeedMultiplier); // Call a method to apply the speed change
            else
                Plugin.Logger.LogWarning("Invalid input for NPC speed multiplier");

            Plugin.Logger.LogInfo("GameData values updated successfully.");
        }
        else
        {
            Plugin.Logger.LogWarning("GameData not initialized.");
        }
    }

    private void TeleportPlayer()
    {
        if (float.TryParse(inputX, out var x) &&
            float.TryParse(inputY, out var y) &&
            float.TryParse(inputZ, out var z))
        {
            var newPosition = new Vector3(x, y, z);
            var controller = player.GetComponent<CharacterController>();

            if (controller != null)
            {
                // Disable the controller temporarily if necessary
                controller.enabled = false;

                // Move the player to the new position
                player.transform.position = newPosition;

                // Re-enable the controller
                controller.enabled = true;

                Plugin.Logger.LogInfo($"Teleported player to {newPosition}");
            }
            else
            {
                Plugin.Logger.LogError("CharacterController not found on the player!");
            }
        }
        else
        {
            Plugin.Logger.LogWarning("Invalid coordinate input for teleportation");
        }
    }
}