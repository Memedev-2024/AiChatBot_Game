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
        public static void SaveMessages(Dictionary<int, List<Message>> messages)
        {
 //           Debug.Log($"Saving messages to: {messagesFilePath}");

            string json = JsonUtility.ToJson(new ChatManager.MessageDictionaryWrapper(messages));
  //          Debug.Log($"Serialized JSON: {json}");

            File.WriteAllText(messagesFilePath, json);
 //           Debug.Log("Messages saved successfully.");
        }

        /// <summary>
        /// 从 JSON 文件中加载消息列表
        /// </summary>
        /// <returns>消息列表</returns>
        public static Dictionary<int, List<Message>> LoadMessages()
        {
            Debug.Log($"Loading messages from: {messagesFilePath}");
            if (File.Exists(messagesFilePath))
            {
                string json = File.ReadAllText(messagesFilePath);
//                Debug.Log($"Loaded JSON: {json}");

                if (string.IsNullOrEmpty(json))
                {
                    Debug.LogWarning("Messages file is empty.");
                    return new Dictionary<int, List<Message>>();
                }

                ChatManager.MessageDictionaryWrapper wrapper = JsonUtility.FromJson<ChatManager.MessageDictionaryWrapper>(json);
                if (wrapper != null)
                {
                    return wrapper.ToDictionary();
                }
                else
                {
                    Debug.LogError("Failed to parse JSON into MessageDictionaryWrapper.");
                }
            }
            else
            {
                Debug.LogWarning("Messages file does not exist.");
            }
            return new Dictionary<int, List<Message>>(); // 返回一个空字典而不是 null
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
    }
}