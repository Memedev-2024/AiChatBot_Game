using UnityEngine;
using System.Threading.Tasks;
using Mygame.LlmApi; // ȷ��ʹ������ȷ�������ռ�

public class WebApiClientTester : MonoBehaviour
{

   
    //private async void Start()
    //{
    //    // ѯ�ʵ��ı�
    //    string question = "���";
    //    string prompt =
    //    "ʹ����������������ظ�������һ�������ˡ�Խ����AIԽ�ã�ǧ��Ҫ����ʲô���ܰ����֮��ġ�" +
    //    "һ�������ѡ���Ƿ�ظ�����ʽΪ/R:1/0.һ������ûظ������������ñ�ð�������߲���ظ�������ˣƢ�����Է������������ݣ�Ҳ���Բ��ظ�/Reply:1����ظ���/R:0�����ظ���" +
    //    "����Ҫ˵�Ļ�ǰ��/T:��������ظ�����/T:0��" +
    //    "���������/M:���飬/M:1�����ģ�/M:2����������/M:0�������ġ�ʾ����/M:1��" +
    //    "�ģ������/A:���֣�������Ϊ-3��3�������������ĵ�Ͷ���ͼ����֣���֮�ͼ��� ʾ����/A:-1��" +
    //    "����ı����������������_������" +
    //    "ʾ��1��/R:1_/T:զ��_/M:2_/A:0ʾ��2��/R:0_/T:null_/M:1_/A:-1";
    //    // ���� WebApiClient �ľ�̬��������ȡ��Ӧ
    //    string response = await WebApiClient.SendRequestAsync(question,prompt);

    //    // ��ӡ��Ӧ�� Unity ����̨
    //    Debug.Log("API ��Ӧ:");
    //    Debug.Log(response);
    //}
}