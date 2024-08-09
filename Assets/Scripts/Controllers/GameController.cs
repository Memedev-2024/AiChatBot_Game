using UnityEngine;
using UnityEngine.UI;
using MyGame.Views;
public enum Page
{
    ContactList,
    Chat
}

public class GameController : MonoBehaviour
{
    public static GameController Instance; // ����ʵ��
    public GameUIView gameUIView;
//    public ContactItemView contactItemView;

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
        ContactItemView.EnterChatUI += OnStartChat;
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
        if (currentPage == Page.Chat)
        {
                gameUIView.ShowContactListView(); // ������ҳ�淵�ص���ϵ���б�
                SetCurrentPage(Page.ContactList);
        }

       
    }
    //������ϵ��
    private void OnStartChat(int contactId)
    {
        Debug.Log($"Contact with ID {contactId} was selected.");

        ChatController.Instance.SetCurrentContactId(contactId);

        gameUIView.ShowChatView(); 
        SetCurrentPage(Page.Chat);

    }
}