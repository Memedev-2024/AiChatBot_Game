using System;

namespace MyGame.Models
{
    [System.Serializable]
    public class Message
    {
        public int MessageId;    // ��Ϣ��Ψһ��ʶ��
        public int SenderId;     // �����ߵ�Ψһ��ʶ��
        public string Content;   // ��Ϣ������

        // Ĭ�Ϲ��캯��
        public Message() { }

        /// <summary>
        /// ��ʼ�� Message �����ʵ��
        /// </summary>
        /// <param name="messageId">��Ϣ��Ψһ��ʶ��</param>
        /// <param name="senderId">�����ߵ�Ψһ��ʶ��</param>
        /// <param name="content">��Ϣ������</param>
        public Message(int messageId, int senderId, string content)
        {
            MessageId = messageId;
            SenderId = senderId;
            Content = content;
        }
    }
}