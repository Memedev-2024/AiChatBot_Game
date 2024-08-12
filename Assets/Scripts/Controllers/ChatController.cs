using UnityEngine;
using System.Collections.Generic;
using MyGame.Models;
using MyGame.Views;
using System.Threading.Tasks;
using MyGame.Utility;

/// <summary>
/// 管理聊天消息和互动逻辑
/// </summary>
public class ChatController : MonoBehaviour
{
    public static ChatController Instance;  // 单例实例，用于全局访问
    public ChatView chatView;  // 聊天视图的引用

    private List<Message> messages = new List<Message>();  // 存储消息的列表
    public int currentContactId; // 当前聊天的联系人 ID
    private int playerId = 256;

    private MyGame.Models.ChatManager chatManager; // 聊天管理器
    private MyGame.Models.ContactManager contactManager; // 联系人管理器
    /// <summary>
    /// 发送玩家消息并获取 NPC 的回复
    /// </summary>
    /// <param name="playerMessage">玩家发送的消息</param>
    /// 
    private void Awake()
    {
        //确保在整个游戏中只有一个 ChatController 实例，并且这个实例在场景切换时不会被销毁。
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

        // 初始化管理器
        chatManager = new MyGame.Models.ChatManager();
        contactManager = new MyGame.Models.ContactManager();
    }
    /// <summary>
    /// 当场景开始时从本地存储加载消息
    /// </summary>
    private void Start()
    {
        // 初始化管理器
        chatManager = new MyGame.Models.ChatManager();
        contactManager = new MyGame.Models.ContactManager();

        // 订阅视图事件
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
        //npcMessageModel = GetComponent<NPCMessageModel>(); // 确保 NPCMessageModel 作为同一个 GameObject 的组件
        LoadMessages(currentContactId);
    }
    public void GTrySendMessage(string playerMessage)
    {
        GSendMassageAndGetRespond(playerMessage);
    }

    private  async Task GSendMassageAndGetRespond(string playerMessage) 
    {
        int playerMessageId = chatManager.GenerateMessageId();//生成消息id
        Message playerMessageObj = new Message(playerMessageId, playerId, playerMessage);//创建消息
        messages.Add(playerMessageObj);//列表中加入消息
        chatManager.AddMessage(currentContactId, playerMessageObj);//添加消息到联系人并存储
        chatView.DisplayMessage(playerMessageObj);//操作视图层显示消息

        // 显示 NPC 正在打字的状态
        chatView.UpdateTypingStatus(true);

        // 获取并显示 NPC 的回复
        int npcMessageId = chatManager.GenerateMessageId();
        string npcResponse = await NPCMessageModel.GetNPCMessage(npcMessageId, currentContactId, playerMessage);

        // 停止显示打字状态
        chatView.UpdateTypingStatus(false);
        Message npcMessageObj = new Message(npcMessageId, currentContactId, npcResponse);
        messages.Add(npcMessageObj);
        chatManager.AddMessage(currentContactId, npcMessageObj);
        chatView.DisplayMessage(npcMessageObj);

    }


    /// <summary>
    /// 设置当前聊天的联系人 ID
    /// </summary>
    /// <param name="contactId">联系人 ID</param>
    public void SetCurrentContactId(int contactId)
        {
            currentContactId = contactId;

            // 从联系人管理器中获取当前联系人的姓名
            string contactName = contactManager.GetContactNameById(contactId);
            chatView.UpdateContactName(contactName);

            LoadMessages(contactId);
        }
    /// <summary>
    /// 加载并显示指定联系人的消息记录
    /// </summary>
    /// <param name="contactId">联系人 ID</param>
    public void LoadMessages(int contactId)
    {
        List<MyGame.Models.Message> messages = chatManager.GetMessagesByContactId(contactId);
        chatView.DisplayMessages(messages);
    }
  
}
