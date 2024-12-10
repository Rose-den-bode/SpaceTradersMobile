using System.Threading.Tasks;
using UnityEngine.Networking;

public static class UnityWebRequestExtensions
{
    public static Task<UnityWebRequest> SendWebRequestAsync(this UnityWebRequest request)
    {
        var completionSource = new TaskCompletionSource<UnityWebRequest>();

        request.SendWebRequest().completed += operation =>
        {
            if (request.result == UnityWebRequest.Result.Success)
            {
                completionSource.SetResult(request);
            }
            else
            {
                completionSource.SetException(new UnityWebRequestException(request));
            }
        };

        return completionSource.Task;
    }
}

// Custom exception for UnityWebRequest
public class UnityWebRequestException : System.Exception
{
    public UnityWebRequestException(UnityWebRequest request)
        : base($"Request failed: {request.error}\nURL: {request.url}")
    { }
}
