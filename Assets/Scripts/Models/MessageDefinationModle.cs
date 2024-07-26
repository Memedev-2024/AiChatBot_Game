using System;



// ʹ���ܹ������л�
namespace MyGame.Models
{
    [System.Serializable]
    public class Message
    {
        public string Sender;  // ��Ϣ�ķ����� ("Player" �� "NPC����")
        public string Content; // ��Ϣ������

        // Ĭ�Ϲ��캯��
        public Message() { }

        /// <summary>
        /// ��ʼ�� Message �����ʵ��
        /// </summary>
        /// <param name="sender">��Ϣ�ķ�����</param>
        /// <param name="content">��Ϣ������</param>
        public Message(string sender, string content)
        {
            Sender = sender;
            Content = content;
        }
    }
}