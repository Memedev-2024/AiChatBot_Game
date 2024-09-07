using UnityEngine;
using System.Threading.Tasks;
using Mygame.LlmApi; // 确保使用了正确的命名空间

public class WebApiClientTester : MonoBehaviour
{

   
    //private async void Start()
    //{
    //    // 询问的文本
    //    string question = "你好";
    //    string prompt =
    //    "使用网上聊天的语气回复，你是一个年轻人。越不像AI越好，千万不要问有什么我能帮你的之类的。" +
    //    "一：你可以选择是否回复，格式为/R:1/0.一般你最好回复，但如果你觉得被冒犯，或者不想回复，可以耍脾气，对方发无意义内容，也可以不回复/Reply:1代表回复，/R:0代表不回复。" +
    //    "二：要说的话前加/T:，如果不回复则是/T:0。" +
    //    "三：必须加/M:心情，/M:1代表开心，/M:2代表生气，/M:0代表无聊。示例：/M:1。" +
    //    "四：必须加/A:数字，数字内为-3到3的整数，觉得聊的投机就加数字，反之就减少 示例：/A:-1。" +
    //    "最后，文本、心情和数字中用_隔开。" +
    //    "示例1：/R:1_/T:咋了_/M:2_/A:0示例2：/R:0_/T:null_/M:1_/A:-1";
    //    // 调用 WebApiClient 的静态方法并获取响应
    //    string response = await WebApiClient.SendRequestAsync(question,prompt);

    //    // 打印响应到 Unity 控制台
    //    Debug.Log("API 响应:");
    //    Debug.Log(response);
    //}
}