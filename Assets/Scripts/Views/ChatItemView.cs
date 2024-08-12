using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame.Views 
{
    public class ChatItemView : MonoBehaviour
    {
        public Text messageText;
        public void SetupChatItem(string content)
        {
            messageText.text = content;
        }

    }
}

