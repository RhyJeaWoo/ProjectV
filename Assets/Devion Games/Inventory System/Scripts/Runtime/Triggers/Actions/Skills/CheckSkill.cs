﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DevionGames.UIWidgets;
using UnityEngine;

namespace DevionGames.InventorySystem
{
    [Icon("Item")]
    [ComponentMenu("Inventory System/Check Skill")]
    public class CheckSkill : Action
    {
        [Tooltip("The name of the window to lock.")]
        [SerializeField]
        private string m_WindowName = "Skills";

        [ItemPicker(true)]
        [SerializeField]
        private Skill m_Skill= null;

        private ItemContainer m_ItemContainer;

        public override void OnStart()
        {
            this.m_ItemContainer = WidgetUtility.Find<ItemContainer>(this.m_WindowName);
        }

        public override ActionStatus OnUpdate()
        {
            if (this.m_ItemContainer == null)
            {
                Debug.LogWarning("Missing window " + this.m_WindowName + " in scene!");
                return ActionStatus.Failure;
            }

            Skill current = (Skill)this.m_ItemContainer.GetItems(this.m_Skill.Id).FirstOrDefault();
            if(current != null){
                return current.CheckSkill() ? ActionStatus.Success : ActionStatus.Failure;
            }
            return ActionStatus.Success;
        }
    }
}
