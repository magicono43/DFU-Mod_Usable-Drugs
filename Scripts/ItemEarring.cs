// Project:         UsableDrugs mod for Daggerfall Unity (http://www.dfworkshop.net)
// Copyright:       Copyright (C) 2022 Kirk.O
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Author:          Kirk.O
// Created On: 	    1/20/2022, 10:10 PM
// Last Edit:		1/20/2022, 10:10 PM
// Version:			1.00
// Special Thanks:  Hazelnut, Ralzar
// Modifier:	

using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.Serialization;
using DaggerfallWorkshop.Game;

namespace UsableDrugs
{
    public class ItemEarring : DaggerfallUnityItem
    {
        public const int templateIndex = 4701;
        public const string baseName = "Earring";

        public static int earringType = -1;

        public ItemEarring() : base(ItemGroups.Jewellery, templateIndex)
        {
            if (earringType >= 0)
                message = earringType;
            else
                message = UsableDrugsMain.GetRandomVariantType(false, false, 11) + 1;
                //message = Random.Range(1, 12);

            earringType = -1;

            switch (message)
            {
                case 1:
                    shortName = "Ruby Earring";
                    value = 120;
					maxCondition = 625;
					enchantmentPoints = 1050;
                    break;
                case 2:
                    shortName = "Emerald Earring";
                    value = 160;
					maxCondition = 780;
					enchantmentPoints = 1650;
                    break;
                case 3:
                    shortName = "Sapphire Earring";
                    value = 135;
					maxCondition = 690;
					enchantmentPoints = 1275;
                    break;
                case 4:
                    shortName = "Diamond Earring";
                    value = 210;
					maxCondition = 1000;
					enchantmentPoints = 2150;
                    break;
                case 5:
                    shortName = "Amethyst Earring";
                    value = 50;
					maxCondition = 380;
					enchantmentPoints = 525;
                    break;
                case 6:
                    shortName = "Apatite Earring";
                    value = 25;
					maxCondition = 290;
					enchantmentPoints = 375;
                    break;
                case 7:
                    shortName = "Aquamarine Earring";
                    value = 75;
					maxCondition = 470;
					enchantmentPoints = 675;
                    break;
                case 8:
                    shortName = "Garnet Earring";
                    value = 35;
					maxCondition = 340;
					enchantmentPoints = 450;
                    break;
                case 9:
                    shortName = "Topaz Earring";
                    value = 60;
					maxCondition = 425;
					enchantmentPoints = 600;
                    break;
                case 10:
                    shortName = "Zircon Earring";
                    value = 100;
					maxCondition = 560;
					enchantmentPoints = 825;
                    break;
                case 11:
                    shortName = "Spinel Earring";
                    value = 85;
					maxCondition = 510;
					enchantmentPoints = 750;
                    break;
                default:
                    shortName = "Broken Earring";
                    value = 1;
					maxCondition = 100;
					enchantmentPoints = 1;
                    break;
            }

            currentCondition = maxCondition;

            CurrentVariant = message - 1;
        }

        public override int InventoryTextureArchive
        {
            get { return templateIndex; }
        }

        public override string ItemName
        {
            get { return shortName; }
        }

        public override string LongName
        {
            get { return shortName; }
        }

        public override EquipSlots GetEquipSlot()
        {
            return GameManager.Instance.PlayerEntity.ItemEquipTable.GetFirstSlot(EquipSlots.Mark0, EquipSlots.Mark1);
        }

        public override int GetEnchantmentPower()
        {
            return enchantmentPoints;
        }

        public override ItemData_v1 GetSaveData()
        {
            ItemData_v1 data = base.GetSaveData();
            data.className = typeof(ItemEarring).ToString();
            return data;
        }

    }
}

