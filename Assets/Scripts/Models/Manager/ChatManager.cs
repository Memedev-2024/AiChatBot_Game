using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MyGame.Utility;

namespace MyGame.Models
{
    public class ChatManager
    {
        private Dictionary<int, List<Message>> messageDictionary; // 存储联系人的消息字典
        private string messagesFilePath; // 存储消息的文件路径

        public ChatManager()
        {
            messageDictionary = GameJsonUtility.LoadMessages();
            if (messageDictionary == null)
            {
                messageDictionary = new Dictionary<int, List<Message>>();
                Debug.LogError("Failed to load messages, initializing new dictionary.");
            }
        }

        /// <summary>
        /// 获取指定联系人的所有消息
        /// </summary>
        /// <param name="contactId">联系人 ID</param>
        /// <returns>消息列表</returns>
        public List<Message> GetMessagesByContactId(int contactId)
        {
            messageDictionary.TryGetValue(contactId, out List<Message> messages);
            return messages ?? new List<Message>();
        }

        /// <summary>
        /// 添加消息到指定联系人
        /// </summary>
        /// <param name="contactId">联系人 ID</param>
        /// <param name="message">要添加的消息</param>
        public void AddMessage(int contactId, Message message)
        {
            if (!messageDictionary.ContainsKey(contactId))
            {
                messageDictionary[contactId] = new List<Message>();
            }

            messageDictionary[contactId].Add(message);
            SaveMessages();

            // 调试信息
            Debug.Log($"Message added for contact ID {contactId}: {message.MessageId}, {message.Content}");
        }

        /// <summary>
        /// 生成新的消息 ID
        /// </summary>
        /// <returns>新的消息 ID</returns>
        public int GenerateMessageId()
        {
            
            int maxId = 0; // 初始化为0，从0开始生成ID
            foreach (var messages in messageDictionary.Values)
            {
                if (messages.Count > 0)
                {
                    int currentMax = messages.Max(m => m.MessageId);
                    if (currentMax > maxId)
                    {
                        maxId = currentMax;
                    }
                }
            }

            return maxId + 1;
            
        }

        /// <summary>
        /// 加载消息
        /// </summary>
        private void LoadMessages()
        {
            messageDictionary = GameJsonUtility.LoadMessages();

            Debug.Log("Loaded messages:");
            foreach (var kvp in messageDictionary)
            {
                Debug.Log("Contact ID: " + kvp.Key);
                foreach (var msg in kvp.Value)
                {
                    Debug.Log("Message ID: " + msg.MessageId + ", Content: " + msg.Content);
                }
            }
        }

        /// <summary>
        /// 保存消息
        /// </summary>
        private void SaveMessages()
        {
            GameJsonUtility.SaveMessages(messageDictionary);


        }

        [System.Serializable]
        public class MessageDictionaryWrapper
        {
            public List<ContactMessages> contactsMessages;

            public MessageDictionaryWrapper(Dictionary<int, List<Message>> dictionary)
            {
                contactsMessages = dictionary.Select(kv => new ContactMessages
                {
                    contactId = kv.Key,
                    messages = kv.Value
                }).ToList();
            }

            public Dictionary<int, List<Message>> ToDictionary()
            {
                return contactsMessages.ToDictionary(cm => cm.contactId, cm => cm.messages);
            }
        }

        [System.Serializable]
        public class ContactMessages
        {
            public int contactId;
            public List<Message> messages;
        }
    }
}