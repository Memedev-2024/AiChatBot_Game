using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
namespace MyGame.Views
{
    public class ChatView : MonoBehaviour
    {
        public InputField inputField;    // 玩家输入消息的输入框
        public Button sendButton;            // 发送消息的按钮
        public ScrollRect scrollRect;       // 滚动视图
        public Transform chatListContainer; // 显示消息的容器
        public GameObject chatItemPrefab; // 消息小预构体

        public Text contactNameText; // 显示联系人姓名的文本框
        public Text typingStatusText; // 显示打字状态的文本框
        public Slider affinitySlider; // 用于显示好感度的滑块

        private List<GameObject> chatPrefabs; // 存储生成的消息小预构体
        private CancellationTokenSource typingCancellationTokenSource;
        // 事件用于通知控制器发送消息
        public event System.Action<string> OnSendMessage;

        // 事件用于通知控制器加载消息
//         public event System.Action<int> OnLoadMessages;

        private void Start()
        {
            chatPrefabs = new List<GameObject>();

            sendButton.onClick.AddListener(OnSendButtonClick);

            inputField.onEndEdit.AddListener(OnInputFieldEndEdit);
        }

        // 发送消息按钮点击事件
        private void OnSendButtonClick()
        {
            GSendMessage();

        }

        // 输入框回车键事件
        private void OnInputFieldEndEdit(string text)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                GSendMessage();
            }
        }

        // 发送消息
        private void GSendMessage()
        {
            string messageText = inputField.text;
            OnSendMessage?.Invoke(messageText);
            inputField.text = "";
        }

        public void DisplayMessage(MyGame.Models.Message message)
        {
            // 实例化消息预构体
            GameObject chatItemGO = Instantiate(chatItemPrefab, chatListContainer);

            // 获取 ChatItemView 组件并设置消息内容
            ChatItemView chatItemView = chatItemGO.GetComponent<ChatItemView>();
            string formattedMessage = $"{message.SenderId}: {message.Content}";
            chatItemView.SetupChatItem(formattedMessage);

            // 将生成的预构体添加到列表中以便管理
            chatPrefabs.Add(chatItemGO);
            // 自动滚动到最下方
            ScrollToBottom();
        }
        public void DisplayMessages(List<MyGame.Models.Message> messages)
        {
            // 清空现有消息
            foreach (var prefab in chatPrefabs)
            {
                Destroy(prefab);
            }
            chatPrefabs.Clear();

            // 显示新消息列表
            foreach (var message in messages)
            {
                DisplayMessage(message);
            }
        }
        private void ScrollToBottom()
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0;
            Canvas.ForceUpdateCanvases();
        }

        public void UpdateContactName(string contactName)
        {
            if (contactNameText != null)
            {
                contactNameText.text = contactName;
            }
        }

        public async void UpdateTypingStatus(bool isTyping)
        {

            // 取消当前正在运行的打字状态任务
            if (typingCancellationTokenSource != null)
            {
                typingCancellationTokenSource.Cancel();
                typingCancellationTokenSource.Dispose();
                typingCancellationTokenSource = null; // Ensure it's set to null after disposal
            }

            // 如果不是打字状态，直接清空文本并返回
            if (!isTyping)
            {
                typingStatusText.text = "";
                return;
            }

            // 为新的打字状态任务创建一个新的 CancellationTokenSource
            typingCancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = typingCancellationTokenSource.Token;

            // 动态显示 "Typing..." 的循环效果
            try
            {
                while (true)
                {
                    typingStatusText.text = "Typing";
                    await Task.Delay(500, token);

                    typingStatusText.text = "Typing.";
                    await Task.Delay(500, token);

                    typingStatusText.text = "Typing..";
                    await Task.Delay(500, token);

                    typingStatusText.text = "Typing...";
                    await Task.Delay(500, token);
                }
            }
            catch (TaskCanceledException)
            {
                // 如果任务被取消，不需要做任何事情
            }
        }
        public void UpdateAffinitySlider(int affinity)
        {
            if (affinitySlider != null)
            {
                affinitySlider.value = affinity;  // 设置滑块的值为当前好感度
            }
        }

    }
    
}
        
