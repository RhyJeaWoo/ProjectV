﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.InventorySystem
{
    [System.Serializable]
    public class ItemCondition
    {
        [ItemPicker(true)]
        public Item item;
        public bool boolValue;
        public string stringValue;
        public int intValue;
    }
}
