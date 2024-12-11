using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using System.Text;

public class SpaceTradersAgentRegistration : MonoBehaviour
{
    // UI Elements
    [SerializeField] private TMP_InputField agentSymbolInput; // UI Input field for agent symbol
    [SerializeField] private TMP_InputField factionInput; // UI Input field for faction
    [SerializeField] private TextMeshProUGUI errorMessageText; // UI Text for displaying error messages
    [SerializeField] private TextMeshProUGUI successMessageText; // UI Text for displaying success messages

    // Private fields for agent symbol and faction
    private string agentSymbol;
    private string faction;
    private string tokenKey = "AgentToken"; // Key for storing token in PlayerPrefs

    // Called when the registration button is clicked
    public void OnRegisterButtonClicked()
    {
        // Get the input values from the UI
        agentSymbol = agentSymbolInput.text;
        faction = factionInput.text; // Faction can be empty now

        // Validate the agent symbol input
        if (string.IsNullOrEmpty(agentSymbol))
        {
            ShowError("Agent Symbol is required.");
            return;
        }

        Debug.Log($"Starting agent registration process with symbol: {agentSymbol} and faction: {faction}");
        StartCoroutine(RegisterAgent());
    }

    private IEnumerator RegisterAgent()
    {
        string url = "https://api.spacetraders.io/v2/register";
        string jsonData = JsonUtility.ToJson(new AgentRegistrationData(agentSymbol, faction));
        byte[] postData = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            // Set up the request details
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log($"Sending registration request to {url} with data: {jsonData}");

            // Send the request and wait for a response
            yield return request.SendWebRequest();

            // Check if the request was successful
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Registration successful. Response: {request.downloadHandler.text}");

                // Extract and save the token
                string token = ExtractToken(request.downloadHandler.text);
                if (!string.IsNullOrEmpty(token))
                {
                    SaveToken(token);
                    ShowSuccess("Registration successful! Agent created.");
                }
                else
                {
                    ShowError("Failed to extract token from the response.");
                }
            }
            else
            {
                // Log the error and show it on the UI
                Debug.LogError($"Registration failed. Error: {request.error}, HTTP Status Code: {request.responseCode}");
                Debug.LogError($"Response Body: {request.downloadHandler.text}");
                ShowError($"Registration failed: {request.error}");
            }
        }
    }

    private string ExtractToken(string jsonResponse)
    {
        // Example of extracting token from JSON response
        // Assuming the response contains a "token" field. Modify if needed.
        var response = JsonUtility.FromJson<AgentResponse>(jsonResponse);
        if (response != null && !string.IsNullOrEmpty(response.token))
        {
            Debug.Log("Token extracted successfully.");
            return response.token;
        }
        else
        {
            Debug.LogError("Failed to extract token from the response.");
            return null;
        }
    }

    private void SaveToken(string token)
    {
        Debug.Log("Saving token to PlayerPrefs.");
        PlayerPrefs.SetString(tokenKey, token);  // Save the token to PlayerPrefs
        PlayerPrefs.Save();  // Ensure the token is saved immediately
        Debug.Log("Token saved successfully.");
    }

    private void ShowError(string message)
    {
        Debug.LogError($"Error: {message}");
        errorMessageText.text = message;
        errorMessageText.gameObject.SetActive(true);
        successMessageText.gameObject.SetActive(false);
    }

    private void ShowSuccess(string message)
    {
        Debug.Log($"Success: {message}");
        successMessageText.text = message;
        successMessageText.gameObject.SetActive(true);
        errorMessageText.gameObject.SetActive(false);
    }

    [System.Serializable]
    private class AgentRegistrationData
    {
        public string symbol;
        public string faction;

        // Constructor to set the agent data
        public AgentRegistrationData(string symbol, string faction)
        {
            this.symbol = symbol;
            this.faction = faction ?? ""; // Ensure that faction is never null
        }
    }

    // Response structure for token extraction
    [System.Serializable]
    private class AgentResponse
    {
        public string token;
    }
}
