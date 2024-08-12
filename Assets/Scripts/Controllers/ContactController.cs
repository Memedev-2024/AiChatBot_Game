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
            // 从 ContactManager 获取所有联系人
            List<Contact> allContacts = contactManager.GetAllContacts();

            // 确保所有联系人都被传递给视图层
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
        /// 随机生成联系人并添加到视图层和管理器
        /// </summary>
        private void GenerateRandomContact()
        {
            // 生成随机联系人
            int newId = contactManager.GenerateUniqueId();
            Contact newContact = new Contact(newId, "Random Name " + newId)
            {
                IsOnline = Random.value > 0.5f // 随机设置在线状态
            };

            // 添加到模型
            contactManager.AddContact(newContact);

            // 更新视图
            contactView.AddContact(newContact.ID, newContact.Name, newContact.IsOnline);
        }


        private void RemoveContact(int id, GameObject contactGO)
        {
            // 从管理器中删除联系人
            contactManager.RemoveContact(id);
            // 视图层的更新已在 ContactView 中处理
        }
    }
}