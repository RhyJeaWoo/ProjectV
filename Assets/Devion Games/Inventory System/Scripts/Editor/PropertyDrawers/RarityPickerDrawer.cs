﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace DevionGames.InventorySystem{
	[CustomPropertyDrawer(typeof(RarityPickerAttribute))]
	public class RarityPickerDrawer : PickerDrawer<Rarity> {

		protected override List<Rarity> Items {
			get {
				return Database.raritys;
			}
		}
	}
}