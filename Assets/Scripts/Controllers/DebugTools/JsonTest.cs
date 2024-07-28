using UnityEngine;
using System.Collections.Generic;
using MyGame.Models;

/// <summary>
/// 测试 JSON 序列化和反序列化功能
/// </summary>
public class TestJsonUtility : MonoBehaviour
{
    void Start()
    {
        // 创建一个测试消息列表
        List<Message> testMessages = new List<Message>
        {
            new Message(),

        };

        // 保存消息到 JSON 文件
        MyGame.Utility.GameJsonUtility.SaveMessages(testMessages);

        // 从 JSON 文件中加载消息
        List<Message> loadedMessages = MyGame.Utility.GameJsonUtility.LoadMessages();

        // 输出加载的消息到控制台
        foreach (Message message in loadedMessages)
        {
            if (message != null)
            {
                Debug.Log($"Sender: {message.SenderId}, Content: {message.Content}");
            }
            else
            {
                Debug.LogWarning("Loaded message is null.");
            }
        }
    }
}
