using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine;

public static class UnityWebRequestExtensions
{
    public static Task<UnityWebRequest> SendWebRequestAsync(this UnityWebRequest request)
    {
        var completionSource = new TaskCompletionSource<UnityWebRequest>();

        request.SendWebRequest().completed += operation =>
        {
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Request successful: {request.url}, Response: {request.downloadHandler.text}");
                completionSource.SetResult(request);
            }
            else
            {
                Debug.LogError($"Request failed: {request.error}, URL: {request.url}, HTTP Status Code: {request.responseCode}");
                completionSource.SetException(new UnityWebRequestException(request));
            }
        };

        return completionSource.Task;
    }
}

public class UnityWebRequestException : System.Exception
{
    public UnityWebRequestException(UnityWebRequest request)
        : base($"Request failed: {request.error}\nURL: {request.url}, HTTP Status Code: {request.responseCode}")
    { }
}
