using UnityEngine;
using System.Collections.Generic;
using MyGame.Models;
using MyGame.Views;
using MyGame.Utility;

public class GameChatController : MonoBehaviour
{
    public static GameChatController Instance;  // ����ʵ��������ȫ�ַ���

    public ChatView chatView;  // ������ͼ������
    public NPCMessageModel npcMessageModel;  // NPC ��Ϣ����ģ�͵�����

    private List<Message> messages = new List<Message>();  // �洢��Ϣ���б�

    private void Awake()
    {
        // ȷ��ʵ��Ψһ��
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
    /// ���������Ϣ����ȡ NPC �Ļظ�
    /// </summary>
    /// <param name="playerMessage">��ҷ��͵���Ϣ</param>
    public void SendMessage(string playerMessage)
    {
        // ��������ʾ�����Ϣ
        Message playerMessageObj = new Message("Player", playerMessage);
        messages.Add(playerMessageObj);
        chatView.DisplayMessage(playerMessageObj);

        // ��ȡ����ʾ NPC �Ļظ�
        if (npcMessageModel != null)
        {
            string npcResponse = npcMessageModel.GetNPCMessage("NPC_ID", playerMessage);  // ����� "NPC_ID" ��Ҫ����ʵ������滻
            Message npcMessageObj = new Message("NPC", npcResponse);
            messages.Add(npcMessageObj);
            chatView.DisplayMessage(npcMessageObj);
        }
        else
        {
            Debug.LogError("NPCMessage model is not set.");
        }

        // ������Ϣ�����ش洢
        SaveMessages();
    }

    private void SaveMessages()
    {
        MyGame.Utility.JsonUtility.SaveMessages(messages);
    }

    /// <summary>
    /// Debug�ã���ɾ������F5��������¼
    /// </summary>
    public void ClearChatHistory()
    {
        messages.Clear();  // �����Ϣ�б�
        chatView.ClearChat();  // ������ͼ�Է�ӳ���
        MyGame.Utility.JsonUtility.ClearMessages();  // ������ش洢�е���Ϣ
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
