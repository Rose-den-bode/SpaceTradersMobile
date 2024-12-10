using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI errorMessage; // Assign in the inspector
    public GameObject newAgentPanel;    // Panel for setting up a new agent
    private string tokenKey = "AgentToken";

    // Called when the "Set Up New Agent" button is clicked
    public void OnSetUpNewAgentClicked()
    {
        ClearLocalData();
        ShowNewAgentSetup();
    }

    // Called when the "Continue" button is clicked
    public void OnContinueClicked()
    {
        string storedToken = PlayerPrefs.GetString(tokenKey, string.Empty);
        if (string.IsNullOrEmpty(storedToken))
        {
            ShowError("No existing agent found. Please set up a new agent.");
        }
        else
        {
            // Proceed to the main game scene
            Debug.Log("Continuing with existing agent...");
            LoadGame(storedToken);
        }
    }

    // Display error messages
    private void ShowError(string message)
    {
        errorMessage.text = message;
        errorMessage.gameObject.SetActive(true);
    }

    // Clear local data
    private void ClearLocalData()
    {
        PlayerPrefs.DeleteKey(tokenKey);
        PlayerPrefs.Save();
        Debug.Log("Local data cleared.");
    }

    // Show the new agent setup panel
    private void ShowNewAgentSetup()
    {
        newAgentPanel.SetActive(true);
        this.gameObject.SetActive(false); // Hide main menu
    }

    // Load the main game scene
    private void LoadGame(string token)
    {
        Debug.Log($"Loading game with token: {token}");
        // Add logic to load the next scene or gameplay
        // e.g., SceneManager.LoadScene("GameScene");
    }
}
