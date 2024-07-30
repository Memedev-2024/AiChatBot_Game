using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MyGame.Utility;

namespace MyGame.Models
{
    public class ChatManager
    {
        private Dictionary<int, List<Message>> messageDictionary; // �洢��ϵ�˵���Ϣ�ֵ�
        private string messagesFilePath; // �洢��Ϣ���ļ�·��

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
        /// ��ȡָ����ϵ�˵�������Ϣ
        /// </summary>
        /// <param name="contactId">��ϵ�� ID</param>
        /// <returns>��Ϣ�б�</returns>
        public List<Message> GetMessagesByContactId(int contactId)
        {
            messageDictionary.TryGetValue(contactId, out List<Message> messages);
            return messages ?? new List<Message>();
        }

        /// <summary>
        /// �����Ϣ��ָ����ϵ��
        /// </summary>
        /// <param name="contactId">��ϵ�� ID</param>
        /// <param name="message">Ҫ��ӵ���Ϣ</param>
        public void AddMessage(int contactId, Message message)
        {
            if (!messageDictionary.ContainsKey(contactId))
            {
                messageDictionary[contactId] = new List<Message>();
            }

            messageDictionary[contactId].Add(message);
            SaveMessages();

            // ������Ϣ
            Debug.Log($"Message added for contact ID {contactId}: {message.MessageId}, {message.Content}");
        }

        /// <summary>
        /// �����µ���Ϣ ID
        /// </summary>
        /// <returns>�µ���Ϣ ID</returns>
        public int GenerateMessageId()
        {
            
            int maxId = 0; // ��ʼ��Ϊ0����0��ʼ����ID
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
        /// ������Ϣ
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
        /// ������Ϣ
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