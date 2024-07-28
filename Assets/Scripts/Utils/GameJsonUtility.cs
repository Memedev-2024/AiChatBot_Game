using UnityEngine;
using System.IO;
using System.Collections.Generic;

using MyGame.Models;


namespace MyGame.Utility
{
    public static class GameJsonUtility
    {
        private static string messagesFilePath = Application.persistentDataPath + "/messages.json";

        /// <summary>
        /// ����Ϣ�б��浽 JSON �ļ�
        /// </summary>
        /// <param name="messages">Ҫ�������Ϣ�б�</param>
        public static void SaveMessages(List<Message> messages)
        {
            // ������Ϣ
            Debug.Log($"Saving messages to: {messagesFilePath}");

            string json = UnityEngine.JsonUtility.ToJson(new MessageList(messages));
            File.WriteAllText(messagesFilePath, json);
        }

        /// <summary>
        /// �� JSON �ļ��м�����Ϣ�б�
        /// </summary>
        /// <returns>��Ϣ�б�</returns>
        public static List<Message> LoadMessages()
        {
            // ������Ϣ
            Debug.Log($"Loading messages from: {messagesFilePath}");
            if (File.Exists(messagesFilePath))
            {
                string json = File.ReadAllText(messagesFilePath);
                MessageList messageList = UnityEngine.JsonUtility.FromJson<MessageList>(json);
                return messageList.messages;
            }
            else
            {
                Debug.LogWarning("Messages file does not exist.");
            }
            return new List<Message>();
        }

        /// <summary>
        /// ����洢����Ϣ�ļ���Debug�ã���ɾ��
        /// </summary>
        public static void ClearMessages()
        {
            if (File.Exists(messagesFilePath))
            {
                File.Delete(messagesFilePath);
                Debug.Log("Messages file cleared.");
            }
            else
            {
                Debug.LogWarning("Messages file does not exist.");
            }
        }

        [System.Serializable]
        private class MessageList
        {
            public List<Message> messages;

            public MessageList(List<Message> messages)
            {
                this.messages = messages;
            }
        }
    }
}