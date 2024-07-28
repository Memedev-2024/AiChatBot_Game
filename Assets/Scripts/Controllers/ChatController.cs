using UnityEngine;
using System.Collections.Generic;
using MyGame.Models;
using MyGame.Views;
using MyGame.Utility;

/// <summary>
/// ����������Ϣ�ͻ����߼�
/// </summary>
public class ChatController : MonoBehaviour
{
    public static ChatController Instance;  // ����ʵ��������ȫ�ַ���
    public ChatView chatView;  // ������ͼ������

    private List<Message> messages = new List<Message>();  // �洢��Ϣ���б�
    private int currentContactId; // ��ǰ�������ϵ�� ID
    private int playerId = 1;

    private MyGame.Models.ChatManager chatManager; // ���������
    private MyGame.Models.ContactManager contactManager; // ��ϵ�˹�����
    /// <summary>
    /// ���������Ϣ����ȡ NPC �Ļظ�
    /// </summary>
    /// <param name="playerMessage">��ҷ��͵���Ϣ</param>
    public void GSendMessage(string playerMessage)
    {
        print("SendedSuccess");
        // ��������ʾ�����Ϣ
        Message playerMessageObj = new Message(1, playerId, playerMessage);
        messages.Add(playerMessageObj);
        chatView.DisplayMessage(playerMessageObj);

        // ��ȡ����ʾ NPC �Ļظ�
        string npcResponse = GetNpcResponse(playerMessage);
        Message npcMessageObj = new Message(2, currentContactId,npcResponse);
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
        GameJsonUtility.SaveMessages(messages);
    }

    /// <summary>
    /// ��������ʼʱ�ӱ��ش洢������Ϣ
    /// </summary>
    private void Start()
    {
        // ��ʼ��������
        chatManager = new MyGame.Models.ChatManager();
        contactManager = new MyGame.Models.ContactManager();

        // ������ͼ�¼�
        if (chatView != null)
        {
            chatView.OnSendMessage += GSendMessage;
            chatView.OnLoadMessages += LoadMessages;
            Debug.Log("OnSendMessage event subscribed");
        }
        else
        {
            Debug.LogError("ChatView instance not found.");
        }
    }
    /// <summary>
    /// ���õ�ǰ�������ϵ�� ID
    /// </summary>
    /// <param name="contactId">��ϵ�� ID</param>
    public void SetCurrentContactId(int contactId)
        {
            currentContactId = contactId;
            LoadMessages(contactId);
        }
    /// <summary>
    /// ���ز���ʾָ����ϵ�˵���Ϣ��¼
    /// </summary>
    /// <param name="contactId">��ϵ�� ID</param>
    private void LoadMessages(int contactId)
    {
        List<MyGame.Models.Message> messages = chatManager.GetMessagesByContactId(contactId);
        chatView.DisplayMessages(messages);
    }

}
