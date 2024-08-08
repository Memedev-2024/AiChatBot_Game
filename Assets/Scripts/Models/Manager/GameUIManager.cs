using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject contactListView;
    public GameObject chatView;

    public GameObject chatPrefab; // 聊天页面的预制体
    private GameObject currentChatView; // 当前聊天页面的实例

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
        //    currentChatView = Instantiate(chatPrefab); // 实例化聊天页面预制体
        //}
        GameController.Instance.SetCurrentPage(Page.Chat);
    }
}