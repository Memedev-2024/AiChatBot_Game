using UnityEngine;
using System.Collections.Generic;
using MyGame.Models;
using MyGame.Views;
using MyGame.Utility;

/// <summary>
/// 管理聊天消息和互动逻辑
/// </summary>
public class ChatController : MonoBehaviour
{
    public static ChatController Instance;  // 单例实例，用于全局访问
    public ChatView chatView;  // 聊天视图的引用

    private List<Message> messages = new List<Message>();  // 存储消息的列表
    private int currentContactId; // 当前聊天的联系人 ID
    private int playerId = 1;

    private MyGame.Models.ChatManager chatManager; // 聊天管理器
    private MyGame.Models.ContactManager contactManager; // 联系人管理器
    /// <summary>
    /// 发送玩家消息并获取 NPC 的回复
    /// </summary>
    /// <param name="playerMessage">玩家发送的消息</param>
    public void GSendMessage(string playerMessage)
    {
        print("SendedSuccess");
        // 创建并显示玩家消息
        Message playerMessageObj = new Message(1, playerId, playerMessage);
        messages.Add(playerMessageObj);
        chatView.DisplayMessage(playerMessageObj);

        // 获取并显示 NPC 的回复
        string npcResponse = GetNpcResponse(playerMessage);
        Message npcMessageObj = new Message(2, currentContactId,npcResponse);
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
        GameJsonUtility.SaveMessages(messages);
    }

    /// <summary>
    /// 当场景开始时从本地存储加载消息
    /// </summary>
    private void Start()
    {
        // 初始化管理器
        chatManager = new MyGame.Models.ChatManager();
        contactManager = new MyGame.Models.ContactManager();

        // 订阅视图事件
        if (chatView != null)
        {
            chatView.OnSendMessage += GSendMessage;
            chatView.OnLoadMessages += LoadMessages;
            Debug.Log("OnSendMessage event subscribed");
        }
        else
        {
            Debug.LogError("ChatView instance not found.");
        }
    }
    /// <summary>
    /// 设置当前聊天的联系人 ID
    /// </summary>
    /// <param name="contactId">联系人 ID</param>
    public void SetCurrentContactId(int contactId)
        {
            currentContactId = contactId;
            LoadMessages(contactId);
        }
    /// <summary>
    /// 加载并显示指定联系人的消息记录
    /// </summary>
    /// <param name="contactId">联系人 ID</param>
    private void LoadMessages(int contactId)
    {
        List<MyGame.Models.Message> messages = chatManager.GetMessagesByContactId(contactId);
        chatView.DisplayMessages(messages);
    }

}
