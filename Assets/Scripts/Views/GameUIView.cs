using UnityEngine;
using UnityEngine.UI;
using MyGame.Views;

public class GameUIView : MonoBehaviour
{
    public GameObject contactListView;
    public GameObject chatPrefab; // 聊天页面的预制体
    private GameObject currentChatView; // 当前聊天页面的实例
    public Button backButton; // 返回按钮的引用
    private ChatView chatView; // 引用 ChatView 实例

    private void Awake()
    {
        if (chatPrefab != null)
        {
            currentChatView = Instantiate(chatPrefab); // 实例化聊天页面预制体
       
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
            backButton.onClick.AddListener(OnBackButtonClicked); // 订阅按钮点击事件
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
            currentChatView.SetActive(false); // 隐藏当前的聊天页面实例
        }
        GameController.Instance.SetCurrentPage(Page.ContactList);
    }

    public void ShowChatView()
    {
        contactListView.SetActive(false);
        if (currentChatView != null)
        {
            currentChatView.SetActive(true); // 显示聊天页面实例
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