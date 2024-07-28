using System;

namespace MyGame.Models
{
    [System.Serializable]
    public class Message
    {
        public int MessageId;    // 消息的唯一标识符
        public int SenderId;     // 发送者的唯一标识符
        public string Content;   // 消息的内容

        // 默认构造函数
        public Message() { }

        /// <summary>
        /// 初始化 Message 类的新实例
        /// </summary>
        /// <param name="messageId">消息的唯一标识符</param>
        /// <param name="senderId">发送者的唯一标识符</param>
        /// <param name="content">消息的内容</param>
        public Message(int messageId, int senderId, string content)
        {
            MessageId = messageId;
            SenderId = senderId;
            Content = content;
        }
    }
}