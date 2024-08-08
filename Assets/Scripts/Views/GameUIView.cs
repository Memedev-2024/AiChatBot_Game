using UnityEngine;
using UnityEngine.UI;
using MyGame.Views;

public class GameUIView : MonoBehaviour
{
    public GameObject contactListPrefab;
    public GameObject chatPrefab; // ����ҳ���Ԥ����
    public Button backButton; // ���ذ�ť������
    private ChatView chatView; // ���� ChatView ʵ��

    private void Awake()
    {
        //if (chatPrefab != null)
        //{
        //    currentChatView = Instantiate(chatPrefab); // ʵ��������ҳ��Ԥ����
       
        //}
        //else
        //{
        //    Debug.LogError("chatPrefab is not assigned.");
        //}
    }

    private void Start()
    {
        backButton.onClick.AddListener(OnBackButtonClicked); // ���İ�ť����¼�
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