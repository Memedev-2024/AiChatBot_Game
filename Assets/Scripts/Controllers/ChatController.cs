using UnityEngine;
using System.Collections.Generic;
using MyGame.Models;
using MyGame.Views;
using System.Threading.Tasks;
using MyGame.Utility;

/// <summary>
/// ����������Ϣ�ͻ����߼�
/// </summary>
public class ChatController : MonoBehaviour
{
    public static ChatController Instance;  // ����ʵ��������ȫ�ַ���
    public ChatView chatView;  // ������ͼ������

    private List<Message> messages = new List<Message>();  // �洢��Ϣ���б�
    public int currentContactId; // ��ǰ�������ϵ�� ID
    private int playerId = 256;

    private MyGame.Models.ChatManager chatManager; // ���������
    private MyGame.Models.ContactManager contactManager; // ��ϵ�˹�����
    /// <summary>
    /// ���������Ϣ����ȡ NPC �Ļظ�
    /// </summary>
    /// <param name="playerMessage">��ҷ��͵���Ϣ</param>
    /// 
    private void Awake()
    {
        //ȷ����������Ϸ��ֻ��һ�� ChatController ʵ�����������ʵ���ڳ����л�ʱ���ᱻ���١�
        if (Instance == null)
        {
            Instance = this;
 //           DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
//        Debug.Log("ChatController instance created.");

        // ��ʼ��������
        chatManager = new MyGame.Models.ChatManager();
        contactManager = new MyGame.Models.ContactManager();
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
            chatView.OnSendMessage += GTrySendMessage;
//            chatView.OnLoadMessages += LoadMessages;
//            Debug.Log("OnSendMessage event subscribed");
        }
        else
        {
            Debug.LogError("ChatView instance not found.");
        }
        //npcMessageModel = GetComponent<NPCMessageModel>(); // ȷ�� NPCMessageModel ��Ϊͬһ�� GameObject �����
        LoadMessages(currentContactId);
    }
    public void GTrySendMessage(string playerMessage)
    {
        GSendMassageAndGetRespond(playerMessage);
    }

    private  async Task GSendMassageAndGetRespond(string playerMessage) 
    {
        int playerMessageId = chatManager.GenerateMessageId();//������Ϣid
        Message playerMessageObj = new Message(playerMessageId, playerId, playerMessage);//������Ϣ
        messages.Add(playerMessageObj);//�б��м�����Ϣ
        chatManager.AddMessage(currentContactId, playerMessageObj);//�����Ϣ����ϵ�˲��洢
        chatView.DisplayMessage(playerMessageObj);//������ͼ����ʾ��Ϣ

        // ��ʾ NPC ���ڴ��ֵ�״̬
        chatView.UpdateTypingStatus(true);

        // ��ȡ����ʾ NPC �Ļظ�
        int npcMessageId = chatManager.GenerateMessageId();
        string npcResponse = await NPCMessageModel.GetNPCMessage(npcMessageId, currentContactId, playerMessage);

        // ֹͣ��ʾ����״̬
        chatView.UpdateTypingStatus(false);
        Message npcMessageObj = new Message(npcMessageId, currentContactId, npcResponse);
        messages.Add(npcMessageObj);
        chatManager.AddMessage(currentContactId, npcMessageObj);
        chatView.DisplayMessage(npcMessageObj);

    }


    /// <summary>
    /// ���õ�ǰ�������ϵ�� ID
    /// </summary>
    /// <param name="contactId">��ϵ�� ID</param>
    public void SetCurrentContactId(int contactId)
        {
            currentContactId = contactId;

            // ����ϵ�˹������л�ȡ��ǰ��ϵ�˵�����
            string contactName = contactManager.GetContactNameById(contactId);
            chatView.UpdateContactName(contactName);

            LoadMessages(contactId);
        }
    /// <summary>
    /// ���ز���ʾָ����ϵ�˵���Ϣ��¼
    /// </summary>
    /// <param name="contactId">��ϵ�� ID</param>
    public void LoadMessages(int contactId)
    {
        List<MyGame.Models.Message> messages = chatManager.GetMessagesByContactId(contactId);
        chatView.DisplayMessages(messages);
    }
  
}
