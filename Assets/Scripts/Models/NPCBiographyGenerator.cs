using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Mygame.LlmApi;

namespace MyGame.Models
{
    public static class NPCBiographyGenerator
    {

        
            /// <summary>
            /// 生成一个 NPC 的名字、个人简介和背景故事。
            /// </summary>
            /// <returns>包含名字、简介和背景故事的元组</returns>
            public static async Task<(string name, string biography, string background)> GenerateNPCBiographyAsync()
            {
                // AI生成的提示信息（Prompt）
                string prompt = "你是一个虚拟角色生成器，你的任务是生成一个有趣的虚拟网名、一个简短的个人简介和一个详细的背景故事。" +
                                "网名应该是简短的、有创意的，尽量避免真实姓名。" +
                                "简介应简短有趣，带有一点神秘感。" +
                                "背景故事应该是独特的，包含虚拟角色的经历、兴趣、爱好或特征。以/Name:你取的网名_/Biography:你的个人简介_/BG:你的完整故事背景，作为格式";

                // 使用 WebApiClient 发送请求
                string response = await WebApiClient.SendRequestAsync(prompt,"请根据格式生成内容");
                Debug.Log(response);

                // 初始化变量
                string name = "未知角色";
                string biography = "暂无简介";
                string background = "暂无背景信息";

                // 解析返回的字符串，并处理可能的异常情况
                string[] parts = response.Split('_');
                // 根据返回的内容填充数据
                foreach (string part in parts)
                {
                    if (part.StartsWith("/Name:"))
                    {
                        name = part.Substring(6).Trim();  // "/Name:" 后面紧跟的内容就是名字
                    }
                    else if (part.StartsWith("/Biography:"))
                    {
                        biography = part.Substring(11).Trim();  // "/Biography:" 后面的内容就是简介
                    }
                    else if (part.StartsWith("/BG:"))
                    {
                        background = part.Substring(4).Trim();  // "/BG:" 后面的内容就是背景信息
                    }
                }

                return (name, biography, background);
            
             }
    }
}