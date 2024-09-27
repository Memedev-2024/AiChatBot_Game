using System.Collections.Generic;

namespace MyGame.Models
{
    [System.Serializable]
    public class Contact
    {
        public int ID;             
        public string Name;
        public string Biography; // 个人简介
        public string Background; // 个人背景

        public bool IsOnline;      
        public MoodType Mood;       // 联系人的心情
        public int Affinity;        // 联系人的好感度 (范围 -5至10)

        public enum MoodType
        {
            Bored = 0,   // 无聊
            Happy = 1,   // 开心
            Angry = 2    // 生气
        }

        public Contact(int id, string name)
        {
            ID = id;
            Name = name;
            IsOnline = true; // 默认离线
            Mood = MoodType.Bored;  // 默认心情值
            Affinity = 0;  // 默认好感度
        }
    }
}