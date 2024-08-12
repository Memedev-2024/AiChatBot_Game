using System;
using UnityEngine;
using Mygame.LlmApi;
using System.Threading.Tasks;

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
        public static async Task<string> GetNPCMessage(int messageId, int senderId, string content)
        {
            string response = await WebApiClient.SendRequestAsync(content, "none");
            // ʾ�������ص�����Ϣ + �����Ϣ
            // ʵ��Ӧ���п��Ը��� npcId �� playerMessage ���ɸ����ӵĻظ�
            return response;
        }
    }
}