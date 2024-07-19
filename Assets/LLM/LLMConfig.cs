public class LLMConfig
{
    #region 讯飞

    /// <summary>
    /// 应用APPID（必须为webapi类型应用，并开通星火认知大模型授权）
    /// </summary>
    public readonly string IFLY_APPID = "YOUR_IFLY_APPID";

    /// <summary>
    /// 接口密钥（webapi类型应用开通星火认知大模型后，控制台--我的应用---星火认知大模型---相应服务的apikey）
    /// </summary>
    public readonly string IFLY_API_SECRET = "YOUR_IFLY_API_SECRET";

    /// <summary>
    /// 接口密钥（webapi类型应用开通星火认知大模型后，控制台--我的应用---星火认知大模型---相应服务的apisecret）
    /// </summary>
    public readonly string IFLY_API_KEY = "YOUR_IFLY_API_KEY";

    /// <summary>
    /// 讯飞大模型访问地址
    /// </summary>
    public readonly string IFLY_HOST_URL = "https://spark-api.xf-yun.com/v3.1/chat";

    /// <summary>
    /// 讯飞图像识别
    /// </summary>
    public readonly string IFLY_IMAGE_RECOGNIZE_URL = "wss://spark-api.cn-huabei-1.xf-yun.com/v2.1/image";

    #endregion
}