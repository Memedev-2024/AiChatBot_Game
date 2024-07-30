using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame.Views
{
    public class ContactView : MonoBehaviour
    {
        public GameObject contactPrefab; // ��ϵ��СԤ����
        public Transform contactListContainer; // ��ʾ��ϵ�˵�����
        public Button debugButton; // ���������ϵ�˵ĵ��԰�ť

        private List<GameObject> contactPrefabs; // �洢���ɵ���ϵ��СԤ����

        public event System.Action OnGenerateRandomContact; // �¼�����֪ͨ���������������ϵ��
        public event System.Action<int, GameObject> OnRemoveContact; // �¼�����֪ͨ������ɾ����ϵ��

        private void Start()
        {
            contactPrefabs = new List<GameObject>();

            // �󶨵��԰�ť����¼�
          //  debugButton.onClick.AddListener(GenerateRandomContact);

        }

        // ���������ϵ�˲���ʾ
        private void GenerateRandomContact()
        {
            OnGenerateRandomContact?.Invoke();
        }

        // �����ϵ�˵���ͼ
        public void AddContact(int id, string name, bool isOnline)
        {
            GameObject contactGO = Instantiate(contactPrefab, contactListContainer);
            ContactItemView contactItemView = contactGO.GetComponent<ContactItemView>();
            contactItemView.SetupContact(id, name, isOnline);

            // ����ɾ����ť�Ļص�
            contactItemView.deleteButton.onClick.AddListener(() => RemoveContact(id, contactGO));

            contactPrefabs.Add(contactGO);
        }

        // ɾ����ϵ��Ԥ����
        private void RemoveContact(int id, GameObject contactGO)
        {
            contactPrefabs.Remove(contactGO);
            Destroy(contactGO);

            OnRemoveContact?.Invoke(id, contactGO);
        }
    }
}