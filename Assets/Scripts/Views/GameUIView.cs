using UnityEngine;
using UnityEngine.UI;
using MyGame.Views;

public class GameUIView : MonoBehaviour
{
    public GameObject contactListPrefab;
    public GameObject chatPrefab; // 聊天页面的预制体
    public Button backButton; // 返回按钮的引用
    private ChatView chatView; // 引用 ChatView 实例

    private void Awake()
    {
        //if (chatPrefab != null)
        //{
        //    currentChatView = Instantiate(chatPrefab); // 实例化聊天页面预制体
       
        //}
        //else
        //{
        //    Debug.LogError("chatPrefab is not assigned.");
        //}
    }

    private void Start()
    {
        backButton.onClick.AddListener(OnBackButtonClicked); // 订阅按钮点击事件
        ShowContactListView();
    }

    public void ShowContactListView()
    {
        contactListPrefab.SetActive(true);
        chatPrefab.SetActive(false); 
    }

    public void ShowChatView()
    {
        contactListPrefab.SetActive(false);
        chatPrefab.SetActive(true); 
    }

    void OnBackButtonClicked()
    {
            GameController.Instance.OnBackButtonPressed();
    }
}