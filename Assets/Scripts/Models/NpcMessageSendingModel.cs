using System;
using UnityEngine;
using Mygame.LlmApi;
using System.Threading.Tasks;

namespace MyGame.Models
{
    public static class NPCMessageModel
    {

        /// <summary>
        /// 获取 NPC 的回复
        /// </summary>
        /// <param name="npcId">NPC 的 ID</param>
        /// <param name="playerMessage">玩家发送的消息</param>
        /// <returns>NPC 的回复</returns>
        public static async Task<string> GetNPCMessage(int messageId, int senderId, string content)
        {
            string response = await WebApiClient.SendRequestAsync(content, "none");
            // 示例：返回调试信息 + 玩家消息
            // 实际应用中可以根据 npcId 和 playerMessage 生成更复杂的回复
            return response;
        }
    }
}