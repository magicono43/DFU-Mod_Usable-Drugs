// Project:         UsableDrugs mod for Daggerfall Unity (http://www.dfworkshop.net)
// Copyright:       Copyright (C) 2022 Kirk.O
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Author:          Kirk.O
// Created On: 	    1/20/2022, 10:10 PM
// Last Edit:		1/20/2022, 10:10 PM
// Version:			1.00
// Special Thanks:  Hazelnut, Ralzar
// Modifier:	

using UnityEngine;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.Serialization;
using DaggerfallConnect;

namespace UsableDrugs
{
    //Whetstone
    public class ItemWhetstone : AbstractItemUsableDrugs
    {
        public const int templateIndex = 800;

        public override int DurabilityLoss => 20;

        public ItemWhetstone() : base(ItemGroups.UselessItems2, templateIndex)
        {
        }

        public override uint GetItemID()
        {
            return templateIndex;
        }

        public override ItemData_v1 GetSaveData()
        {
            ItemData_v1 data = base.GetSaveData();
            data.className = typeof(ItemWhetstone).ToString();
            return data;
        }

        public override bool IsValidForRepair(DaggerfallUnityItem item)
        {
            DFCareer.Skills skill = item.GetWeaponSkillID();

            return true;
        }

        public override int GetRepairPercentage(int luckMod, DaggerfallUnityItem itemToRepair)
        {
            return Random.Range(14 + luckMod, 26 + luckMod);
        }

        public override int GetStaminaDrain(int endurMod)
        {
            return 10 - endurMod;
        }

        public override int GetTimeDrain(int speedMod, int agiliMod)
        {
            return 1800 - (speedMod * 100) - (agiliMod * 50);
        }
    }

    //Sewing Kit
    public class ItemSewingKit : AbstractItemUsableDrugs
    {
        public const int templateIndex = 801;

        public ItemSewingKit() : base(ItemGroups.UselessItems2, templateIndex)
        {
        }

        public override uint GetItemID()
        {
            return templateIndex;
        }

        public override ItemData_v1 GetSaveData()
        {
            ItemData_v1 data = base.GetSaveData();
            data.className = typeof(ItemSewingKit).ToString();
            return data;
        }

        public override int DurabilityLoss => 20;

        public override bool IsValidForRepair(DaggerfallUnityItem item)
        {
            return !item.IsEnchanted && !item.IsArtifact &&
                ((item.ItemGroup == ItemGroups.Armor && item.NativeMaterialValue == (int)ArmorMaterialTypes.Leather) ||
                item.ItemGroup == ItemGroups.MensClothing || item.ItemGroup == ItemGroups.WomensClothing ||
                item.TemplateIndex == 530); // Item Index for Climates and Calories Camping Equipment item, so can repair the tent item now pretty much.
        }

        public override int GetRepairPercentage(int luckMod, DaggerfallUnityItem itemToRepair)
        {
            return Random.Range(20 + luckMod, 38 + luckMod);
        }

        public override int GetStaminaDrain(int endurMod)
        {
            return 4;
        }

        public override int GetTimeDrain(int speedMod, int agiliMod)
        {
            return 1800 - (speedMod * 70) - (agiliMod * 80);
        }
    }

    //Armorers Hammer
    public class ItemArmorersHammer : AbstractItemUsableDrugs
    {
        public const int templateIndex = 802;

        public ItemArmorersHammer() : base(ItemGroups.UselessItems2, templateIndex)
        {
        }

        public override uint GetItemID()
        {
            return templateIndex;
        }

        public override ItemData_v1 GetSaveData()
        {
            ItemData_v1 data = base.GetSaveData();
            data.className = typeof(ItemArmorersHammer).ToString();
            return data;
        }

        public override int DurabilityLoss => 30;

        public override bool IsValidForRepair(DaggerfallUnityItem item)
        {
            return true;
        }

        public override int GetRepairPercentage(int luckMod, DaggerfallUnityItem itemToRepair)
        {
            return Random.Range(14 + luckMod, 22 + luckMod);
        }

        public override int GetStaminaDrain(int endurMod)
        {
            return 14 - endurMod;
        }

        public override int GetTimeDrain(int speedMod, int agiliMod)
        {
            return 1800 - (speedMod * 50) - (agiliMod * 30);
        }
    }

    //Jewelers Pliers
    public class ItemJewelersPliers : AbstractItemUsableDrugs
    {
        public const int templateIndex = 803;

        public ItemJewelersPliers() : base(ItemGroups.UselessItems2, templateIndex)
        {
        }

        public override uint GetItemID()
        {
            return templateIndex;
        }

        public override ItemData_v1 GetSaveData()
        {
            ItemData_v1 data = base.GetSaveData();
            data.className = typeof(ItemJewelersPliers).ToString();
            return data;
        }

        public override int DurabilityLoss => 25;

