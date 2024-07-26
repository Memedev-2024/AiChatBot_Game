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
            new Message("Player", "Hello!"),
            new Message("NPC", "Hi there!"),
            new Message("Player", "How are you?"),
            new Message("NPC", "I'm good, thanks!")
        };

        // ������Ϣ�� JSON �ļ�
        MyGame.Utility.JsonUtility.SaveMessages(testMessages);

        // �� JSON �ļ��м�����Ϣ
        List<Message> loadedMessages = MyGame.Utility.JsonUtility.LoadMessages();

        // ������ص���Ϣ������̨
        foreach (Message message in loadedMessages)
        {
            if (message != null)
            {
                Debug.Log($"Sender: {message.Sender}, Content: {message.Content}");
            }
            else
            {
                Debug.LogWarning("Loaded message is null.");
            }
        }
    }
}
