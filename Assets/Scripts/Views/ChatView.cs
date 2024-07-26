using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// �������� UI Ԫ��
/// </summary>
namespace MyGame.Views
{
    public class ChatView : MonoBehaviour
    {
        public InputField inputField;    // ���������Ϣ�������
        public Button sendButton;        // ������Ϣ�İ�ť
        public TMP_Text chatText;        // ��ʾ�����¼���ı����

        // �������� GameChatController ������
        private GameChatController chatController;

        private void Start()
        {
            // ���ð�ť����¼��Է�����Ϣ
            sendButton.onClick.AddListener(OnSendButtonClick);

            // �󶨻س���������Ϣ
            inputField.onEndEdit.AddListener(OnInputFieldEndEdit);

            // ��ȡ GameChatController ��ʵ��
            chatController = GameChatController.Instance;
        }

        /// <summary>
        /// �����Ͱ�ť����¼�
        /// </summary>
        private void OnSendButtonClick()
        {
            SendMessageToController();
        }

        /// <summary>
        /// ����س����¼�
        /// </summary>
        private void OnInputFieldEndEdit(string text)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMessageToController();
            }
        }

        /// <summary>
        /// ����Ϣ���͸�������
        /// </summary>
        private void SendMessageToController()
        {
            string playerMessage = inputField.text;
            if (!string.IsNullOrEmpty(playerMessage))
            {
                chatController.SendMessage(playerMessage);
                inputField.text = "";  // ��������
            }
        }

        /// <summary>
        /// ��������ͼ����ʾһ����Ϣ
        /// </summary>
        /// <param name="message">Ҫ��ʾ����Ϣ</param>
        public void DisplayMessage(MyGame.Models.Message message)
        {
            chatText.text += $"{message.Sender}: {message.Content}\n";
        }
      
        /// <summary>
        /// Debug�ã���ɾ������F5��������¼
        /// </summary>
        private void Update()
        {
            // ��� F5 ���İ����¼�
            if (Input.GetKeyDown(KeyCode.F5))
            {
                ClearChatHistory();
            }
        }
        public void ClearChatHistory()
        {
            chatController.ClearChatHistory();  // ���� GameChatController ����������¼����
        }

        public void ClearChat()
        {
            chatText.text = "";  // ��������¼��ʾ����
        }
    }
}