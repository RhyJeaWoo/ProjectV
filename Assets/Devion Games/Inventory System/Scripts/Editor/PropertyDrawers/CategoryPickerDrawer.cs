﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace DevionGames.InventorySystem{
	[CustomPropertyDrawer(typeof(CategoryPickerAttribute))]
	public class CategoryPickerDrawer : PickerDrawer<Category> {

		protected override List<Category> Items {
			get {
				return Database.categories;
			}
		}
		

	}
}