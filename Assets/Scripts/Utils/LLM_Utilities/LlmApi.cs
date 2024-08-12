using UnityEngine;
using System.Threading.Tasks;
using Mygame.LlmApi; // 确保使用了正确的命名空间

public class WebApiClientTester : MonoBehaviour
{
    private async void Start()
    {
        // 询问的文本
        string question = "你是谁";

        // 调用 WebApiClient 的静态方法并获取响应
        string response = await WebApiClient.SendRequestAsync(question);

        // 打印响应到 Unity 控制台
        Debug.Log("API 响应:");
        Debug.Log(response);
    }
}