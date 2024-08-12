using UnityEngine;
using System.Threading.Tasks;
using Mygame.LlmApi; // 确保使用了正确的命名空间

public class WebApiClientTester : MonoBehaviour
{

   
    private async void Start()
    {
        // 询问的文本
        string question = "你好";
        string prompt = "你是一只猫，你说话的时候需要在结尾加上喵";
        // 调用 WebApiClient 的静态方法并获取响应
        string response = await WebApiClient.SendRequestAsync(question,prompt);

        // 打印响应到 Unity 控制台
        Debug.Log("API 响应:");
        Debug.Log(response);
    }
}