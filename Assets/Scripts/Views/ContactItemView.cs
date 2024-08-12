using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MyGame.Views
{
    public class ContactItemView : MonoBehaviour
    {
        public Text nameText;

        public Button enterChat;
        public Button deleteButton;

        [HideInInspector]
        public int contactId;
        public bool ifOnline;

        public static event System.Action<int> EnterChatUI;
        public static event System.Action<int, GameObject> OnDeleteContact;

        private void Start()
        {
            enterChat.onClick.AddListener(OnContactClick);
            deleteButton.onClick.AddListener(OnDeleteClick);
        }
        // 设置联系人的信息
        public void SetupContact(int id, string name, bool isOnline)
        {
            nameText.text = name;
            contactId = id;
            ifOnline = isOnline;
        }

        private void OnContactClick()
        {
            EnterChatUI?.Invoke(contactId);
        }
        private void OnDeleteClick()
        {
            OnDeleteContact?.Invoke(contactId, gameObject);
        }
    }
}