using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 管理聊天消息和互动逻辑
/// </summary>
public class GameChatController : MonoBehaviour
{
    public static ChatController Instance;  // 单例实例，用于全局访问

    public ChatView chatView;  // 聊天视图的引用

    private List<Message> messages = new List<Message>();  // 存储消息的列表
 
    /// <summary>
    /// 发送玩家消息并获取 NPC 的回复
    /// </summary>
    /// <param name="playerMessage">玩家发送的消息</param>
    public void SendMessage(string playerMessage)
    {
        // 创建并显示玩家消息
        Message playerMessageObj = new Message("Player", playerMessage);
        messages.Add(playerMessageObj);
        chatView.DisplayMessage(playerMessageObj);

        // 获取并显示 NPC 的回复
        string npcResponse = GetNpcResponse(playerMessage);
        Message npcMessageObj = new Message("NPC", npcResponse);
        messages.Add(npcMessageObj);
        chatView.DisplayMessage(npcMessageObj);

        // 保存消息到本地存储
        SaveMessages();
    }

    /// <summary>
    /// 模拟获取 NPC 的回复
    /// </summary>
    /// <param name="playerMessage">玩家发送的消息</param>
    /// <returns>NPC 的回复</returns>
    private string GetNpcResponse(string playerMessage)
    {
        // 模拟的 NPC 回复
        return "Debug: " + playerMessage;
    }

    /// <summary>
    /// 将消息保存到本地存储
    /// </summary>
    private void SaveMessages()
    {
        JsonUtility.SaveMessages(messages);
    }

    /// <summary>
    /// 当场景开始时从本地存储加载消息
    /// </summary>
    private void Start()
    {
        messages = JsonUtility.LoadMessages();
        foreach (Message message in messages)
        {
            chatView.DisplayMessage(message);
        }
    }
}
