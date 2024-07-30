using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame.Views
{
    public class ChatView : MonoBehaviour
    {
        public InputField inputField;    // 玩家输入消息的输入框
        public Button sendButton;            // 发送消息的按钮
        public TMP_Text chatText;            // 显示聊天记录的文本组件

        // 事件用于通知控制器发送消息
        public event System.Action<string> OnSendMessage;

        // 事件用于通知控制器加载消息
        public event System.Action<int> OnLoadMessages;

        private void Start()
        {
            // 设置按钮点击事件以发送消息
            sendButton.onClick.AddListener(OnSendButtonClick);

            // 绑定回车键发送消息
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


        // 显示单条消息
        public void DisplayMessage(MyGame.Models.Message message)
        {
            string formattedMessage = $"{message.Content}";
            string senderName = $"{message.SenderId}";
            chatText.text += "\n" + senderName+": "+formattedMessage;
        }
        // 显示消息列表
        public void DisplayMessages(List<MyGame.Models.Message> messages)
        {
            chatText.text = "";
            foreach (var message in messages)
            {
                DisplayMessage(message);
            }
        }
    }
}