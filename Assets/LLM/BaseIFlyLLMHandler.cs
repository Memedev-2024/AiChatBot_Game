using Newtonsoft.Json;
using System;
using UnityEngine;
using System.Text;
using BestHTTP.WebSocket;
using System.Collections.Generic;

/**
 * �ǻ���֪��ģ�� WebAPI �ӿڵ���ʾ�� �ӿ��ĵ����ؿ�����https://www.xfyun.cn/doc/spark/Web.html
 * ���������ӣ�https://www.xfyun.cn/doc/spark/%E6%8E%A5%E5%8F%A3%E8%AF%B4%E6%98%8E.html ��code���ش�����ʱ�ؿ���
 * @author iflytek
 */


/// <summary>
/// Ѷ�ɴ�ģ�ͻ���
/// </summary>
public class BaseIFlyLLMHandler : IIFlyLLMHandler
{
    #region Data

    /// <summary>
    /// WebSocket
    /// </summary>
    protected WebSocket m_WebSocket;

    /// <summary>
    /// �������ʷ���ݵĶ���
    /// </summary>
    protected List<IIFlyLLMHandler.Content> m_CacheHistory = new List<IIFlyLLMHandler.Content>();

    /// <summary>
    /// �Ƿ����
    /// </summary>
    protected bool m_ReadEnd = true;

    /// <summary>
    /// �Ƿ���Ҫ�ط�
    /// </summary>
    protected bool m_NeedSend = false;

    /// <summary>
    /// ��ģ�͵�����
    /// </summary>
    protected LLMConfig m_Config;

    /// <summary>
    /// ��¼��һ���ϵ� sid ����Ϊ��Ϻ������һ�ε�û�д����꣬��һ��������Ȼ����յ��� sid ��������
    /// </summary>
    protected string m_OldSid = "";

    /// <summary>
    /// // ������¼�м��sid ,��Ϊ���ܱ����(��һ��ÿ�ζ�����ȫ�������̽���������)
    /// </summary>
    protected string m_RcdSid = "";

    /// <summary>
    /// ���յ����ݵ��¼�
    /// </summary>
    public Action<IIFlyLLMHandler.JsonResponse> OnMessageReceivedAction { get; set; }

    #endregion

    #region interface function


    /// <summary>
    /// ��ʼ��
    /// </summary>
    public virtual void Initialized()
    {
        m_Config = new LLMConfig();

    }

    /// <summary>
    /// ����
    /// </summary>
    public virtual void Connect()
    {

        if (m_WebSocket != null)
        {
            Close();
        }
        string authUrl = GetAuthUrl();
        string url = authUrl.Replace("http://", "ws://").Replace("https://", "wss://");
        m_WebSocket = new WebSocket(new Uri(url));
        m_WebSocket.OnOpen += OnWebSocketOpen;
        m_WebSocket.OnMessage += OnWebSocketMessage;
        m_WebSocket.OnError += OnWebSocketError;
        m_WebSocket.OnClosed += OnWebSocketClosed;
        m_WebSocket.Open();

    }


    /// <summary>
    /// �ر�
    /// </summary>
    public virtual void Close()
    {
        m_OldSid = m_RcdSid;
        m_WebSocket?.Close();
        m_WebSocket = null;
    }

    /// <summary>
    /// ����
    /// </summary>
    public virtual void Reset()
    {
        m_CacheHistory.Clear();
    }

    /// <summary>
    /// ������Ϣ
    /// </summary>
    /// <param name="history"></param>
    public virtual void SendMsg(string askContent)
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
                Connect();
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
    /// �������ӵ��¼�
    /// </summary>
    /// <param name="ws"></param>
    protected virtual void OnWebSocketOpen(WebSocket ws)
    {
        Debug.Log("WebSocket Connected!");
        if (m_NeedSend) Send();
    }

    /// <summary>
    /// ���յ����ݵ��¼�
    /// </summary>
    /// <param name="ws"></param>
    /// <param name="message"></param>
    protected virtual void OnWebSocketMessage(WebSocket ws, string message)
    {
        // ������յ�����Ϣ
        // ��������Ը�����Ҫ������Ӧ�Ĵ���
        Debug.Log($"Received message: {message}");
        IIFlyLLMHandler.JsonResponse jsonResponse = JsonConvert.DeserializeObject<IIFlyLLMHandler.JsonResponse>(message);

        if (jsonResponse.header.code == 10013) // ������ 10013������������˲�ͨ��������Υ�棬�����µ�����������
        {
            Debug.LogError($"OnWebSocketMessage message {jsonResponse.header.message} ");
        }
        else
        {
            Debug.Log($"OnWebSocketMessage m_OldSid {m_OldSid}, jsonResponse.header.sid {jsonResponse.header.sid} jsonResponse.header.code {jsonResponse.header.code}");
            // �жϲ�����һ�ε� sid �ľɵ���������������Ӧ
            if (jsonResponse.header.sid.Equals(m_OldSid) == false)
            {
                if (jsonResponse.header.code == 10014) // ������ 10014����������漰������Ϣ����˲�ͨ������������޷�չʾ���û�
                {
                    jsonResponse = GetHandleJsonResponse(jsonResponse);
                }
                OnMessageReceivedAction?.Invoke(jsonResponse);
                m_RcdSid = jsonResponse.header.sid;
            }
        }

        if (jsonResponse.payload.choices.status == 0)
        {
            Debug.Log("Get First IFlyLLM Response");
        }

        if (jsonResponse.payload.choices.status == 2)
        {
            m_ReadEnd = true;
            Debug.Log("Get Last IFlyLLM Response");
            m_OldSid = jsonResponse.header.sid;
        }
    }

