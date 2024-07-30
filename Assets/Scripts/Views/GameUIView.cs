using UnityEngine;
using UnityEngine.UI;
using MyGame.Views;

public class GameUIView : MonoBehaviour
{
    public GameObject contactListView;
    public GameObject chatPrefab; // ����ҳ���Ԥ����
    private GameObject currentChatView; // ��ǰ����ҳ���ʵ��
    public Button backButton; // ���ذ�ť������
    private ChatView chatView; // ���� ChatView ʵ��

    private void Awake()
    {
        if (chatPrefab != null)
        {
            currentChatView = Instantiate(chatPrefab); // ʵ��������ҳ��Ԥ����
       
        }
        else
        {
            Debug.LogError("chatPrefab is not assigned.");
        }
    }

    private void Start()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClicked); // ���İ�ť����¼�
        }
        else
        {
            Debug.LogError("Back Button reference not set in GameUIView.");
        }
        ShowContactListView();
    }

    public void ShowContactListView()
    {
        contactListView.SetActive(true);
        if (currentChatView != null)
        {
            currentChatView.SetActive(false); // ���ص�ǰ������ҳ��ʵ��
        }
        GameController.Instance.SetCurrentPage(Page.ContactList);
    }

    public void ShowChatView()
    {
        contactListView.SetActive(false);
        if (currentChatView != null)
        {
            currentChatView.SetActive(true); // ��ʾ����ҳ��ʵ��
        }
    }

    void OnBackButtonClicked()
    {
        if (GameController.Instance != null)
        {
            string result = GameController.Instance.OnBackButtonPressed();
            Debug.Log("Back button result: " + result);
        }
        else
        {
            Debug.LogError("GameController instance not found.");
        }
    }
}