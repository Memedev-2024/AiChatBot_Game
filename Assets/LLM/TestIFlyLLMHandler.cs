using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestIFlyLLMHandler : MonoBehaviour
{
    public Text TagrgetText;
    public InputField TagrgetInputField;
    public Button TagrgetButton;

    List<string> m_StrLst = new List<string>() {
        "你是谁",
        "最近天气"
    };

    IIFlyLLMHandler m_Handler;

    // Start is called before the first frame update
    void Start()
    {
        m_Handler = new IFlyLLMHandler();
        m_Handler.Initialized();
        TagrgetText.text = "";
        m_Handler.OnMessageReceivedAction = (jsonResponse) => { TagrgetText.text += jsonResponse.payload.choices.text[0].content; };

        TagrgetButton.onClick.AddListener(() => {
            TagrgetText.text = "";
            m_Handler.SendMsg(TagrgetInputField.text);
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            //m_Handler.SendMsg(m_StrLst[Random.Range(0,m_StrLst.Count)]);
        }
    }
}