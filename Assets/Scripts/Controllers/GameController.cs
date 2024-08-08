using UnityEngine;

public enum Page
{
    ContactList,
    Chat
}

public class GameController : MonoBehaviour
{
    public static GameController Instance; // ����ʵ��
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
                gameUIView.ShowChatView(); // ����ϵ���б��л�������ҳ��
                SetCurrentPage(Page.Chat);

            
        }
        else if (currentPage == Page.Chat)
        {
                gameUIView.ShowContactListView(); // ������ҳ�淵�ص���ϵ���б�
                SetCurrentPage(Page.ContactList);
        }
    }
}