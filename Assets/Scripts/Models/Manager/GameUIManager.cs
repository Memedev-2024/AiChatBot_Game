using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject contactListView;
    public GameObject chatView;

    public GameObject chatPrefab; // ����ҳ���Ԥ����
    private GameObject currentChatView; // ��ǰ����ҳ���ʵ��

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

    public void ShowContactListView()
    {
        contactListView.SetActive(true);
        chatView.SetActive(false);
        GameController.Instance.SetCurrentPage(Page.ContactList);
    }

    public void ShowChatView()
    {
        contactListView.SetActive(false);
        chatView.SetActive(true);
        //if (currentChatView == null)
        //{
        //    currentChatView = Instantiate(chatPrefab); // ʵ��������ҳ��Ԥ����
        //}
        GameController.Instance.SetCurrentPage(Page.Chat);
    }
}