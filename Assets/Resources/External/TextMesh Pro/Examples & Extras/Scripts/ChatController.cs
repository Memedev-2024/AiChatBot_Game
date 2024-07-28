using System.Collections.Generic;
using UnityEngine;
using MyGame.Models;
using MyGame.Views;

namespace MyGame.Controllers
{
    public class ChatController : MonoBehaviour
    {
        public ChatView chatView; // 聊天视图的引用
        private ChatManager chatManager; // 聊天管理器
        private ContactManager contactManager; // 联系人管理器

        private int currentContactId; // 当前聊天的联系人 ID

        private void Start()
        {
            // 初始化管理器
            chatManager = new ChatManager();
            contactManager = new ContactManager();

            // 订阅视图事件
            chatView.OnSendMessage += SendMessage;
            chatView.OnLoadMessages += LoadMessages;
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
            List<Message> messages = chatManager.GetMessagesByContactId(contactId);
            chatView.DisplayMessages(messages);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="messageText">消息文本</param>
        private void SendMessage(string messageText)
        {
            if (string.IsNullOrWhiteSpace(messageText)) return;

            // 创建新消息，假设玩家的 ID 是 0
            Message newMessage = new Message(chatManager.GenerateMessageId(), 0, messageText);

            // 添加消息到聊天管理器
            chatManager.AddMessage(currentContactId, newMessage);

            // 添加消息到视图层
            chatView.AddMessage(newMessage);
        }
    }
}