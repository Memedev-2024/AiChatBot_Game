using UnityEngine;
using MyGame.Controllers;
using MyGame.Models;
using MyGame.Views;
public class GameManager : MonoBehaviour
{
     object[] instances;
    // ��ʼ����Ϸ
    void Start()
    {
        //��ȡʵ���Լ���Ƿ���ȷ���ر�Ҫ�ű���������������ã�����һ�����ò��ϣ�֮�����ɾ��
        instances = FindObjectsOfType<ChatView>();
        DebugInstanceAmount("ChatView");

    }


    //����ű��������ԣ�����
    private void DebugInstanceAmount(string classname)
    {
        if (instances.Length == 0)
        {
            Debug.LogError("Error: No instances of"+classname+" found in the scene!");
        }
        else if (instances.Length > 1)
        {
            Debug.LogError($"Error: Multiple ({instances.Length}) instances of " + classname + " found in the scene! There should be only one.");
        }
        else
        {
 //           Debug.Log(classname+" instance found, all good.");
        }


    }
}
    