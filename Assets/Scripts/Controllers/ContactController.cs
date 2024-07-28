using System.Collections.Generic;
using UnityEngine;
using MyGame.Models;
using MyGame.Views;

namespace MyGame.Controllers
{
    public class ContactController : MonoBehaviour
    {
        public ContactView contactView; // 联系人视图的引用
        private ContactManager contactManager; // 联系人管理器

        private void Start()
        {
            // 初始化联系人管理器
            contactManager = new ContactManager();

            // 订阅视图事件
            contactView.OnGenerateRandomContact += GenerateRandomContact;
            contactView.OnRemoveContact += RemoveContact;

            // 加载联系人列表并显示在视图层
            LoadContacts();
        }

        /// <summary>
        /// 加载联系人列表并显示在视图层
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
        /// 随机生成联系人并添加到视图层和管理器
        /// </summary>
        private void GenerateRandomContact()
        {
            int id = contactManager.GenerateUniqueId();
            string name = "Contact " + id;
            bool isOnline = Random.Range(0, 2) == 0;

            // 创建新联系人
            Contact newContact = new Contact(id, name) { IsOnline = isOnline };

            // 添加到管理器
            contactManager.AddContact(newContact);

            // 添加到视图层
            contactView.AddContact(newContact.ID, newContact.Name, newContact.IsOnline);
        }

        /// <summary>
        /// 删除联系人
        /// </summary>
        /// <param name="id">要删除的联系人的 ID</param>
        /// <param name="contactGO">要删除的联系人的 GameObject</param>
        private void RemoveContact(int id, GameObject contactGO)
        {
            // 从管理器中删除联系人
            contactManager.RemoveContact(id);
        }
    }
}