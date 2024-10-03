using System;
using UnityEngine;

namespace MyGame.Models
{
    public static class NPCMessageModel
    {

        /// <summary>
        /// ��ȡ NPC �Ļظ�
        /// </summary>
        /// <param name="npcId">NPC �� ID</param>
        /// <param name="playerMessage">��ҷ��͵���Ϣ</param>
        /// <returns>NPC �Ļظ�</returns>
        public static string GetNPCMessage(int messageId, int senderId, string content)
        {
            // ʾ�������ص�����Ϣ + �����Ϣ
            // ʵ��Ӧ���п��Ը��� npcId �� playerMessage ���ɸ����ӵĻظ�
            return $"Here is a Debug response: {content},MessageID is:{messageId}";
        }
    }
}