using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI errorMessage;
    public GameObject newAgentPanel;
    private string tokenKey = "AgentToken";

    // Called when the user clicks the 'Set Up New Agent' button
    public void OnSetUpNewAgentClicked()
    {
        Debug.Log("Set up new agent clicked.");
        ClearLocalData();
        ShowNewAgentSetup();
    }

    // Called when the user clicks the 'Continue' button
    public void OnContinueClicked()
    {
        // Retrieve the stored token from PlayerPrefs
        string storedToken = PlayerPrefs.GetString(tokenKey, string.Empty);

        if (string.IsNullOrEmpty(storedToken))
        {
            // If the token is not found, show an error message
            Debug.LogError("No existing agent found. Please set up a new agent.");
            ShowError("No existing agent found. Please set up a new agent.");
        }
        else
        {
            // If the token is found, continue with the existing agent
            Debug.Log($"Continuing with existing agent: {storedToken}");
            LoadGame(storedToken);
        }
    }

    // Display an error message to the player
    private void ShowError(string message)
    {
        Debug.LogError($"Error: {message}");
        errorMessage.text = message;
        errorMessage.gameObject.SetActive(true);
    }

    // Clears the stored token data
    private void ClearLocalData()
    {
        Debug.Log("Clearing local data...");
        PlayerPrefs.DeleteKey(tokenKey);  // Delete the token from PlayerPrefs
        PlayerPrefs.Save();  // Ensure the change is saved
        Debug.Log("Local data cleared.");
    }

    // Show the panel to set up a new agent
    private void ShowNewAgentSetup()
    {
        Debug.Log("Displaying new agent setup panel.");
        newAgentPanel.SetActive(true);  // Show the new agent panel
        this.gameObject.SetActive(false);  // Hide the current panel
    }

    // Load the game with the provided token
    private void LoadGame(string token)
    {
        Debug.Log($"Loading game with token: {token}");
        // Here you can add logic to actually load the game state using the token
    }
}
