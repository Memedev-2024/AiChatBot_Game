using UnityEngine;
using System.Collections.Generic;
using MyGame.Models;

/// <summary>
/// ���� JSON ���л��ͷ����л�����
/// </summary>
public class TestJsonUtility : MonoBehaviour
{
    void Start()
    {
        // ����һ��������Ϣ�б�
        List<Message> testMessages = new List<Message>
        {
            new Message(),

        };

        // ������Ϣ�� JSON �ļ�
        MyGame.Utility.GameJsonUtility.SaveMessages(testMessages);

        // �� JSON �ļ��м�����Ϣ
        List<Message> loadedMessages = MyGame.Utility.GameJsonUtility.LoadMessages();

        // ������ص���Ϣ������̨
        foreach (Message message in loadedMessages)
        {
            if (message != null)
            {
                Debug.Log($"Sender: {message.SenderId}, Content: {message.Content}");
            }
            else
            {
                Debug.LogWarning("Loaded message is null.");
            }
        }
    }
}
