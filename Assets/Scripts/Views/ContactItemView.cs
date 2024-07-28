using UnityEngine;
using UnityEngine.UI;

namespace MyGame.Views
{
    public class ContactItemView : MonoBehaviour
    {
        public Text nameText; // 联系人名字文本
        public Text statusText; // 联系人在线状态文本
        public Text idText; // 联系人ID文本
        public Button deleteButton; // 删除按钮

        private int contactID; // 联系人ID
        private ContactView contactView; // 父视图

        private void Start()
        {
            // 获取父视图组件
            contactView = FindObjectOfType<ContactView>();
        }

        // 设置联系人的信息
        public void SetupContact(int id, string name, bool isOnline)
        {
            contactID = id;
            nameText.text = name;
            statusText.text = isOnline ? "Online" : "Offline";
            idText.text = "ID: " + id;
        }

        // 其他相关逻辑，如进入聊天
        public void OnContactClick()
        {
            // 进入聊天逻辑
            Debug.Log("Entering chat with contact: " + contactID);
        }
    }
}