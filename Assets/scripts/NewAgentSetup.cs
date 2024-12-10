using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Threading.Tasks;

public class NewAgentSetup : MonoBehaviour
{
    public TMP_InputField agentNameInput; // Input field for agent name
    public TextMeshProUGUI errorMessage; // Assign in the inspector
    private string baseUrl = "https://api.spacetraders.io/v2";
    private string tokenKey = "AgentToken";

    // Called when the "Create Agent" button is clicked
    public async void OnCreateAgentClicked()
    {
        string agentName = agentNameInput.text;
        if (string.IsNullOrEmpty(agentName))
        {
            ShowError("Agent name cannot be empty.");
            return;
        }

        string response = await CreateAgentAsync(agentName);
        if (!string.IsNullOrEmpty(response))
        {
            Debug.Log("Agent created successfully.");
            SaveToken(response);
            // Proceed to the main game scene
            // SceneManager.LoadScene("GameScene");
        }
    }

    // Create a new agent
    private async Task<string> CreateAgentAsync(string agentName)
    {
        string url = $"{baseUrl}/register";
        WWWForm form = new WWWForm();
        form.AddField("name", agentName);

        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            try
            {
                await request.SendWebRequestAsync(); // Use the extension method here

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Response: " + request.downloadHandler.text);
                    return ExtractToken(request.downloadHandler.text);
                }
                else
                {
                    ShowError($"Error: {request.error}");
                    return null;
                }
            }
            catch (UnityWebRequestException ex)
            {
                ShowError(ex.Message);
                return null;
            }
        }
    }

    // Display error messages
    private void ShowError(string message)
    {
        errorMessage.text = message;
        errorMessage.gameObject.SetActive(true);
    }

    // Save token locally
    private void SaveToken(string token)
    {
        PlayerPrefs.SetString(tokenKey, token);
        PlayerPrefs.Save();
        Debug.Log("Token saved locally.");
    }

    // Extract token from API response (adjust based on API structure)
    private string ExtractToken(string jsonResponse)
    {
        // Example parsing; use JsonUtility or Newtonsoft.Json
        // Replace with actual response parsing logic
        return "example-token";
    }
}
