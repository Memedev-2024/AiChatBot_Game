using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame.Views
{
    public class ChatView : MonoBehaviour
    {
        public InputField inputField;    // ���������Ϣ�������
        public Button sendButton;            // ������Ϣ�İ�ť
        public TMP_Text chatText;            // ��ʾ�����¼���ı����

        // �¼�����֪ͨ������������Ϣ
        public event System.Action<string> OnSendMessage;

        // �¼�����֪ͨ������������Ϣ
        public event System.Action<int> OnLoadMessages;

        private void Start()
        {
            // ���ð�ť����¼��Է�����Ϣ
            sendButton.onClick.AddListener(OnSendButtonClick);

            // �󶨻س���������Ϣ
            inputField.onEndEdit.AddListener(OnInputFieldEndEdit);
        }

        // ������Ϣ��ť����¼�
        private void OnSendButtonClick()
        {
            GSendMessage();

        }

        // �����س����¼�
        private void OnInputFieldEndEdit(string text)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                GSendMessage();
            }
        }

        // ������Ϣ
        private void GSendMessage()
        {
            string messageText = inputField.text;
            OnSendMessage?.Invoke(messageText);
            inputField.text = "";
        }


        // ��ʾ������Ϣ
        public void DisplayMessage(MyGame.Models.Message message)
        {
            string formattedMessage = $"{message.Content}";
            string senderName = $"{message.SenderId}";
            chatText.text += "\n" + senderName+": "+formattedMessage;
        }
        // ��ʾ��Ϣ�б�
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