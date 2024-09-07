using System.Collections.Generic;

namespace MyGame.Models
{
    [System.Serializable]
    public class Contact
    {
        public int ID;              // ��ϵ�˵�Ψһ��ʶ��
        public string Name;         // ��ϵ�˵�����
        public bool IsOnline;       // ��ϵ���Ƿ�����
        public MoodType Mood;       // ��ϵ�˵�����
        public int Affinity;        // ��ϵ�˵ĺøж� (��Χ -5��10)

        public enum MoodType
        {
            Bored = 0,   // ����
            Happy = 1,   // ����
            Angry = 2    // ����
        }

        public Contact(int id, string name)
        {
            ID = id;
            Name = name;
            IsOnline = true; // Ĭ������
            Mood = MoodType.Bored;  // Ĭ������ֵ
            Affinity = 0;  // Ĭ�Ϻøж�
        }
    }
}