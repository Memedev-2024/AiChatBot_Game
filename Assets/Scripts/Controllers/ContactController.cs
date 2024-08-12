using System.Collections.Generic;
using UnityEngine;
using MyGame.Models;
using MyGame.Views;

namespace MyGame.Controllers
{
    public class ContactController : MonoBehaviour
    {
        public ContactView contactView; // ��ϵ����ͼ������
        private ContactManager contactManager; // ��ϵ�˹�����

        private void Start()
        {
            // ��ʼ����ϵ�˹�����
            contactManager = new ContactManager();

            // ������ͼ�¼�
            contactView.OnGenerateRandomContact += GenerateRandomContact;
            contactView.OnRemoveContact += RemoveContact;

            // ������ϵ���б���ʾ����ͼ��
            LoadContacts();
        }

        /// <summary>
        /// ������ϵ���б���ʾ����ͼ��
        /// </summary>
        private void LoadContacts()
        {
            // �� ContactManager ��ȡ������ϵ��
            List<Contact> allContacts = contactManager.GetAllContacts();

            // ȷ��������ϵ�˶������ݸ���ͼ��
            if (allContacts != null && allContacts.Count > 0)
            {
                contactView.AddContacts(allContacts);
            }
            else
            {
                Debug.Log("No contacts found to display.");
            }
        }
        /// <summary>
        /// ���������ϵ�˲���ӵ���ͼ��͹�����
        /// </summary>
        private void GenerateRandomContact()
        {
            // ���������ϵ��
            int newId = contactManager.GenerateUniqueId();
            Contact newContact = new Contact(newId, "Random Name " + newId)
            {
                IsOnline = Random.value > 0.5f // �����������״̬
            };

            // ��ӵ�ģ��
            contactManager.AddContact(newContact);

            // ������ͼ
            contactView.AddContact(newContact.ID, newContact.Name, newContact.IsOnline);
        }


        private void RemoveContact(int id, GameObject contactGO)
        {
            // �ӹ�������ɾ����ϵ��
            contactManager.RemoveContact(id);
            // ��ͼ��ĸ������� ContactView �д���
        }
    }
}