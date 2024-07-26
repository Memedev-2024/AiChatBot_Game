using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 处理聊天 UI 元素
/// </summary>
namespace MyGame.Views
{
    public class ChatView : MonoBehaviour
    {
        public InputField inputField;    // 玩家输入消息的输入框
        public Button sendButton;        // 发送消息的按钮
        public TMP_Text chatText;        // 显示聊天记录的文本组件

        // 用于设置 GameChatController 的引用
        private GameChatController chatController;

        private void Start()
        {
            // 设置按钮点击事件以发送消息
            sendButton.onClick.AddListener(OnSendButtonClick);

            // 绑定回车键发送消息
            inputField.onEndEdit.AddListener(OnInputFieldEndEdit);

            // 获取 GameChatController 的实例
            chatController = GameChatController.Instance;
        }

        /// <summary>
        /// 处理发送按钮点击事件
        /// </summary>
        private void OnSendButtonClick()
        {
            SendMessageToController();
        }

        /// <summary>
        /// 处理回车键事件
        /// </summary>
        private void OnInputFieldEndEdit(string text)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMessageToController();
            }
        }

        /// <summary>
        /// 将消息发送给控制器
        /// </summary>
        private void SendMessageToController()
        {
            string playerMessage = inputField.text;
            if (!string.IsNullOrEmpty(playerMessage))
            {
                chatController.SendMessage(playerMessage);
                inputField.text = "";  // 清空输入框
            }
        }

        /// <summary>
        /// 在聊天视图中显示一条消息
        /// </summary>
        /// <param name="message">要显示的消息</param>
        public void DisplayMessage(MyGame.Models.Message message)
        {
            chatText.text += $"{message.Sender}: {message.Content}\n";
        }
      
        /// <summary>
        /// Debug用，可删除，按F5清除聊天记录
        /// </summary>
        private void Update()
        {
            // 检测 F5 键的按下事件
            if (Input.GetKeyDown(KeyCode.F5))
            {
                ClearChatHistory();
            }
        }
        public void ClearChatHistory()
        {
            chatController.ClearChatHistory();  // 调用 GameChatController 的清除聊天记录功能
        }

        public void ClearChat()
        {
            chatText.text = "";  // 清空聊天记录显示区域
        }
    }
}