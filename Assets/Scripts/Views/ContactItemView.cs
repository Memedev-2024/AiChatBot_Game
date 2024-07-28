using UnityEngine;
using UnityEngine.UI;

namespace MyGame.Views
{
    public class ContactItemView : MonoBehaviour
    {
        public Text nameText; // ��ϵ�������ı�
        public Text statusText; // ��ϵ������״̬�ı�
        public Text idText; // ��ϵ��ID�ı�
        public Button deleteButton; // ɾ����ť

        private int contactID; // ��ϵ��ID
        private ContactView contactView; // ����ͼ

        private void Start()
        {
            // ��ȡ����ͼ���
            contactView = FindObjectOfType<ContactView>();
        }

        // ������ϵ�˵���Ϣ
        public void SetupContact(int id, string name, bool isOnline)
        {
            contactID = id;
            nameText.text = name;
            statusText.text = isOnline ? "Online" : "Offline";
            idText.text = "ID: " + id;
        }

        // ��������߼������������
        public void OnContactClick()
        {
            // ���������߼�
            Debug.Log("Entering chat with contact: " + contactID);
        }
    }
}