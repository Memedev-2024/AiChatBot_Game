using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyGame.Models
{
    public class ContactManager
    {

        private Dictionary<int, Contact> contactDictionary; // 存储联系人ID和联系人的字典
        private string contactsFilePath; // 存储联系人的文件路径

        public ContactManager()
        {
            contactDictionary = new Dictionary<int, Contact>();
            contactsFilePath = Application.persistentDataPath + "/contacts.json";
            LoadContacts();
        }

        /// <summary>
        /// 添加一个新联系人
        /// </summary>
        /// <param name="contact">要添加的联系人</param>
        public void AddContact(Contact contact)
        {
            if (!contactDictionary.ContainsKey(contact.ID))
            {
                contactDictionary.Add(contact.ID, contact);
                SaveContacts();
            }
        }

        /// <summary>
        /// 通过 ID 查找联系人
        /// </summary>
        /// <param name="id">联系人的 ID</param>
        /// <returns>找到的联系人</returns>
        public Contact GetContactById(int id)
        {
            contactDictionary.TryGetValue(id, out Contact name);
            return name;
        }

        /// <summary>
        /// 删除联系人
        /// </summary>
        /// <param name="id">要删除的联系人的 ID</param>
        public void RemoveContact(int id)
        {
            if (contactDictionary.Remove(id))
            {
                SaveContacts();
            }
        }

        /// <summary>
        /// 获取所有联系人
        /// </summary>
        /// <returns>所有联系人的列表</returns>
        public List<Contact> GetAllContacts()
        {
            return contactDictionary.Values.ToList();
        }

        /// <summary>
        /// 生成唯一的联系人 ID
        /// </summary>
        /// <returns>唯一的联系人 ID</returns>
        public int GenerateUniqueId()
        {
            return contactDictionary.Count > 0 ? contactDictionary.Keys.Max() + 1 : 1;
        }

        /// <summary>
        /// 加载联系人
        /// </summary>
        private void LoadContacts()
        {
            if (System.IO.File.Exists(contactsFilePath))
            {
                string json = System.IO.File.ReadAllText(contactsFilePath);
                List<Contact> contacts = JsonUtility.FromJson<ContactListWrapper>(json).contacts;
                contactDictionary = contacts.ToDictionary(contact => contact.ID);

            }
            else
            {
                contactDictionary = new Dictionary<int, Contact>();
            }
        }
        /// <summary>
        /// 保存联系人
        /// </summary>
        private void SaveContacts()
        {
            List<Contact> contacts = contactDictionary.Values.ToList();
            string json = JsonUtility.ToJson(new ContactListWrapper { contacts = contacts });
            System.IO.File.WriteAllText(contactsFilePath, json);
        }
        public string GetContactNameById(int contactId)
        {
            Contact contact = GetContactById(contactId);
            return contact != null ? contact.Name : "Unknown";
        }
        [System.Serializable]
        private class ContactListWrapper
        {
            public List<Contact> contacts;
        }

        public void UpdateContactMoodAndAffinity(int contactId, Contact.MoodType mood, int affinityChange)
        {
            if (contactDictionary.TryGetValue(contactId, out Contact contact))
            {
                contact.Mood = mood;

                contact.Affinity = Mathf.Clamp(contact.Affinity + affinityChange, -5, 10);

                SaveContacts();
            }
            else
            {
                Debug.LogWarning($"Contact with ID {contactId} not found.");
            }
        }
        // 获取联系人的好感度
        public int GetContactAffinity(int contactId)
        {
            if (contactDictionary.ContainsKey(contactId))
            {
                return contactDictionary[contactId].Affinity;
            }
            return 0;
        }
    }
}