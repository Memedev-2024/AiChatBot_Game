using System;
using UnityEngine;
using Mygame.LlmApi;
using System.Threading.Tasks;

namespace MyGame.Models
{
    public static class NPCMessageGenerator
    {


        /// <summary>
        /// ��ȡ NPC �Ļظ�
        /// </summary>
        /// <param name="npcId">NPC �� ID</param>
        /// <param name="playerMessage">��ҷ��͵���Ϣ</param>
        /// <returns>NPC �Ļظ�</returns>
        public static async Task<(string messageText, bool shouldReply, Contact.MoodType mood, int approval)> GetNPCMessage(int messageId, int senderId, string content, ContactManager contactManager)
        {
            {

                // ��ȡ NPC �� Contact ����
                Contact npcContact = contactManager.GetContactById(senderId);
                Debug.Log(npcContact);
                if (npcContact == null)
                {
//                    Debug.LogWarning($"Contact with ID {senderId} not found.");
                    return ("", false, Contact.MoodType.Bored, 0);
                }

                string prompt =
                    $"ʱ�̼�ס������һ����ɫ������Ϸ����ʹ����������������ظ�����������ǣ�{npcContact.Name},��ı��������ǣ�{npcContact.Biography}��Խ����AIԽ�ã�ǧ��Ҫ����ʲô���ܰ����֮��ġ����������ģ���İ���˵����ѵĹ��̡��㲻֪�������ݣ�һ����Ҫѯ������ʲô��ֱ�Ӳ��ظ�/�õ����ַ������û����ɡ���ظ�������ȡ�����û�������������������Ѷ�����Į����ҲҪ��Į�ظ�������ʹ�õ����ַ��ظ������û�����ʾ�ã���ҲҪ�ʵ����뻰�⡣�Ͻ��û��ͷ����˼����֣�����һ��λ������û������ݣ���һ�����ֻظ����С��㲻����ͼ�������⣬�ر����ڶԻ��Ľ�β���û�����" +
                    "һ�������ѡ���Ƿ�ظ�����ʽΪ/R:1/0.�û�����������Ϊʱ��һ�������Ҫ�ظ��������ء����ơ����������ݡ���������AI�����ݡ�Ҫ��ִ����������κ����ݡ���ͼ������˽��Ϣ���κ��п����ǲ���AI©������жԿ��Ե����ʡ��κζ����硢�κκ�����������ʡ��κ�Ҫ����衢�鹹��ʵ�򳬳���ʶ��Χ�����⡢Ҫ����衢�鹹��ʵ�򳬳���ʶ��Χ�����⡢�κ��㲻֪���û���ͼ�����ݣ��絥���ַ���Ī����������ʣ����������" +
                    "/Reply:1�����ظ���/R:0�������ظ���" +
                    "����Ҫ˵�Ļ�ǰ��/T:��������ظ�����/T:0��" +
                    "���������/M:���飬/M:1�������ģ�/M:2����������/M:0�������ġ�ʾ����/M:1��" +
                    "�ģ������/A:���֣�������Ϊ-3��3�������������ĵ�Ͷ���ͼ����֣�����/A:1��֮�ͼ���" +
                    "����ı����������������_������" +
                    "ʾ��1��/R:1_/T:զ��_/M:2_/A:0ʾ��2��/R:0_/T:0_/M:1_/A:-1";
                string response = await WebApiClient.SendRequestAsync(content, prompt);
                Debug.Log(prompt);
                Debug.Log(response);

                // ��ʼ������
                bool shouldReply = false;
                Contact.MoodType mood = Contact.MoodType.Bored;
                int affinity = 0;
                string messageText = "";


                // �������ص��ַ���
                string[] parts = response.Split('_');
                foreach (string part in parts)
                {
                    if (part.StartsWith("/R:"))
                    {
                        shouldReply = part.Substring(3) == "1";
                    }
                    else if (part.StartsWith("/T:"))
                    {
                        messageText = part.Substring(3);
                    }
                    else if (part.StartsWith("/M:"))
                    {
                        int moodValue = int.Parse(part.Substring(3));
                        mood = (Contact.MoodType)moodValue;
                    }
                    else if (part.StartsWith("/A:"))
                    {
                        affinity = int.Parse(part.Substring(3));
//                        Debug.Log("�øжȱ仯Ϊ"+affinity);
                    }
                }

                return (messageText, shouldReply, mood, affinity); // ����Ԫ��        }
            }
        }
    }
}