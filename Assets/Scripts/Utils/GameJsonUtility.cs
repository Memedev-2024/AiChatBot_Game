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
        public static void SaveMessages(Dictionary<int, List<Message>> messages)
        {
 //           Debug.Log($"Saving messages to: {messagesFilePath}");

            string json = JsonUtility.ToJson(new ChatManager.MessageDictionaryWrapper(messages));
  //          Debug.Log($"Serialized JSON: {json}");

            File.WriteAllText(messagesFilePath, json);
 //           Debug.Log("Messages saved successfully.");
        }

        /// <summary>
        /// �� JSON �ļ��м�����Ϣ�б�
        /// </summary>
        /// <returns>��Ϣ�б�</returns>
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
            return new Dictionary<int, List<Message>>(); // ����һ�����ֵ������ null
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
    }
}