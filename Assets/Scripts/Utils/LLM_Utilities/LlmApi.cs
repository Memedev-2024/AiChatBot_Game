using UnityEngine;
using System.Threading.Tasks;
using Mygame.LlmApi; // ȷ��ʹ������ȷ�������ռ�

public class WebApiClientTester : MonoBehaviour
{

   
    private async void Start()
    {
        // ѯ�ʵ��ı�
        string question = "���";
        string prompt = "����һֻè����˵����ʱ����Ҫ�ڽ�β������";
        // ���� WebApiClient �ľ�̬��������ȡ��Ӧ
        string response = await WebApiClient.SendRequestAsync(question,prompt);

        // ��ӡ��Ӧ�� Unity ����̨
        Debug.Log("API ��Ӧ:");
        Debug.Log(response);
    }
}