        public override bool IsValidForRepair(DaggerfallUnityItem item)
        {

            return true;
        }

        public override int GetRepairPercentage(int luckMod, DaggerfallUnityItem itemToRepair)
        {
            return Random.Range(14 + luckMod, 22 + luckMod);
        }

        public override int GetStaminaDrain(int endurMod)
        {
            return 11 - endurMod;
        }

        public override int GetTimeDrain(int speedMod, int agiliMod)
        {
            return 1800 - (speedMod * 60) - (agiliMod * 50);
        }
    }

    //Epoxy Glue
    public class ItemEpoxyGlue : AbstractItemUsableDrugs
    {
        public const int templateIndex = 804;

        public ItemEpoxyGlue() : base(ItemGroups.UselessItems2, templateIndex)
        {
        }

        public override uint GetItemID()
        {
            return templateIndex;
        }

        public override ItemData_v1 GetSaveData()
        {
            ItemData_v1 data = base.GetSaveData();
            data.className = typeof(ItemEpoxyGlue).ToString();
            return data;
        }

        public override int DurabilityLoss => 10;

        public override bool IsValidForRepair(DaggerfallUnityItem item)
        {
            DFCareer.Skills skill = item.GetWeaponSkillID();

            return true;
        }

        public override int GetRepairPercentage(int luckMod, DaggerfallUnityItem itemToRepair)
        {
            return Random.Range(12 + luckMod, 20 + luckMod);
        }

        public override int GetStaminaDrain(int endurMod)
        {
            return 12 - endurMod;
        }

        public override int GetTimeDrain(int speedMod, int agiliMod)
        {
            return 1800 - (speedMod * 40) - (agiliMod * 20);
        }
    }

    //Charging Powder
    public class ItemChargingPowder : AbstractItemUsableDrugs
    {
        public const int templateIndex = 805;

        public ItemChargingPowder() : base(ItemGroups.UselessItems2, templateIndex)
        {
        }

        public override uint GetItemID()
        {
            return templateIndex;
        }

        public override ItemData_v1 GetSaveData()
        {
            ItemData_v1 data = base.GetSaveData();
            data.className = typeof(ItemChargingPowder).ToString();
            return data;
        }

        public override int DurabilityLoss => 20;

        public override bool IsValidForRepair(DaggerfallUnityItem item)
        {
            return true;
        }

        public override int GetRepairPercentage(int luckMod, DaggerfallUnityItem itemToRepair)
        {
            int repairPercentage = Random.Range(7 + luckMod, 13 + luckMod);

            // Adds bonus repair value amount with Charging Powder repairing more for staves and adamantium items, etc.
            return (int)Mathf.Round(repairPercentage * GetBonusMultiplier(itemToRepair));
        }

        public override int GetStaminaDrain(int endurMod)
        {
            return 4;
        }

        public override int GetTimeDrain(int speedMod, int agiliMod)
        {
            return 1200 - (speedMod * 20) - (agiliMod * 10);
        }

        private float GetBonusMultiplier(DaggerfallUnityItem item)
        {
            if (item.TemplateIndex == (int)Weapons.Staff)
            {
                if (item.NativeMaterialValue == 2)       // Silver Staff
                    return 2.25f;
                else if (item.NativeMaterialValue == 4)  // Dwarven Staff
                    return 2.50f;
                else if (item.NativeMaterialValue == 6)  // Adamantium Staff
                    return 3.00f;
                else                                // All Other Staves
                    return 1.75f;
            }
            else if (item.TemplateIndex == (int)Weapons.Dagger)
            {
                if (item.NativeMaterialValue == 2)       // Silver Dagger
                    return 1.50f;
                else if (item.NativeMaterialValue == 4)  // Dwarven Dagger
                    return 1.75f;
                else if (item.NativeMaterialValue == 6)  // Adamantium Dagger
                    return 2.00f;
                else                                // All Other Daggers
                    return 1.25f;
            }
            else if (item.NativeMaterialValue == 4)      // Dwarven Item
                return 1.25f;
            else if (item.NativeMaterialValue == 2)      // Silver Item
                return 1.50f;
            else if (item.NativeMaterialValue == 6)      // Adamantium Item
                return 1.75f;
            else if (item.TemplateIndex == (int)Jewellery.Wand)
                return 2.50f;
            else if (item.TemplateIndex == (int)Jewellery.Amulet || TemplateIndex == (int)Jewellery.Torc)
                return 1.50f;
            else if (item.TemplateIndex == (int)Jewellery.Ring)
                return 1.25f;
            else if (item.TemplateIndex == (int)MensClothing.Plain_robes || TemplateIndex == (int)WomensClothing.Plain_robes)
                return 2.00f;
            else if (item.TemplateIndex == (int)MensClothing.Priest_robes || TemplateIndex == (int)WomensClothing.Priestess_robes)
                return 1.25f;

            return 1f;
        }

    }
}

