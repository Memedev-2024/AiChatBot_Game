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
        public InputField inputField;    // ���������Ϣ�������
        public Button sendButton;            // ������Ϣ�İ�ť
        public ScrollRect scrollRect;       // ������ͼ
        public Transform chatListContainer; // ��ʾ��Ϣ������
        public GameObject chatItemPrefab; // ��ϢСԤ����

        public Text contactNameText; // ��ʾ��ϵ���������ı���
        public Text typingStatusText; // ��ʾ����״̬���ı���
        public Slider affinitySlider; // ������ʾ�øжȵĻ���

        private List<GameObject> chatPrefabs; // �洢���ɵ���ϢСԤ����
        private CancellationTokenSource typingCancellationTokenSource;
        // �¼�����֪ͨ������������Ϣ
        public event System.Action<string> OnSendMessage;

        // �¼�����֪ͨ������������Ϣ
//         public event System.Action<int> OnLoadMessages;

        private void Start()
        {
            chatPrefabs = new List<GameObject>();

            sendButton.onClick.AddListener(OnSendButtonClick);

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

        public void DisplayMessage(MyGame.Models.Message message)
        {
            // ʵ������ϢԤ����
            GameObject chatItemGO = Instantiate(chatItemPrefab, chatListContainer);

            // ��ȡ ChatItemView �����������Ϣ����
            ChatItemView chatItemView = chatItemGO.GetComponent<ChatItemView>();
            string formattedMessage = $"{message.SenderId}: {message.Content}";
            chatItemView.SetupChatItem(formattedMessage);

            // �����ɵ�Ԥ������ӵ��б����Ա����
            chatPrefabs.Add(chatItemGO);
            // �Զ����������·�
            ScrollToBottom();
        }
        public void DisplayMessages(List<MyGame.Models.Message> messages)
        {
            // ���������Ϣ
            foreach (var prefab in chatPrefabs)
            {
                Destroy(prefab);
            }
            chatPrefabs.Clear();

            // ��ʾ����Ϣ�б�
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

            // ȡ����ǰ�������еĴ���״̬����
            if (typingCancellationTokenSource != null)
            {
                typingCancellationTokenSource.Cancel();
                typingCancellationTokenSource.Dispose();
                typingCancellationTokenSource = null; // Ensure it's set to null after disposal
            }

            // ������Ǵ���״̬��ֱ������ı�������
            if (!isTyping)
            {
                typingStatusText.text = "";
                return;
            }

            // Ϊ�µĴ���״̬���񴴽�һ���µ� CancellationTokenSource
            typingCancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = typingCancellationTokenSource.Token;

            // ��̬��ʾ "Typing..." ��ѭ��Ч��
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
                // �������ȡ��������Ҫ���κ�����
            }
        }
        public void UpdateAffinitySlider(int affinity)
        {
            if (affinitySlider != null)
            {
                affinitySlider.value = affinity;  // ���û����ֵΪ��ǰ�øж�
            }
        }

    }
    
}
        
