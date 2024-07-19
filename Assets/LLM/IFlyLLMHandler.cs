using BestHTTP.WebSocket;
using System;
using UnityEngine;

/// <summary>
/// ʹ��Ѷ�ɴ�ģ��
/// </summary>
public class IFlyLLMHandler : BaseIFlyLLMHandler
{
    #region Data

    /// <summary>
    /// �������ӷ�����ʧ���¼�
    /// </summary>
    int m_TimerServerAccessFailure = -1;
    const float INTERVAL_TIME_SEND_SERVER_ACCESS_FAILURE = 4.0f;

    #endregion

    #region interface function

    /// <summary>
    /// ��ʼ��
    /// </summary>
    public override void Initialized()
    {
        base.Initialized();
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

            Connect();
        }
        catch (Exception e)
        {
            Debug.LogError($"e.ToString() {e.ToString()}\ne.Message {e.Message}");
        }
    }

    #endregion

    #region Websocket�ص�

    /// <summary>
    /// ���������¼�
    /// </summary>
    /// <param name="ws"></param>
    protected override void OnWebSocketOpen(WebSocket ws)
    {
        Debug.Log("WebSocket Connected!");
        Send();
    }

    /// <summary>
    /// ���Ӵ�����Ϣ
    /// </summary>
    /// <param name="ws"></param>
    /// <param name="ex"></param>
    protected override void OnWebSocketError(WebSocket ws, Exception ex)
    {
        base.OnWebSocketError(ws, ex);
        SendServerAccessFailure(1005);
    }

    /// <summary>
    /// ���ӹر��¼�
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
    /// ���ͷ���������ʧ�ܴ���
    /// </summary>
    /// <param name="code"></param>
    private void SendServerAccessFailure(UInt16 code)
    {
        if (code == 1005) // ����ʧ�ܴ�����
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