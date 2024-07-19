using BestHTTP.WebSocket;
using System;
using UnityEngine;

/// <summary>
/// 使用讯飞大模型
/// </summary>
public class IFlyLLMHandler : BaseIFlyLLMHandler
{
    #region Data

    /// <summary>
    /// 发送连接服务器失败事件
    /// </summary>
    int m_TimerServerAccessFailure = -1;
    const float INTERVAL_TIME_SEND_SERVER_ACCESS_FAILURE = 4.0f;

    #endregion

    #region interface function

    /// <summary>
    /// 初始化
    /// </summary>
    public override void Initialized()
    {
        base.Initialized();
    }

    /// <summary>
    /// 发送数据
    /// </summary>
    /// <param name="askContent"></param>
    public override void SendMsg(string askContent)
    {
        try
        {
            CacheHistory(askContent);

            Connect();
        }
        catch (Exception e)
        {
            Debug.LogError($"e.ToString() {e.ToString()}\ne.Message {e.Message}");
        }
    }

    #endregion

    #region Websocket回调

    /// <summary>
    /// 建立连接事件
    /// </summary>
    /// <param name="ws"></param>
    protected override void OnWebSocketOpen(WebSocket ws)
    {
        Debug.Log("WebSocket Connected!");
        Send();
    }

    /// <summary>
    /// 连接错误信息
    /// </summary>
    /// <param name="ws"></param>
    /// <param name="ex"></param>
    protected override void OnWebSocketError(WebSocket ws, Exception ex)
    {
        base.OnWebSocketError(ws, ex);
        SendServerAccessFailure(1005);
    }

    /// <summary>
    /// 连接关闭事件
    /// </summary>
    /// <param name="ws"></param>
    /// <param name="code"></param>
    /// <param name="message"></param>
    protected override void OnWebSocketClosed(WebSocket ws, ushort code, string message)
    {
        base.OnWebSocketClosed(ws, code, message);
        SendServerAccessFailure(code);
    }

    #endregion

    #region private function

    /// <summary>
    /// 发送服务器连接失败错误
    /// </summary>
    /// <param name="code"></param>
    private void SendServerAccessFailure(UInt16 code)
    {
        if (code == 1005) // 连接失败错误码
        {
            if (m_TimerServerAccessFailure != -1) return;
            m_TimerServerAccessFailure = Timer.Instance.Post2Scale((index) => {
                Debug.Log("[BubbleChatUseIFlyLLMHandler] SendServerAccessFailure");
                m_TimerServerAccessFailure = -1;
            }, INTERVAL_TIME_SEND_SERVER_ACCESS_FAILURE);
        }
    }

    #endregion
}