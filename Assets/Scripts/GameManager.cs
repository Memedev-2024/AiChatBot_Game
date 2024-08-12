using UnityEngine;
using MyGame.Controllers;
using MyGame.Models;
using MyGame.Views;
public class GameManager : MonoBehaviour
{
     object[] instances;
    // 初始化游戏
    void Start()
    {
        //获取实例以检测是否正确挂载必要脚本，这个功能练手用，好像一下子用不上，之后可能删除
        instances = FindObjectsOfType<ChatView>();
        DebugInstanceAmount("ChatView");

    }


    //如果脚本数量不对，报错
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
    