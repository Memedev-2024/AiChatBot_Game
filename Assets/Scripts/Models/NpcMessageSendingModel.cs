using System;
using UnityEngine;

namespace MyGame.Models
{
    public class NPCMessageModel : MonoBehaviour
    {

        /// <summary>
        /// 获取 NPC 的回复
        /// </summary>
        /// <param name="npcId">NPC 的 ID</param>
        /// <param name="playerMessage">玩家发送的消息</param>
        /// <returns>NPC 的回复</returns>
        public string GetNPCMessage(int messageId, int senderId, string content)
        {
            // 示例：返回调试信息 + 玩家消息
            // 实际应用中可以根据 npcId 和 playerMessage 生成更复杂的回复
            return $"Here is a Debug response: {content},MessageID is:{messageId}";
        }
    }
}