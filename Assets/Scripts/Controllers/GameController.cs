using UnityEngine;

public enum Page
{
    ContactList,
    Chat
}

public class GameController : MonoBehaviour
{
    public static GameController Instance; // 单例实例
    private Page currentPage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentPage(Page page)
    {
        currentPage = page;
    }

    public Page GetCurrentPage()
    {
        return currentPage;
    }

    public string OnBackButtonPressed()
    {
        if (currentPage == Page.ContactList)
        {
            GameUIView gameUIView = FindObjectOfType<GameUIView>();
            if (gameUIView != null)
            {
                gameUIView.ShowChatView(); // 从联系人列表切换到聊天页面
                return "success";
            }
        }
        else if (currentPage == Page.Chat)
        {
            GameUIView gameUIView = FindObjectOfType<GameUIView>();
            if (gameUIView != null)
            {
                gameUIView.ShowContactListView(); // 从聊天页面返回到联系人列表
                return "success";
            }
        }
        return "failed";
    }
}