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
        /// 将消息列表保存到 JSON 文件
        /// </summary>
        /// <param name="messages">要保存的消息列表</param>
        public static void SaveMessages(List<Message> messages)
        {
            // 调试信息
            Debug.Log($"Saving messages to: {messagesFilePath}");

            string json = UnityEngine.JsonUtility.ToJson(new MessageList(messages));
            File.WriteAllText(messagesFilePath, json);
        }

        /// <summary>
        /// 从 JSON 文件中加载消息列表
        /// </summary>
        /// <returns>消息列表</returns>
        public static List<Message> LoadMessages()
        {
            // 调试信息
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
        /// 清除存储的消息文件，Debug用，可删除
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