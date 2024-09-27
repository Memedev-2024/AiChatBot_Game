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
            /// ����һ�� NPC �����֡����˼��ͱ������¡�
            /// </summary>
            /// <returns>�������֡����ͱ������µ�Ԫ��</returns>
            public static async Task<(string name, string biography, string background)> GenerateNPCBiographyAsync()
            {
                // AI���ɵ���ʾ��Ϣ��Prompt��
                string prompt = "����һ�������ɫ���������������������һ����Ȥ������������һ����̵ĸ��˼���һ����ϸ�ı������¡�" +
                                "����Ӧ���Ǽ�̵ġ��д���ģ�����������ʵ������" +
                                "���Ӧ�����Ȥ������һ�����ظС�" +
                                "��������Ӧ���Ƕ��صģ����������ɫ�ľ�������Ȥ�����û���������/Name:��ȡ������_/Biography:��ĸ��˼��_/BG:����������±�������Ϊ��ʽ";

                // ʹ�� WebApiClient ��������
                string response = await WebApiClient.SendRequestAsync(prompt,"����ݸ�ʽ��������");
                Debug.Log(response);

                // ��ʼ������
                string name = "δ֪��ɫ";
                string biography = "���޼��";
                string background = "���ޱ�����Ϣ";

                // �������ص��ַ�������������ܵ��쳣���
                string[] parts = response.Split('_');
                // ���ݷ��ص������������
                foreach (string part in parts)
                {
                    if (part.StartsWith("/Name:"))
                    {
                        name = part.Substring(6).Trim();  // "/Name:" ������������ݾ�������
                    }
                    else if (part.StartsWith("/Biography:"))
                    {
                        biography = part.Substring(11).Trim();  // "/Biography:" ��������ݾ��Ǽ��
                    }
                    else if (part.StartsWith("/BG:"))
                    {
                        background = part.Substring(4).Trim();  // "/BG:" ��������ݾ��Ǳ�����Ϣ
                    }
                }

                return (name, biography, background);
            
             }
    }
}