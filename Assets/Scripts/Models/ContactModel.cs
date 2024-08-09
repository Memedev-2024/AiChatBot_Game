using System.Collections.Generic;

namespace MyGame.Models
{
    [System.Serializable]
    public class Contact
    {
        public int ID;              // ��ϵ�˵�Ψһ��ʶ��
        public string Name;         // ��ϵ�˵�����
        public bool IsOnline;       // ��ϵ���Ƿ�����

        public Contact(int id, string name)
        {
            ID = id;
            Name = name;
            IsOnline = false; // Ĭ������
        }
    }
}