using UnityEngine;
using System.Collections.Generic;
using MyGame.Models;
using MyGame.Views;
using MyGame.Utility;

public class GameChatController : MonoBehaviour
{
    public static GameChatController Instance;  // 单例实例，用于全局访问

    public ChatView chatView;  // 聊天视图的引用
    public NPCMessageModel npcMessageModel;  // NPC 消息生成模型的引用

    private List<Message> messages = new List<Message>();  // 存储消息的列表

    private void Awake()
    {
        // 确保实例唯一性
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
        if (npcMessageModel != null)
        {
            string npcResponse = npcMessageModel.GetNPCMessage("NPC_ID", playerMessage);  // 这里的 "NPC_ID" 需要根据实际情况替换
            Message npcMessageObj = new Message("NPC", npcResponse);
            messages.Add(npcMessageObj);
            chatView.DisplayMessage(npcMessageObj);
        }
        else
        {
            Debug.LogError("NPCMessage model is not set.");
        }

        // 保存消息到本地存储
        SaveMessages();
    }

    private void SaveMessages()
    {
        MyGame.Utility.JsonUtility.SaveMessages(messages);
    }

    /// <summary>
    /// Debug用，可删除，按F5清除聊天记录
    /// </summary>
    public void ClearChatHistory()
    {
        messages.Clear();  // 清除消息列表
        chatView.ClearChat();  // 更新视图以反映清除
        MyGame.Utility.JsonUtility.ClearMessages();  // 清除本地存储中的消息
    }

    private void Start()
    {
        messages = MyGame.Utility.JsonUtility.LoadMessages();
        foreach (Message message in messages)
        {
            chatView.DisplayMessage(message);
        }
    }
}
