using System.Collections.Generic;

namespace MyGame.Models
{
    [System.Serializable]
    public class Contact
    {
        public int ID;              // ��ϵ�˵�Ψһ��ʶ��
        public string Name;         // ��ϵ�˵�����
        public bool IsOnline;       // ��ϵ���Ƿ�����
        public List<Message> Messages; // ��ϵ�˵���Ϣ�б�

        public Contact()
        {
            Messages = new List<Message>();
        }

        public Contact(int id, string name)
        {
            ID = id;
            Name = name;
            IsOnline = false; // Ĭ������
            Messages = new List<Message>();
        }

        public void AddMessage(Message message)
        {
            Messages.Add(message);
        }
    }
}