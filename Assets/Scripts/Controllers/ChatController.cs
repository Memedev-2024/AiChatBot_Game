using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ����������Ϣ�ͻ����߼�
/// </summary>
public class GameChatController : MonoBehaviour
{
    public static ChatController Instance;  // ����ʵ��������ȫ�ַ���

    public ChatView chatView;  // ������ͼ������

    private List<Message> messages = new List<Message>();  // �洢��Ϣ���б�
 
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
        string npcResponse = GetNpcResponse(playerMessage);
        Message npcMessageObj = new Message("NPC", npcResponse);
        messages.Add(npcMessageObj);
        chatView.DisplayMessage(npcMessageObj);

        // ������Ϣ�����ش洢
        SaveMessages();
    }

    /// <summary>
    /// ģ���ȡ NPC �Ļظ�
    /// </summary>
    /// <param name="playerMessage">��ҷ��͵���Ϣ</param>
    /// <returns>NPC �Ļظ�</returns>
    private string GetNpcResponse(string playerMessage)
    {
        // ģ��� NPC �ظ�
        return "Debug: " + playerMessage;
    }

    /// <summary>
    /// ����Ϣ���浽���ش洢
    /// </summary>
    private void SaveMessages()
    {
        JsonUtility.SaveMessages(messages);
    }

    /// <summary>
    /// ��������ʼʱ�ӱ��ش洢������Ϣ
    /// </summary>
    private void Start()
    {
        messages = JsonUtility.LoadMessages();
        foreach (Message message in messages)
        {
            chatView.DisplayMessage(message);
        }
    }
}
