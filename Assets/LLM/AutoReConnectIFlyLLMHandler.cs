using BestHTTP.WebSocket;
using System;
using UnityEngine;

/// <summary>
/// ���Զ�������Ѷ�ɴ�ģ�ͷ���
/// </summary>
public class AutoReConnectIFlyLLMHandler : BaseIFlyLLMHandler
{

    #region Data

    /// <summary>
    /// m_ReconnectTimerId ���Զ������� TimerId
    /// </summary>
    int m_ReconnectTimerId = -1;

    /// <summary>
    /// m_ReconnectDelay ��  �Զ������� ʱ��
    /// </summary>
    float m_ReconnectDelay = 0.5f;

    #endregion

    #region interface function

    /// <summary>
    /// ��ʼ��
    /// </summary>
    public override void Initialized()
    {
        base.Initialized();
        Connect();
    }

    /// <summary>
    /// �ر�����
    /// </summary>
    public override void Close()
    {
        CancelReconnectTimer();
        base.Close();
    }

    /// <summary>
    /// �Զ�����
    /// </summary>
    internal void AutoReconnect()
    {
        Connect();
    }

    /// <summary>
    /// ��������
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
                    //�ж�websocket��Ȼ���ٷ���
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

    #region Websocket�ص�

    /// <summary>
    /// �ر������¼�
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
    /// ���ӷ���������¼�
    /// </summary>
    /// <param name="ws"></param>
    /// <param name="ex"></param>
    protected override void OnWebSocketError(WebSocket ws, Exception ex)
    {
        base.OnWebSocketError(ws, ex);
        RunReconnectTimer();
    }
    #endregion
    #region ��ʱ��

    /// <summary>
    /// ��ʱ�Զ���������
    /// </summary>
    private void RunReconnectTimer()
    {
        Debug.Log($"Scheduling reconnect in {m_ReconnectDelay} seconds...");
        CancelReconnectTimer();
        m_ReconnectTimerId = Timer.Instance.Post2Really((v) => { AutoReconnect(); }, m_ReconnectDelay);
    }

    /// <summary>
    /// ȡ���������Ӽ�ʱ
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