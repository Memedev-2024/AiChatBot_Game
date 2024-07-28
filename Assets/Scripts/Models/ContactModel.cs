using System.Collections.Generic;

namespace MyGame.Models
{
    [System.Serializable]
    public class Contact
    {
        public int ID;              // 联系人的唯一标识符
        public string Name;         // 联系人的名字
        public bool IsOnline;       // 联系人是否在线
        public List<Message> Messages; // 联系人的消息列表

        public Contact()
        {
            Messages = new List<Message>();
        }

        public Contact(int id, string name)
        {
            ID = id;
            Name = name;
            IsOnline = false; // 默认离线
            Messages = new List<Message>();
        }

        public void AddMessage(Message message)
        {
            Messages.Add(message);
        }
    }
}