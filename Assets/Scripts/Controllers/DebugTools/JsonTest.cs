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
            new Message("Player", "Hello!"),
            new Message("NPC", "Hi there!"),
            new Message("Player", "How are you?"),
            new Message("NPC", "I'm good, thanks!")
        };

        // 保存消息到 JSON 文件
        MyGame.Utility.JsonUtility.SaveMessages(testMessages);

        // 从 JSON 文件中加载消息
        List<Message> loadedMessages = MyGame.Utility.JsonUtility.LoadMessages();

        // 输出加载的消息到控制台
        foreach (Message message in loadedMessages)
        {
            if (message != null)
            {
                Debug.Log($"Sender: {message.Sender}, Content: {message.Content}");
            }
            else
            {
                Debug.LogWarning("Loaded message is null.");
            }
        }
    }
}
