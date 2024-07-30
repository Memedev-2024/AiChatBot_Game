using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyGame.Models
{
    public class ContactManager
    {
        private Dictionary<int, Contact> contactDictionary; // �洢��ϵ��ID����ϵ�˵��ֵ�
        private string contactsFilePath; // �洢��ϵ�˵��ļ�·��

        public ContactManager()
        {
            contactDictionary = new Dictionary<int, Contact>();
            contactsFilePath = Application.persistentDataPath + "/contacts.json";
            LoadContacts();
        }

        /// <summary>
        /// ����һ������ϵ��
        /// </summary>
        /// <param name="contact">Ҫ���ӵ���ϵ��</param>
        public void AddContact(Contact contact)
        {
            if (!contactDictionary.ContainsKey(contact.ID))
            {
                contactDictionary.Add(contact.ID, contact);
                SaveContacts();
            }
        }

        /// <summary>
        /// ͨ�� ID ������ϵ��
        /// </summary>
        /// <param name="id">��ϵ�˵� ID</param>
        /// <returns>�ҵ�����ϵ��</returns>
        public Contact GetContactById(int id)
        {
            contactDictionary.TryGetValue(id, out Contact contact);
            return contact;
        }

        /// <summary>
        /// ɾ����ϵ��
        /// </summary>
        /// <param name="id">Ҫɾ������ϵ�˵� ID</param>
        public void RemoveContact(int id)
        {
            if (contactDictionary.Remove(id))
            {
                SaveContacts();
            }
        }

        /// <summary>
        /// ��ȡ������ϵ��
        /// </summary>
        /// <returns>������ϵ�˵��б�</returns>
        public List<Contact> GetAllContacts()
        {
            return contactDictionary.Values.ToList();
        }

        /// <summary>
        /// ����Ψһ����ϵ�� ID
        /// </summary>
        /// <returns>Ψһ����ϵ�� ID</returns>
        public int GenerateUniqueId()
        {
            return contactDictionary.Count > 0 ? contactDictionary.Keys.Max() + 1 : 1;
        }

        /// <summary>
        /// ������ϵ��
        /// </summary>
        private void LoadContacts()
        {
            if (System.IO.File.Exists(contactsFilePath))
            {
                string json = System.IO.File.ReadAllText(contactsFilePath);
                List<Contact> contacts = JsonUtility.FromJson<ContactListWrapper>(json).contacts;
                contactDictionary = contacts.ToDictionary(contact => contact.ID);
            }
        }

        /// <summary>
        /// ������ϵ��
        /// </summary>
        private void SaveContacts()
        {
            List<Contact> contacts = contactDictionary.Values.ToList();
            string json = JsonUtility.ToJson(new ContactListWrapper { contacts = contacts });
            System.IO.File.WriteAllText(contactsFilePath, json);
        }

        [System.Serializable]
        private class ContactListWrapper
        {
            public List<Contact> contacts;
        }
    }
}