using BestHTTP.WebSocket;
using System;
using UnityEngine;

/// <summary>
/// 带自动重连的讯飞大模型访问
/// </summary>
public class AutoReConnectIFlyLLMHandler : BaseIFlyLLMHandler
{

    #region Data

    /// <summary>
    /// m_ReconnectTimerId ：自动重连的 TimerId
    /// </summary>
    int m_ReconnectTimerId = -1;

    /// <summary>
    /// m_ReconnectDelay ：  自动重连的 时间
    /// </summary>
    float m_ReconnectDelay = 0.5f;

    #endregion

    #region interface function

    /// <summary>
    /// 初始化
    /// </summary>
    public override void Initialized()
    {
        base.Initialized();
        Connect();
    }

    /// <summary>
    /// 关闭连接
    /// </summary>
    public override void Close()
    {
        CancelReconnectTimer();
        base.Close();
    }

    /// <summary>
    /// 自动重连
    /// </summary>
    internal void AutoReconnect()
    {
        Connect();
    }

    /// <summary>
    /// 发送请求
    /// </summary>
    /// <param name="askContent"></param>
    public override void SendMsg(string askContent)
    {
        try
        {
            CacheHistory(askContent);

            if (m_WebSocket != null && m_WebSocket.IsOpen)
            {
                if (m_ReadEnd)
                {
                    Send();
                }
                else
                {
                    //中断websocket，然后再发送
                    Close();
                    m_NeedSend = true;
                }
            }
            else
            {
                m_NeedSend = true;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    #endregion

    #region Websocket回调

    /// <summary>
    /// 关闭连接事件
    /// </summary>
    /// <param name="ws"></param>
    /// <param name="code"></param>
    /// <param name="message"></param>
    protected override void OnWebSocketClosed(WebSocket ws, ushort code, string message)
    {
        base.OnWebSocketClosed(ws, code, message);
        RunReconnectTimer();
    }

    /// <summary>
    /// 连接发生错误的事件
    /// </summary>
    /// <param name="ws"></param>
    /// <param name="ex"></param>
    protected override void OnWebSocketError(WebSocket ws, Exception ex)
    {
        base.OnWebSocketError(ws, ex);
        RunReconnectTimer();
    }
    #endregion
    #region 计时器

    /// <summary>
    /// 计时自动重新连接
    /// </summary>
    private void RunReconnectTimer()
    {
        Debug.Log($"Scheduling reconnect in {m_ReconnectDelay} seconds...");
        CancelReconnectTimer();
        m_ReconnectTimerId = Timer.Instance.Post2Really((v) => { AutoReconnect(); }, m_ReconnectDelay);
    }

    /// <summary>
    /// 取消重新连接计时
    /// </summary>
    private void CancelReconnectTimer()
    {
        if (m_ReconnectTimerId != -1)
        {
            Timer.Instance.Cancel(m_ReconnectTimerId);
            m_ReconnectTimerId = -1;
        }
    }

    #endregion
}