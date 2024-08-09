using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyGame.Models;

namespace MyGame.Views
{
    public class ContactView : MonoBehaviour
    {
        public GameObject contactItemPrefab; // ��ϵ��СԤ����
        public Transform contactListContainer; // ��ʾ��ϵ�˵�����
        public Button debugButton; // ���������ϵ�˵ĵ��԰�ť

        private List<GameObject> contactPrefabs; // �洢���ɵ���ϵ��СԤ����

        public event System.Action OnGenerateRandomContact; // �¼�����֪ͨ���������������ϵ��
        //public event System.Action<int, GameObject> OnRemoveContact; // �¼�����֪ͨ������ɾ����ϵ��

        private void Start()
        {
            contactPrefabs = new List<GameObject>();
            // �󶨵��԰�ť����¼�
            debugButton.onClick.AddListener(GenerateRandomContact);

        }

       // ���������ϵ�˲���ʾ
        private void GenerateRandomContact()
        {
            OnGenerateRandomContact?.Invoke();
        }

        // ��������ϵ�����鲢��ӵ���ͼ
        public void AddContacts(List<Contact> newContacts)
        {
            Debug.Log($"Total contacts to add: {newContacts.Count}");
            foreach (var contact in newContacts)
            {
                AddContact(contact.ID, contact.Name, contact.IsOnline);
            }
        }
        // ��ӵ�����ϵ�˵���ͼ
        public void AddContact(int id, string name, bool isOnline)
        {
            GameObject contactItemGO = Instantiate(contactItemPrefab, contactListContainer);
            ContactItemView contactItemView = contactItemGO.GetComponent<ContactItemView>();
            contactItemView.SetupContact(id, name, isOnline);
  //          contactPrefabs.Add(contactItemGO);
        }

    }
}