using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyGame.Models;

namespace MyGame.Views
{
    public class ContactView : MonoBehaviour
    {
        public GameObject contactItemPrefab; // 联系人小预构体
        public Transform contactListContainer; // 显示联系人的容器
        public Button debugButton; // 随机生成联系人的调试按钮

        private List<GameObject> contactPrefabs; // 存储生成的联系人小预构体

        public event System.Action OnGenerateRandomContact; // 事件用于通知控制器生成随机联系人
        public event System.Action<int, GameObject> OnRemoveContact;

        private void Start()
        {
            contactPrefabs = new List<GameObject>();
            // 绑定调试按钮点击事件
            debugButton.onClick.AddListener(GenerateRandomContact);
            // 订阅删除联系人事件
            ContactItemView.OnDeleteContact += HandleDeleteContact;
        }

       // 随机生成联系人并显示
        private void GenerateRandomContact()
        {
            OnGenerateRandomContact?.Invoke();
        }

        // 接收新联系人数组并添加到视图
        public void AddContacts(List<Contact> newContacts)
        {
//            Debug.Log($"Total contacts to add: {newContacts.Count}");
            foreach (var contact in newContacts)
            {
                AddContact(contact.ID, contact.Name, contact.IsOnline);
            }
        }
        // 添加单个联系人到视图
        public void AddContact(int id, string name, bool isOnline)
        {
            GameObject contactItemGO = Instantiate(contactItemPrefab, contactListContainer);
            ContactItemView contactItemView = contactItemGO.GetComponent<ContactItemView>();
            contactItemView.SetupContact(id, name, isOnline);

            // 添加到 contactPrefabs 列表
            contactPrefabs.Add(contactItemGO);
        }
        // 处理删除联系人的逻辑
        private void HandleDeleteContact(int id, GameObject contactGO)
        {
            OnRemoveContact?.Invoke(id, contactGO);

            // 更新视图：删除联系人小预构体
            contactPrefabs.Remove(contactGO);
            Destroy(contactGO);
        }
    }
}