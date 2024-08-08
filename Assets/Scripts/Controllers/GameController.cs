using UnityEngine;

public enum Page
{
    ContactList,
    Chat
}

public class GameController : MonoBehaviour
{
    public static GameController Instance; // 单例实例
    public GameUIView gameUIView;

    private Page currentPage;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
 //           DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start() 
    {
        SetCurrentPage(Page.ContactList);
    }
    public void SetCurrentPage(Page page)
    {
        currentPage = page;
    }

    public Page GetCurrentPage()
    {
        return currentPage;
    }

    public void OnBackButtonPressed()
    {
        if (currentPage == Page.ContactList)
        {
                gameUIView.ShowChatView(); // 从联系人列表切换到聊天页面
                SetCurrentPage(Page.Chat);

            
        }
        else if (currentPage == Page.Chat)
        {
                gameUIView.ShowContactListView(); // 从聊天页面返回到联系人列表
                SetCurrentPage(Page.ContactList);
        }
    }
}