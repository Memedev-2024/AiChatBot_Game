using UnityEngine;

public enum Page
{
    ContactList,
    Chat
}

public class GameController : MonoBehaviour
{
    public static GameController Instance; // ����ʵ��
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
                gameUIView.ShowChatView(); // ����ϵ���б��л�������ҳ��
                return "success";
            }
        }
        else if (currentPage == Page.Chat)
        {
            GameUIView gameUIView = FindObjectOfType<GameUIView>();
            if (gameUIView != null)
            {
                gameUIView.ShowContactListView(); // ������ҳ�淵�ص���ϵ���б�
                return "success";
            }
        }
        return "failed";
    }
}