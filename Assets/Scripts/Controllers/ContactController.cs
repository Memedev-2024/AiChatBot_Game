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
            List<Contact> contacts = contactManager.GetAllContacts();
            foreach (var contact in contacts)
            {
                contactView.AddContact(contact.ID, contact.Name, contact.IsOnline);
            }
        }

        /// <summary>
        /// ���������ϵ�˲���ӵ���ͼ��͹�����
        /// </summary>
        private void GenerateRandomContact()
        {
            int id = contactManager.GenerateUniqueId();
            string name = "Contact " + id;
            bool isOnline = Random.Range(0, 2) == 0;

            // ��������ϵ��
            Contact newContact = new Contact(id, name) { IsOnline = isOnline };

            // ��ӵ�������
            contactManager.AddContact(newContact);

            // ��ӵ���ͼ��
            contactView.AddContact(newContact.ID, newContact.Name, newContact.IsOnline);
        }

        /// <summary>
        /// ɾ����ϵ��
        /// </summary>
        /// <param name="id">Ҫɾ������ϵ�˵� ID</param>
        /// <param name="contactGO">Ҫɾ������ϵ�˵� GameObject</param>
        private void RemoveContact(int id, GameObject contactGO)
        {
            // �ӹ�������ɾ����ϵ��
            contactManager.RemoveContact(id);
        }
    }
}