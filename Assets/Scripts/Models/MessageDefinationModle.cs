using System;



// 使类能够被序列化
namespace MyGame.Models
{
    [System.Serializable]
    public class Message
    {
        public string Sender;  // 消息的发送者 ("Player" 或 "NPC名字")
        public string Content; // 消息的内容

        // 默认构造函数
        public Message() { }

        /// <summary>
        /// 初始化 Message 类的新实例
        /// </summary>
        /// <param name="sender">消息的发送者</param>
        /// <param name="content">消息的内容</param>
        public Message(string sender, string content)
        {
            Sender = sender;
            Content = content;
        }
    }
}