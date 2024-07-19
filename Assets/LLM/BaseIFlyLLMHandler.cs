using Newtonsoft.Json;
using System;
using UnityEngine;
using System.Text;
using BestHTTP.WebSocket;
using System.Collections.Generic;

/**
 * 星火认知大模型 WebAPI 接口调用示例 接口文档（必看）：https://www.xfyun.cn/doc/spark/Web.html
 * 错误码链接：https://www.xfyun.cn/doc/spark/%E6%8E%A5%E5%8F%A3%E8%AF%B4%E6%98%8E.html （code返回错误码时必看）
 * @author iflytek
 */


/// <summary>
/// 讯飞大模型基类
/// </summary>
public class BaseIFlyLLMHandler : IIFlyLLMHandler
{
    #region Data

    /// <summary>
    /// WebSocket
    /// </summary>
    protected WebSocket m_WebSocket;

    /// <summary>
    /// 缓存的历史数据的队列
    /// </summary>
    protected List<IIFlyLLMHandler.Content> m_CacheHistory = new List<IIFlyLLMHandler.Content>();

    /// <summary>
    /// 是否读完
    /// </summary>
    protected bool m_ReadEnd = true;

    /// <summary>
    /// 是否需要重发
    /// </summary>
    protected bool m_NeedSend = false;

    /// <summary>
    /// 大模型的配置
    /// </summary>
    protected LLMConfig m_Config;

    /// <summary>
    /// 记录上一个老的 sid ，因为打断后，如果上一次的没有传输完，下一次连接依然会接收到该 sid 的数据流
    /// </summary>
    protected string m_OldSid = "";

    /// <summary>
    /// // 用来记录中间的sid ,因为可能被打断(不一定每次都能完全正常流程接收完数据)
    /// </summary>
    protected string m_RcdSid = "";

    /// <summary>
    /// 接收到数据的事件
    /// </summary>
    public Action<IIFlyLLMHandler.JsonResponse> OnMessageReceivedAction { get; set; }

    #endregion

    #region interface function


    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Initialized()
    {
        m_Config = new LLMConfig();

    }

    /// <summary>
    /// 连接
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
    /// 关闭
    /// </summary>
    public virtual void Close()
    {
        m_OldSid = m_RcdSid;
        m_WebSocket?.Close();
        m_WebSocket = null;
    }

    /// <summary>
    /// 重置
    /// </summary>
    public virtual void Reset()
    {
        m_CacheHistory.Clear();
    }

    /// <summary>
    /// 发送消息
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
                    //中断websocket，然后再发送
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

    #region Websocket回调

    /// <summary>
    /// 建立连接的事件
    /// </summary>
    /// <param name="ws"></param>
    protected virtual void OnWebSocketOpen(WebSocket ws)
    {
        Debug.Log("WebSocket Connected!");
        if (m_NeedSend) Send();
    }

    /// <summary>
    /// 接收到数据的事件
    /// </summary>
    /// <param name="ws"></param>
    /// <param name="message"></param>
    protected virtual void OnWebSocketMessage(WebSocket ws, string message)
    {
        // 处理接收到的消息
        // 这里你可以根据需要进行相应的处理
        Debug.Log($"Received message: {message}");
        IIFlyLLMHandler.JsonResponse jsonResponse = JsonConvert.DeserializeObject<IIFlyLLMHandler.JsonResponse>(message);

        if (jsonResponse.header.code == 10013) // 错误码 10013：输入内容审核不通过，涉嫌违规，请重新调整输入内容
        {
            Debug.LogError($"OnWebSocketMessage message {jsonResponse.header.message} ");
        }
        else
        {
            Debug.Log($"OnWebSocketMessage m_OldSid {m_OldSid}, jsonResponse.header.sid {jsonResponse.header.sid} jsonResponse.header.code {jsonResponse.header.code}");
            // 判断不是上一次的 sid 的旧的数据流在做出反应
            if (jsonResponse.header.sid.Equals(m_OldSid) == false)
            {
                if (jsonResponse.header.code == 10014) // 错误码 10014：输出内容涉及敏感信息，审核不通过，后续结果无法展示给用户
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
    /// 连接发生错误的事件
    /// </summary>
    /// <param name="ws"></param>
    /// <param name="ex"></param>
    protected virtual void OnWebSocketError(WebSocket ws, Exception ex)
    {
        Debug.LogError($"WebSocket Error: {ex.Message}");
    }

    /// <summary>
    /// 连接关闭的事件
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
    /// 真正发送数据
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
    /// 创建一个连接请求
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
    /// 缓存历史记录数据结构，优化性能
    /// (缓存需要自己额外补充，这里先做简单的提问)
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
    /// 敏感输出内容后的处理
    /// 重新组织数据(涉及敏感内容不予展示)
    /// </summary>
    /// <param name="jsonResponse"></param>
    /// <returns></returns>
    protected virtual IIFlyLLMHandler.JsonResponse GetHandleJsonResponse(IIFlyLLMHandler.JsonResponse jsonResponse)
    {
        int status = 2;

        IIFlyLLMHandler.HeaderResponse header = new IIFlyLLMHandler.HeaderResponse() { code = 0, message = "Success", sid = jsonResponse.header.sid, status = status };

        IIFlyLLMHandler.Text text = new IIFlyLLMHandler.Text() { content = "***(涉及敏感内容不予展示)。", role = "assistant", index = 0 };
        List<IIFlyLLMHandler.Text> textlst = new List<IIFlyLLMHandler.Text>();
        textlst.Add(text);

        IIFlyLLMHandler.Choices choices = new IIFlyLLMHandler.Choices() { status = status, seq = 999, text = textlst };
        IIFlyLLMHandler.PayloadResponse payload = new IIFlyLLMHandler.PayloadResponse() { choices = choices };

        IIFlyLLMHandler.JsonResponse reorganizingInformationJRspn = new IIFlyLLMHandler.JsonResponse() { header = header, payload = payload };

        return reorganizingInformationJRspn;
    }

    #endregion

    #region 字符串操作

    /// <summary>
    /// 生成得到授权的 URL 
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