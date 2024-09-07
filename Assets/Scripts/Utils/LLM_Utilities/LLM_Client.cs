using System;
using UnityEngine;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mygame.LlmApi
{
    /// <summary>
    /// 用于与星火认知大模型 WebAPI 进行交互的客户端类。
    /// </summary>
    public class WebApiClient
    {
        private static ClientWebSocket webSocket; // 用于 WebSocket 连接的实例
        private static CancellationToken cancellationToken; // 取消任务的令牌
        private const string xAppId = "70eef98a"; // 应用 APPID（替换为你的实际 APPID）
        private const string apiSecret = "YTdjMjY0Y2YyYjM0OGJmZDk1MjQwYWYw"; // 接口密钥（替换为你的实际 API Secret）
        private const string apiKey = "aaa1842a4cc48558d75eb993ab7eb3b0"; // 接口密钥（替换为你的实际 API Key）
        private static string hostUrl = "wss://spark-api.xf-yun.com/v3.5/chat"; // WebSocket 接口 URL

        /// <summary>
        /// 发送请求并接收响应的异步方法。
        /// </summary>
        /// <param name="messageText">发送的消息文本</param>
        /// <returns>API 响应的字符串</returns>
        public static async Task<string> SendRequestAsync(string messageText,string systemPrompt)
        {
            string authUrl = GetAuthUrl(); // 获取认证 URL
            string url = authUrl.Replace("http://", "ws://").Replace("https://", "wss://"); // 替换为 WebSocket URL

            using (webSocket = new ClientWebSocket())
            {
                try
                {
                    await webSocket.ConnectAsync(new Uri(url), cancellationToken); // 连接到 WebSocket

                    // 构造 JSON 请求体
                    JsonRequest request = new JsonRequest
                    {
                        header = new Header { app_id = xAppId, uid = "12345" }, // 请求头部信息
                        parameter = new Parameter
                        {
                            chat = new Chat
                            {
                                domain = "generalv3.5", // 模型领域
                                temperature = 0.5, // 温度采样阈值
                                max_tokens = 1024 // 最大令牌数
                            }
                        },
                        payload = new Payload
                        {
                            message = new Message
                            {
                                text = new List<Content>
                                {
                                    new Content { role = "system", content = systemPrompt }, // 消息内容
                                    new Content { role = "user", content = messageText }, // 消息内容
                                }
                            }
                        }
                    };
                    //foreach (var item in request.payload.message.text)
                    //{
                    //    Debug.Log($"Role: {item.role}, Content: {item.content}");
                       
                    //};//Debug测试列表
                    string jsonString = JsonConvert.SerializeObject(request); // 将请求体序列化为 JSON 字符串
                    byte[] frameData = Encoding.UTF8.GetBytes(jsonString); // 将 JSON 字符串转换为字节数组

                    await webSocket.SendAsync(new ArraySegment<byte>(frameData), WebSocketMessageType.Text, true, cancellationToken); // 发送数据

                    return await ReceiveResponseAsync(); // 接收并返回响应
                }
                catch (Exception e)
                {
                    return $"Error: {e.Message}"; // 捕获异常并返回错误信息
                }
            }
        }

        /// <summary>
        /// 异步接收 WebSocket 响应的方法。
        /// </summary>
        /// <returns>API 响应的字符串</returns>
        private static async Task<string> ReceiveResponseAsync()
        {
            byte[] receiveBuffer = new byte[1024]; // 接收缓冲区
            StringBuilder response = new StringBuilder(); // 用于构建响应的字符串
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), cancellationToken); // 接收数据

            while (!result.CloseStatus.HasValue)
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string receivedMessage = Encoding.UTF8.GetString(receiveBuffer, 0, result.Count); // 解码接收到的消息
                    JObject jsonObj = JObject.Parse(receivedMessage); // 将消息解析为 JSON 对象
                    int code = (int)jsonObj["header"]["code"]; // 获取响应代码

                    if (code == 0)
                    {
                        int status = (int)jsonObj["payload"]["choices"]["status"]; // 获取状态
                        JArray textArray = (JArray)jsonObj["payload"]["choices"]["text"]; // 获取文本数组
                        string content = (string)textArray[0]["content"]; // 获取内容
                        response.Append(content); // 添加到响应字符串

                        if (status == 2)
                        {
                            int totalTokens = (int)jsonObj["payload"]["usage"]["text"]["total_tokens"]; // 获取总令牌数
                            return response.ToString(); // 返回最终响应
                        }
                    }
                    else
                    {
                        return $"Request error: {receivedMessage}"; // 返回错误信息
                    }
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    return "WebSocket connection closed"; // WebSocket 连接关闭
                }

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), cancellationToken); // 接收下一个数据
            }

            return response.ToString(); // 返回最终响应
        }

        /// <summary>
        /// 获取用于认证的 URL。
        /// </summary>
        /// <returns>认证 URL 字符串</returns>
    static string GetAuthUrl()
            {
                string date = DateTime.UtcNow.ToString("r");

                Uri uri = new Uri(hostUrl);
                StringBuilder builder = new StringBuilder("host: ").Append(uri.Host).Append("\n").//
                                        Append("date: ").Append(date).Append("\n").//
                                        Append("GET ").Append(uri.LocalPath).Append(" HTTP/1.1");

                string sha = HMACsha256(apiSecret, builder.ToString());
                string authorization = string.Format("api_key=\"{0}\", algorithm=\"{1}\", headers=\"{2}\", signature=\"{3}\"", apiKey, "hmac-sha256", "host date request-line", sha);
                //System.Web.HttpUtility.UrlEncode

                string NewUrl = "https://" + uri.Host + uri.LocalPath;

                string path1 = "authorization" + "=" + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authorization));
                date = date.Replace(" ", "%20").Replace(":", "%3A").Replace(",", "%2C");
                string path2 = "date" + "=" + date;
                string path3 = "host" + "=" + uri.Host;

                NewUrl = NewUrl + "?" + path1 + "&" + path2 + "&" + path3;
                return NewUrl;
            }

        /// <summary>
        /// 使用 HMAC-SHA256 算法生成签名。
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="data">要签名的数据</param>
        /// <returns>签名的 Base64 字符串</returns>
        private static string HMACsha256(string key, string data)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            using (var hmac = new HMACSHA256(keyBytes))
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                byte[] hashBytes = hmac.ComputeHash(dataBytes);
                return Convert.ToBase64String(hashBytes); // 返回 Base64 编码的签名
            }
        }
    }

    // 以下为请求体的类定义
    public class JsonRequest
    {
        public Header header { get; set; }
        public Parameter parameter { get; set; }
        public Payload payload { get; set; }
    }

    public class Header
    {
        public string app_id { get; set; }
        public string uid { get; set; }
    }

    public class Parameter
    {
        public Chat chat { get; set; }
    }

    public class Chat
    {
        public string domain { get; set; }
        public double temperature { get; set; }
        public int max_tokens { get; set; }
    }

    public class Payload
    {
        public Message message { get; set; }
    }

    public class Message
    {
        public List<Content> text { get; set; }
    }

    public class Content
    {
        public string role { get; set; }
        public string content { get; set; }
    }
}
