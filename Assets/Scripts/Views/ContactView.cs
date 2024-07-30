using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame.Views
{
    public class ContactView : MonoBehaviour
    {
        public GameObject contactPrefab; // 联系人小预构体
        public Transform contactListContainer; // 显示联系人的容器
        public Button debugButton; // 随机生成联系人的调试按钮

        private List<GameObject> contactPrefabs; // 存储生成的联系人小预构体

        public event System.Action OnGenerateRandomContact; // 事件用于通知控制器生成随机联系人
        public event System.Action<int, GameObject> OnRemoveContact; // 事件用于通知控制器删除联系人

        private void Start()
        {
            contactPrefabs = new List<GameObject>();

            // 绑定调试按钮点击事件
          //  debugButton.onClick.AddListener(GenerateRandomContact);

        }

        // 随机生成联系人并显示
        private void GenerateRandomContact()
        {
            OnGenerateRandomContact?.Invoke();
        }

        // 添加联系人到视图
        public void AddContact(int id, string name, bool isOnline)
        {
            GameObject contactGO = Instantiate(contactPrefab, contactListContainer);
            ContactItemView contactItemView = contactGO.GetComponent<ContactItemView>();
            contactItemView.SetupContact(id, name, isOnline);

            // 设置删除按钮的回调
            contactItemView.deleteButton.onClick.AddListener(() => RemoveContact(id, contactGO));

            contactPrefabs.Add(contactGO);
        }

        // 删除联系人预构体
        private void RemoveContact(int id, GameObject contactGO)
        {
            contactPrefabs.Remove(contactGO);
            Destroy(contactGO);

            OnRemoveContact?.Invoke(id, contactGO);
        }
    }
}