    /// <summary>
    /// ���ӷ���������¼�
    /// </summary>
    /// <param name="ws"></param>
    /// <param name="ex"></param>
    protected virtual void OnWebSocketError(WebSocket ws, Exception ex)
    {
        Debug.LogError($"WebSocket Error: {ex.Message}");
    }

    /// <summary>
    /// ���ӹرյ��¼�
    /// </summary>
    /// <param name="ws"></param>
    /// <param name="code"></param>
    /// <param name="message"></param>
    protected virtual void OnWebSocketClosed(WebSocket ws, UInt16 code, string message)
    {
        Debug.LogFormat("WebSocket Closed!: code={0}, msg={1}", code, message);
    }

    #endregion

    #region protected function

    /// <summary>
    /// ������������
    /// </summary>
    protected virtual void Send()
    {
        m_NeedSend = false;
        m_ReadEnd = false;
        IIFlyLLMHandler.JsonRequest request = CreateRequest();
        string jsonString = JsonConvert.SerializeObject(request);
        Debug.LogError(jsonString);
        m_WebSocket?.Send(jsonString);
    }


    /// <summary>
    /// ����һ����������
    /// </summary>
    /// <returns></returns>
    protected virtual IIFlyLLMHandler.JsonRequest CreateRequest()
    {
        return new IIFlyLLMHandler.JsonRequest
        {
            header = new IIFlyLLMHandler.Header
            {
                app_id = m_Config.IFLY_APPID,
                uid = Guid.NewGuid().ToString().Substring(0, 10)
            },
            parameter = new IIFlyLLMHandler.Parameter
            {
                chat = new IIFlyLLMHandler.Chat
                {
                    domain = "generalv3",
                    temperature = 0.01,
                    max_tokens = 480
                }
            },
            payload = new IIFlyLLMHandler.Payload
            {
                message = new IIFlyLLMHandler.Message
                {
                    text = m_CacheHistory
                }
            }
        };
    }



    /// <summary>
    /// ������ʷ��¼���ݽṹ���Ż�����
    /// (������Ҫ�Լ����ⲹ�䣬���������򵥵�����)
    /// </summary>
    protected virtual void CacheHistory(string askContent)
    {
        m_CacheHistory.Clear();
        IIFlyLLMHandler.Content content = new IIFlyLLMHandler.Content();
        content.role = "user";
        content.content = askContent;
        m_CacheHistory.Add(content);
    }

    /// <summary>
    /// ����������ݺ�Ĵ���
    /// ������֯����(�漰�������ݲ���չʾ)
    /// </summary>
    /// <param name="jsonResponse"></param>
    /// <returns></returns>
    protected virtual IIFlyLLMHandler.JsonResponse GetHandleJsonResponse(IIFlyLLMHandler.JsonResponse jsonResponse)
    {
        int status = 2;

        IIFlyLLMHandler.HeaderResponse header = new IIFlyLLMHandler.HeaderResponse() { code = 0, message = "Success", sid = jsonResponse.header.sid, status = status };

        IIFlyLLMHandler.Text text = new IIFlyLLMHandler.Text() { content = "***(�漰�������ݲ���չʾ)��", role = "assistant", index = 0 };
        List<IIFlyLLMHandler.Text> textlst = new List<IIFlyLLMHandler.Text>();
        textlst.Add(text);

        IIFlyLLMHandler.Choices choices = new IIFlyLLMHandler.Choices() { status = status, seq = 999, text = textlst };
        IIFlyLLMHandler.PayloadResponse payload = new IIFlyLLMHandler.PayloadResponse() { choices = choices };

        IIFlyLLMHandler.JsonResponse reorganizingInformationJRspn = new IIFlyLLMHandler.JsonResponse() { header = header, payload = payload };

        return reorganizingInformationJRspn;
    }

    #endregion

    #region �ַ�������

    /// <summary>
    /// ���ɵõ���Ȩ�� URL 
    /// </summary>
    /// <returns></returns>
    private string GetAuthUrl()
    {
        string date = DateTime.UtcNow.ToString("r");
        Uri uri = new Uri(m_Config.IFLY_HOST_URL);
        StringBuilder builder = new StringBuilder("host: ").Append(uri.Host).Append("\n").//
                                Append("date: ").Append(date).Append("\n").//
                                Append("GET ").Append(uri.LocalPath).Append(" HTTP/1.1");
        string sha = HMACsha256(m_Config.IFLY_API_SECRET, builder.ToString());
        string authorization = string.Format("api_key=\"{0}\", algorithm=\"{1}\", headers=\"{2}\", signature=\"{3}\"", m_Config.IFLY_API_KEY, "hmac-sha256", "host date request-line", sha);
        string NewUrl = "https://" + uri.Host + uri.LocalPath;
        string path1 = "authorization" + "=" + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authorization));
        date = date.Replace(" ", "%20").Replace(":", "%3A").Replace(",", "%2C");
        string path2 = "date" + "=" + date;
        string path3 = "host" + "=" + uri.Host;
        NewUrl = NewUrl + "?" + path1 + "&" + path2 + "&" + path3;
        return NewUrl;
    }

    /// <summary>
    /// HMACsha256 
    /// </summary>
    /// <param name="apiSecretIsKey"></param>
    /// <param name="buider"></param>
    /// <returns></returns>
    private string HMACsha256(string apiSecretIsKey, string buider)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(apiSecretIsKey);
        System.Security.Cryptography.HMACSHA256 hMACSHA256 = new System.Security.Cryptography.HMACSHA256(bytes);
        byte[] date = System.Text.Encoding.UTF8.GetBytes(buider);
        date = hMACSHA256.ComputeHash(date);
        hMACSHA256.Clear();
        return Convert.ToBase64String(date);
    }

    #endregion

}