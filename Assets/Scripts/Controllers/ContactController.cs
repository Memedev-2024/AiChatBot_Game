using System.Collections.Generic;
using UnityEngine;
using MyGame.Models;
using MyGame.Views;



namespace MyGame.Controllers
{
    public class ContactController : MonoBehaviour
    {
        public ChatManager chatManager;
        public ContactView contactView; 
        private ContactManager contactManager;

        private void Start()
        {
            // ��ʼ����ϵ�˹�����
            contactManager = new ContactManager();
            // ��ȡ ChatManager ʵ��
            chatManager = ChatController.Instance?.chatManager; // ͨ��������ȡ
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
        private  async void GenerateRandomContact()
        {
            // ���������ϵ��
            int newId = contactManager.GenerateUniqueId();

            // ʹ�� AI �������ֺ͸������ϣ�ֱ�ӵ��þ�̬������
            (var name, var biography,var background) = await NPCBiographyGenerator.GenerateNPCBiographyAsync();
            
            Contact newContact = new Contact(newId, name)
            {
                Background = background,
                Biography = biography, // ���ɵļ��
                IsOnline = Random.value > 0.5f, // �����������״̬
                Mood = Contact.MoodType.Bored,
                Affinity = 0,

            };

            // ��ӵ�ģ��
            contactManager.AddContact(newContact);

            // ������ͼ
            contactView.AddContact(newContact.ID, newContact.Name, newContact.IsOnline);
        }


        private void RemoveContact(int id, GameObject contactGO)
        {
            contactManager.RemoveContact(id);
            chatManager.RemoveMessagesByContactId(id);
        }
    }
}