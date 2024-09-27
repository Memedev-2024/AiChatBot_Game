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
            // 初始化联系人管理器
            contactManager = new ContactManager();
            // 获取 ChatManager 实例
            chatManager = ChatController.Instance?.chatManager; // 通过单例获取
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
        private  async void GenerateRandomContact()
        {
            // 生成随机联系人
            int newId = contactManager.GenerateUniqueId();

            // 使用 AI 生成名字和个人资料（直接调用静态方法）
            (var name, var biography,var background) = await NPCBiographyGenerator.GenerateNPCBiographyAsync();
            
            Contact newContact = new Contact(newId, name)
            {
                Background = background,
                Biography = biography, // 生成的简介
                IsOnline = Random.value > 0.5f, // 随机设置在线状态
                Mood = Contact.MoodType.Bored,
                Affinity = 0,

            };

            // 添加到模型
            contactManager.AddContact(newContact);

            // 更新视图
            contactView.AddContact(newContact.ID, newContact.Name, newContact.IsOnline);
        }


        private void RemoveContact(int id, GameObject contactGO)
        {
            contactManager.RemoveContact(id);
            chatManager.RemoveMessagesByContactId(id);
        }
    }
}