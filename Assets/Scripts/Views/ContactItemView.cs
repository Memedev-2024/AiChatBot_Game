using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MyGame.Views
{
    public class ContactItemView : MonoBehaviour
    {
        public Text nameText;

        public Button enterChat;

        [HideInInspector]
        public int contactId;
        public bool ifOnline;

        public static event System.Action<int> EnterChatUI;

        private void Start()
        {
            enterChat.onClick.AddListener(OnContactClick);
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
    }
}