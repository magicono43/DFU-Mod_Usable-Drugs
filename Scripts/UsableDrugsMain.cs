// Project:         UsableDrugs mod for Daggerfall Unity (http://www.dfworkshop.net)
// Copyright:       Copyright (C) 2022 Kirk.O
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Author:          Kirk.O
// Created On: 	    1/20/2022, 10:10 PM
// Last Edit:		1/20/2022, 10:10 PM
// Version:			1.00
// Special Thanks:  Hazelnut, Ralzar
// Modifier:			

using System;
using UnityEngine;
using DaggerfallConnect;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.Utility;
using Wenzil.Console;
using System.Collections.Generic;
using DaggerfallConnect.FallExe;

namespace UsableDrugs
{
    public class UsableDrugsMain : MonoBehaviour
    {
        static Mod mod;

        [Invoke(StateManager.StateTypes.Start, 0)]
        public static void Init(InitParams initParams)
        {
            mod = initParams.Mod;
            var go = new GameObject(mod.Title);
            go.AddComponent<UsableDrugsMain>();
        }

        void Awake()
        {
            InitMod();

            mod.IsReady = true;
        }

        private static void InitMod()
        {
            Debug.Log("Begin mod init: UsableDrugs");

            DaggerfallUnity.Instance.ItemHelper.RegisterCustomItem(ItemRing.templateIndex, ItemGroups.UselessItems1, typeof(ItemRing));
            DaggerfallUnity.Instance.ItemHelper.RegisterCustomItem(ItemEarring.templateIndex, ItemGroups.UselessItems1, typeof(ItemEarring));

            PlayerActivate.OnLootSpawned += DrugsLoot_OnLootSpawned;
            LootTables.OnLootSpawned += DrugsLoot_OnDungeonLootSpawned;
            EnemyDeath.OnEnemyDeath += DrugsLoot_OnEnemyDeath;

            Debug.Log("Finished mod init: UsableDrugs");
        }

        void Start()
        {
            RegisterJACommands();
        }

        public static void DrugsLoot_OnLootSpawned(object sender, ContainerLootSpawnedEventArgs e) // Populates objects such as shop shelves and private property upon player activation.
        {
            DaggerfallInterior interior = GameManager.Instance.PlayerEnterExit.Interior;
            PlayerEntity playerEntity = GameManager.Instance.PlayerEntity;
            ItemHelper itemHelper = DaggerfallUnity.Instance.ItemHelper;
            int[] allowedDrugs = { }; // Gemstone = 1
            int DrugsAmount = 0;
            int qualityLenience = 0;
            float condDamage = 1f;

            if (interior != null && e.ContainerType == LootContainerTypes.ShopShelves)
            {
                if (interior.BuildingData.BuildingType == DFLocation.BuildingTypes.GemStore)
                {
                    allowedDrugs = new int[] { 1, 1, 1, 1, 1, 1, 4700, 4700, 4700, 4700, 4700, 4701, 4701, 4701, 4701, 4701, 4702, 4702, 4702, 4702, 4703, 4703, 4703, 4704, 4704, 4704, 4705, 4705, 4706, 4706, 4706, 4707, 4707, 4707 };
                    DrugsAmount = UnityEngine.Random.Range(3, 10);
                }
                else if (interior.BuildingData.BuildingType == DFLocation.BuildingTypes.PawnShop)
                {
					allowedDrugs = new int[] { 4700, 4700, 4700, 4701, 4701, 4701, 4702, 4702, 4702, 4703, 4703, 4704, 4704, 4705, 4705, 4705, 4706, 4706, 4707, 4707 };
                    DrugsAmount = UnityEngine.Random.Range(0, 3);
                    qualityLenience = 4;
                }
                else if (interior.BuildingData.BuildingType == DFLocation.BuildingTypes.Alchemist)
                {
					allowedDrugs = new int[] { 1 };
                    DrugsAmount = UnityEngine.Random.Range(2, 6);
                }
            }
            else if (interior != null && e.ContainerType == LootContainerTypes.HouseContainers)
            {
                if (interior.BuildingData.BuildingType == DFLocation.BuildingTypes.Palace)
                {
                    if (Dice100.SuccessRoll(30)) // 30
                        DrugsAmount = UnityEngine.Random.Range(0, 4);
                    allowedDrugs = new int[] { 1, 1, 4700, 4700, 4700, 4700, 4701, 4701, 4701, 4701, 4702, 4702, 4702, 4703, 4703, 4704, 4704, 4705, 4705, 4705, 4706, 4706, 4706, 4706, 4707, 4707, 4707, 4707 };
                    condDamage = UnityEngine.Random.Range(0.60f, 0.85f);
                    qualityLenience = 6;
                }
                else
                {
                    if (Dice100.SuccessRoll(15)) // 15
                        DrugsAmount = UnityEngine.Random.Range(0, 3);
                    allowedDrugs = new int[] { 1, 1, 1, 4700, 4700, 4700, 4700, 4701, 4701, 4701, 4701, 4701, 4702, 4702, 4702, 4703, 4703, 4704, 4706, 4706, 4706, 4706, 4707, 4707, 4707, 4707 };
                    condDamage = UnityEngine.Random.Range(0.35f, 0.60f);
                    qualityLenience = 2;
                }
            }

            for (int i = 0; i < DrugsAmount; i++)
            {
                DaggerfallUnityItem item = null;
                ItemTemplate itemTemplate;
                int chosenJewelType = PickOneOf(allowedDrugs);
                int chosenItemIndex = -1;

                switch (chosenJewelType)
                {
                    case 1:
                        chosenItemIndex = RollGemstoneUsed(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, playerEntity.Stats.LiveLuck, interior.BuildingData.Quality);
                        itemTemplate = itemHelper.GetItemTemplate(ItemGroups.Gems, DetermineGemstone(chosenItemIndex));
                        if (itemTemplate.rarity > (interior.BuildingData.Quality + qualityLenience))
                            break;
                        item = ItemBuilder.CreateItem(ItemGroups.Gems, DetermineGemstone(chosenItemIndex));
                        item.currentCondition = (int)(condDamage * item.maxCondition);
                        e.Loot.AddItem(item);
                        break;
                    case 4700:
                    case 4701:
                    case 4702:
                    case 4703:
                    case 4704:
                    case 4705:
                    case 4706:
                    case 4707:
                        itemTemplate = itemHelper.GetItemTemplate(ItemGroups.Jewellery, chosenJewelType);
                        if (itemTemplate.rarity > (interior.BuildingData.Quality + qualityLenience))
                            break;
                        item = ItemBuilder.CreateItem(ItemGroups.Jewellery, chosenJewelType);
                        item.currentCondition = (int)(condDamage * item.maxCondition);
                        e.Loot.AddItem(item);
                        break;
                    default:
                        break;
                }
            }
        }

        public static void DrugsLoot_OnDungeonLootSpawned(object sender, TabledLootSpawnedEventArgs e) // Populates static loot piles in dungeons.
        {
            PlayerEntity playerEntity = GameManager.Instance.PlayerEntity;
            ItemHelper itemHelper = DaggerfallUnity.Instance.ItemHelper;
            int[] allowedDrugs = { }; // Gemstone = 1
            int DrugsAmount = 0;
            float condDamage = 1f;

            //DaggerfallDungeon dungeon = GameManager.Instance.PlayerEnterExit.Dungeon;
            //DFRegion.DungeonTypes dungtype = dungeon.Summary.DungeonType;

            switch (e.LocationIndex)
            {
                case (int)DFRegion.DungeonTypes.Crypt:
                case (int)DFRegion.DungeonTypes.Cemetery:
                    if (Dice100.SuccessRoll(15)) // 15
                        DrugsAmount = UnityEngine.Random.Range(0, 3);
                    allowedDrugs = new int[] { 1, 1, 1, 4700, 4700, 4700, 4700, 4701, 4701, 4701, 4701, 4702, 4702, 4702, 4703, 4703, 4704, 4706, 4706, 4706, 4707, 4707, 4707 };
                    condDamage = UnityEngine.Random.Range(0.3f, 0.6f);
                    break;
                case (int)DFRegion.DungeonTypes.OrcStronghold:
                case (int)DFRegion.DungeonTypes.HumanStronghold:
                case (int)DFRegion.DungeonTypes.BarbarianStronghold:
                    if (Dice100.SuccessRoll(5)) // 5
                        DrugsAmount = UnityEngine.Random.Range(0, 3);
                    allowedDrugs = new int[] { 1, 1, 1, 4700, 4700, 4700, 4700, 4701, 4701, 4701, 4701, 4702, 4702, 4702, 4703, 4703, 4705, 4706, 4706, 4706, 4706, 4707, 4707, 4707, 4707 };
                    condDamage = UnityEngine.Random.Range(0.5f, 0.8f);
                    break;
                case (int)DFRegion.DungeonTypes.DesecratedTemple:
                    if (Dice100.SuccessRoll(10)) // 10
                        DrugsAmount = UnityEngine.Random.Range(0, 3);
                    allowedDrugs = new int[] { 1, 4700, 4700, 4700, 4700, 4701, 4701, 4701, 4701, 4702, 4702, 4702, 4703, 4703, 4704, 4704, 4704, 4706, 4706, 4706, 4706, 4707, 4707, 4707, 4707 };
                    condDamage = UnityEngine.Random.Range(0.4f, 0.7f);
                    break;
                case (int)DFRegion.DungeonTypes.VampireHaunt:
                    if (Dice100.SuccessRoll(10)) // 10
                        DrugsAmount = UnityEngine.Random.Range(0, 3);
                    allowedDrugs = new int[] { 1, 1, 1, 1, 1, 1, 4701, 4701, 4701, 4701, 4701, 4701, 4702, 4702, 4702, 4702, 4703, 4703, 4703, 4706, 4706, 4706, 4706, 4707, 4707, 4707, 4707 };
                    condDamage = UnityEngine.Random.Range(0.5f, 0.8f);
                    break;
                case (int)DFRegion.DungeonTypes.RuinedCastle:
                case (int)DFRegion.DungeonTypes.DragonsDen:
                    if (Dice100.SuccessRoll(20)) // 20
                        DrugsAmount = UnityEngine.Random.Range(0, 5);
                    allowedDrugs = new int[] { 1, 1, 1, 1, 4700, 4700, 4700, 4700, 4701, 4701, 4701, 4702, 4702, 4702, 4703, 4703, 4703, 4704, 4705, 4705, 4705, 4706, 4706, 4706, 4707, 4707, 4707 };
                    condDamage = UnityEngine.Random.Range(0.2f, 0.5f);
                    break;
                default:
                    break;
            }

            for (int i = 0; i < DrugsAmount; i++)
            {
                DaggerfallUnityItem item = null;
                ItemTemplate itemTemplate;
                int chosenJewelType = PickOneOf(allowedDrugs);
                int chosenItemIndex = -1;

                switch (chosenJewelType)
                {
                    case 1:
                        chosenItemIndex = RollGemstoneUsed(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, playerEntity.Stats.LiveLuck);
                        itemTemplate = itemHelper.GetItemTemplate(ItemGroups.Gems, DetermineGemstone(chosenItemIndex));
                        item = ItemBuilder.CreateItem(ItemGroups.Gems, DetermineGemstone(chosenItemIndex));
                        item.currentCondition = (int)(condDamage * item.maxCondition);
                        e.Items.AddItem(item);
                        break;
                    case 4700:
                    case 4701:
                    case 4702:
                    case 4703:
                    case 4704:
                    case 4705:
                    case 4706:
                    case 4707:
                        itemTemplate = itemHelper.GetItemTemplate(ItemGroups.Jewellery, chosenJewelType);
                        item = ItemBuilder.CreateItem(ItemGroups.Jewellery, chosenJewelType);
                        item.currentCondition = (int)(condDamage * item.maxCondition);
                        e.Items.AddItem(item);
                        break;
                    default:
                        break;
                }
            }
        }

        public static void DrugsLoot_OnEnemyDeath(object sender, EventArgs e) // Populates enemy loot upon their death.
        {
            EnemyDeath enemyDeath = sender as EnemyDeath;
            if (enemyDeath != null)
            {
                DaggerfallEntityBehaviour entityBehaviour = enemyDeath.GetComponent<DaggerfallEntityBehaviour>();
                if (entityBehaviour != null)
                {
                    EnemyEntity enemyEntity = entityBehaviour.Entity as EnemyEntity;
                    if (enemyEntity != null)
                    {
                        PlayerEntity playerEntity = GameManager.Instance.PlayerEntity;
                        int[] allowedDrugs = {}; // Generic = 0, Gemstone = 1
                        bool silverAverse = false;
                        int DrugsAmount = 1;
                        float dropOddsMod = 1f;

                        if (enemyEntity.EntityType == EntityTypes.EnemyClass)
                        {
                            switch (enemyEntity.CareerIndex)
                            {
                                case (int)ClassCareers.Mage:
                                case (int)ClassCareers.Spellsword:
                                case (int)ClassCareers.Battlemage:
                                case (int)ClassCareers.Sorcerer:
                                case (int)ClassCareers.Healer:
                                    allowedDrugs = new int[] {0, 0, 1, 1, 4700, 4700, 4701, 4701, 4701, 4702, 4702, 4703, 4703, 4704, 4706, 4706, 4707, 4707};
                                    DrugsAmount = PickOneOf(1, 1, 1, 1, 1, 1, 2, 2, 2, 3);
                                    break;
                                case (int)ClassCareers.Nightblade:
                                case (int)ClassCareers.Bard:
                                case (int)ClassCareers.Acrobat:
                                case (int)ClassCareers.Assassin:
                                    allowedDrugs = new int[] { 0, 0, 1, 1, 4700, 4700, 4701, 4701, 4701, 4702, 4702, 4703, 4703, 4706, 4706, 4707, 4707 };
                                    DrugsAmount = PickOneOf(1, 1, 2, 2, 3);
                                    dropOddsMod = 1.5f;
                                    break;
                                case (int)ClassCareers.Burglar:
                                case (int)ClassCareers.Rogue:
                                case (int)ClassCareers.Thief:
                                    allowedDrugs = new int[] { 0, 0, 1, 1, 1, 4700, 4700, 4701, 4701, 4702, 4702, 4703, 4703, 4704, 4705, 4706, 4706, 4707, 4707 };
                                    DrugsAmount = PickOneOf(2, 2, 2, 2, 3, 3, 3, 3, 3, 4);
                                    dropOddsMod = 2f;
                                    break;
                                case (int)ClassCareers.Monk:
                                case (int)ClassCareers.Archer:
                                case (int)ClassCareers.Ranger:
                                case (int)ClassCareers.Barbarian:
                                case (int)ClassCareers.Warrior:
                                case (int)ClassCareers.Knight:
                                    allowedDrugs = new int[] { 0, 0, 1, 4700, 4700, 4701, 4701, 4701, 4702, 4702, 4703, 4703, 4706, 4706, 4707, 4707 };
                                    DrugsAmount = PickOneOf(1, 1, 1, 1, 2);
                                    break;
                                default:
                                    return;
                            }
                        }
                        else
                        {
                            switch (enemyEntity.CareerIndex)
                            {
                                case (int)MonsterCareers.Orc:
                                case (int)MonsterCareers.Zombie:
                                case (int)MonsterCareers.Giant:
                                    allowedDrugs = new int[] { 0, 0, 0, 0, 1, 4700, 4700, 4701, 4701, 4701, 4701, 4702, 4702, 4703, 4706, 4707 };
                                    dropOddsMod = 0.25f;
                                    break;
                                case (int)MonsterCareers.OrcSergeant:
                                case (int)MonsterCareers.Centaur:
                                    allowedDrugs = new int[] { 0, 0, 0, 1, 1, 4700, 4700, 4701, 4701, 4701, 4701, 4702, 4702, 4703, 4706, 4707 };
                                    dropOddsMod = 0.50f;
                                    break;
                                case (int)MonsterCareers.OrcShaman:
                                    DrugsAmount = PickOneOf(1, 2);
                                    allowedDrugs = new int[] { 0, 0, 1, 1, 4700, 4700, 4701, 4701, 4701, 4702, 4702, 4703, 4704, 4706, 4707 };
                                    dropOddsMod = 1.25f;
                                    break;
                                case (int)MonsterCareers.OrcWarlord:
                                    DrugsAmount = PickOneOf(1, 2);
                                    allowedDrugs = new int[] { 0, 0, 1, 1, 4700, 4700, 4701, 4701, 4702, 4702, 4703, 4705, 4706, 4707 };
                                    dropOddsMod = 1.5f;
                                    break;
                                case (int)MonsterCareers.DaedraLord:
                                    allowedDrugs = new int[] { 1, 1, 4700, 4700, 4701, 4701, 4702, 4702, 4703, 4703, 4705, 4706, 4707 };
                                    DrugsAmount = PickOneOf(1, 1, 2, 2, 2, 2, 2, 3);
                                    dropOddsMod = 1.75f;
                                    break;
                                case (int)MonsterCareers.DaedraSeducer:
                                    allowedDrugs = new int[] { 1, 1, 4700, 4700, 4701, 4701, 4702, 4702, 4703, 4703, 4704, 4706, 4706, 4707, 4707 };
                                    DrugsAmount = PickOneOf(2, 2, 3, 3, 3, 3, 3, 4);
                                    dropOddsMod = 2f;
                                    break;
                                case (int)MonsterCareers.Lamia:
                                    allowedDrugs = new int[] { 4700, 4700, 4701, 4701, 4702, 4702, 4703, 4703, 4704, 4706, 4706, 4707, 4707, 4707 };
                                    DrugsAmount = PickOneOf(1, 1, 2, 2, 2, 3);
                                    dropOddsMod = 1.5f;
                                    break;
                                case (int)MonsterCareers.Mummy:
                                    allowedDrugs = new int[] { 0, 0, 0, 4700, 4700, 4700, 4701, 4701, 4701, 4702, 4702, 4703, 4706, 4707 };
                                    DrugsAmount = PickOneOf(1, 1, 2, 2, 2, 3);
                                    dropOddsMod = 1.5f;
                                    silverAverse = true;
                                    break;
                                case (int)MonsterCareers.Vampire:
                                case (int)MonsterCareers.VampireAncient:
                                    allowedDrugs = new int[] { 0, 0, 1, 1, 1, 4700, 4700, 4700, 4701, 4701, 4702, 4702, 4702, 4703, 4704, 4706, 4707 };
                                    DrugsAmount = PickOneOf(1, 2, 2);
                                    dropOddsMod = 1.5f;
                                    silverAverse = true;
                                    break;
                                default:
                                    return;
                            }
                        }
                        if (Dice100.SuccessRoll((int)Mathf.Floor(5f * dropOddsMod))) // (Dice100.SuccessRoll((int)Mathf.Floor(5f * dropOddsMod)))
                        {
                            for (int i = 0; i < DrugsAmount; i++)
                            {
                                DaggerfallUnityItem item = null;
                                int chosenJewelType = PickOneOf(allowedDrugs);
                                int chosenItemIndex = -1;

                                switch (chosenJewelType)
                                {
                                    case 0:
                                        chosenItemIndex = ChooseItemFromFilteredList(new int[] { 133, 134, 135, 136, 137, 138, 139, 140 }, enemyEntity.Level, playerEntity.Stats.LiveLuck);
                                        item = ItemBuilder.CreateItem(ItemGroups.Jewellery, chosenItemIndex);
                                        item.currentCondition = (int)(UnityEngine.Random.Range(0.4f, 0.7f) * item.maxCondition);
                                        entityBehaviour.CorpseLootContainer.Items.AddItem(item);
                                        break;
                                    case 1:
                                        chosenItemIndex = RollGemstoneUsed(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, playerEntity.Stats.LiveLuck);
                                        item = ItemBuilder.CreateItem(ItemGroups.Gems, DetermineGemstone(chosenItemIndex));
                                        item.currentCondition = (int)(UnityEngine.Random.Range(0.6f, 0.9f) * item.maxCondition);
                                        entityBehaviour.CorpseLootContainer.Items.AddItem(item);
                                        break;
                                    case 4700:
                                    case 4704:
                                    case 4705:
                                        item = ItemBuilder.CreateItem(ItemGroups.Jewellery, chosenJewelType);
                                        if (silverAverse && (item.message % 2 != 0)) // Ignores this item being added if it is silver and about to be added to a silver averse creature's inventory.
                                            break;
                                        item.currentCondition = (int)(UnityEngine.Random.Range(0.4f, 0.7f) * item.maxCondition);
                                        entityBehaviour.CorpseLootContainer.Items.AddItem(item);
                                        break;
                                    case 4701:
                                    case 4702:
                                    case 4703:
                                    case 4706:
                                    case 4707:
                                        item = ItemBuilder.CreateItem(ItemGroups.Jewellery, chosenJewelType);
                                        item.currentCondition = (int)(UnityEngine.Random.Range(0.4f, 0.7f) * item.maxCondition);
                                        entityBehaviour.CorpseLootContainer.Items.AddItem(item);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static int DetermineGemstone(int gemIndex)
        {
            switch (gemIndex)
            {
                case 1:
                    return 0; // Ruby
                case 2:
                    return 1; // Emerald
                case 3:
                    return 2; // Sapphire
                case 4:
                    return 3; // Diamond
                case 5:
                    return 4708; // Amethyst
                case 6:
                    return 4709; // Apatite
                case 7:
                    return 4710; // Aquamarine
                case 8:
                    return 4711; // Garnet
                case 9:
                    return 4712; // Topaz
                case 10:
                    return 4713; // Zircon
                case 11:
                    return 4714; // Spinel
                default:
                    return 0; // Invalid/Default to Ruby
            }
        }

        public static int GetRandomVariantType(bool hasSilver = false, bool hasUngemmed = false, int totalVariants = 0)
        {
            PlayerEntity playerEntity = GameManager.Instance.PlayerEntity;
            int playerLuck = playerEntity.Stats.LiveLuck;
            List<int> variantList = new List<int>();

            if (hasSilver)
            {
                int silverOrGold = PickOneOf(0, 0, 0, 0, 0, 0, 1, 1, 1, 1); // Silver = 0, Gold = 1.
                if (silverOrGold == 0)
                {
                    for (int i = 0; i < totalVariants; i++)
                    {
                        if (i % 2 == 0) { } // Do nothing
                        else
                            variantList.Add(i);
                    }
                    return RollGemstoneUsed(variantList.ToArray(), playerLuck, -1, hasUngemmed);
                }
                else
                {
                    for (int i = 0; i < totalVariants + 1; i++)
                    {
                        if (i != 0 && i % 2 == 0)
                            variantList.Add(i);
                    }
                    return RollGemstoneUsed(variantList.ToArray(), playerLuck, -1, hasUngemmed);
                }
            }
            else
            {
                for (int i = 0; i < totalVariants; i++)
                {
                    variantList.Add(i);
                }
                return RollGemstoneUsed(variantList.ToArray(), playerLuck, -1, hasUngemmed);
            }
        }

        public static int RollGemstoneUsed(int[] itemList, int playerLuck = -1, int shopQuality = -1, bool hasUngemmed = false)
        {
            int[] GemRarities = new int[] { 12, 15, 14, 17, 5, 2, 8, 3, 7, 11, 10 };
            int[] itemRolls = new int[] { };
            List<int> itemRollsList = new List<int>();

            if (hasUngemmed)
                GemRarities = new int[] { 1, 12, 15, 14, 17, 5, 2, 8, 3, 7, 11, 10 };

            for (int i = 0; i < itemList.Length; i++)
            {
                int itemRarity = ((GemRarities[i] * 5) - 101) * -1; // This is to "flip" the rarity values, so 100 will become 1 and 1 will become 100. 
                int arrayStart = itemRollsList.Count;
                int fillElements = 0;
                if (shopQuality != -1)
                {
                    float luckMod = (playerLuck - 50) / 5f;
                    float qualityMod = shopQuality;

                    if (itemRarity >= 60)
                    {
                        fillElements = (int)Mathf.Clamp(Mathf.Ceil((itemRarity * 2.5f) + ((qualityMod + luckMod) * 2)), 1, 400);
                    }
                    else if (itemRarity >= 35)
                    {
                        fillElements = (int)Mathf.Clamp(Mathf.Ceil((itemRarity * 1.5f) + (qualityMod + luckMod)), 1, 400);
                    }
                    else
                    {
                        fillElements = (int)Mathf.Clamp(Mathf.Ceil(itemRarity - (qualityMod + luckMod)), 1, 400);
                    }
                }
                else
                {
                    float luckMod = (playerLuck - 50) / 5f;

                    if (itemRarity >= 60)
                    {
                        fillElements = (int)Mathf.Clamp(Mathf.Ceil((itemRarity * 2.5f) + (luckMod * 2)), 1, 400);
                    }
                    else if (itemRarity >= 35)
                    {
                        fillElements = (int)Mathf.Clamp(Mathf.Ceil((itemRarity * 1.5f) + luckMod), 1, 400);
                    }
                    else
                    {
                        fillElements = (int)Mathf.Clamp(Mathf.Ceil(itemRarity - luckMod), 1, 400);
                    }
                }

                itemRolls = FillArray(itemRollsList, arrayStart, fillElements, i);
            }

            int chosenItemIndex = -1;

            if (itemRolls.Length > 0)
                chosenItemIndex = PickOneOf(itemRolls);

            if (chosenItemIndex == -1)
                return -1;

            return itemList[chosenItemIndex];
        }

        public static int ChooseItemFromFilteredList(int[] itemList, int enemyLevel = -1, int playerLuck = -1)
        {
            int[] itemRolls = new int[] { };
            List<int> itemRollsList = new List<int>();

            for (int i = 0; i < itemList.Length; i++)
            {
                int itemRarity = ((2 * 5) - 101) * -1; // This is to "flip" the rarity values, so 100 will become 1 and 1 will become 100. 
                int arrayStart = itemRollsList.Count;
                int fillElements = 0;
                if (enemyLevel != -1)
                {
                    if (itemRarity >= 60)
                    {
                        if (enemyLevel >= 21)
                            fillElements = (int)Mathf.Clamp(Mathf.Ceil(itemRarity - (enemyLevel / 1.5f)), 1, 400);
                        else if (enemyLevel >= 11)
                            fillElements = (int)Mathf.Clamp(Mathf.Ceil(itemRarity - (enemyLevel / 2.5f)), 1, 400);
                        else
                            fillElements = (int)Mathf.Clamp(Mathf.Ceil(itemRarity + (60f / enemyLevel)), 1, 400);
                    }
                    else if (itemRarity >= 35)
                    {
                        if (enemyLevel >= 21)
                            fillElements = (int)Mathf.Clamp(Mathf.Ceil(itemRarity + enemyLevel), 1, 400);
                        else if (enemyLevel >= 11)
                            fillElements = (int)Mathf.Clamp(Mathf.Ceil(itemRarity + (enemyLevel / 2f)), 1, 400);
                        else
                            fillElements = (int)Mathf.Clamp(Mathf.Ceil(itemRarity - (5f / enemyLevel)), 1, 400);
                    }
                    else
                    {
                        if (enemyLevel >= 21)
                            fillElements = (int)Mathf.Clamp(Mathf.Ceil(itemRarity + (enemyLevel / 2f)), 1, 400);
                        else if (enemyLevel >= 11)
                            fillElements = (int)Mathf.Clamp(Mathf.Ceil(itemRarity - (5f / enemyLevel)), 1, 400);
                        else
                            fillElements = (int)Mathf.Clamp(Mathf.Ceil(itemRarity - (5f / enemyLevel)), 1, 400);
                    }
                }
                else
                {
                    float luckMod = (playerLuck - 50) / 5f;

                    if (itemRarity >= 60)
                    {
                        fillElements = (int)Mathf.Clamp(Mathf.Ceil((itemRarity * 2.5f) + (luckMod * 2)), 1, 400);
                    }
                    else if (itemRarity >= 35)
                    {
                        fillElements = (int)Mathf.Clamp(Mathf.Ceil((itemRarity * 1.5f) + luckMod), 1, 400);
                    }
                    else
                    {
                        fillElements = (int)Mathf.Clamp(Mathf.Ceil(itemRarity - luckMod), 1, 400);
                    }
                }

                itemRolls = FillArray(itemRollsList, arrayStart, fillElements, i);
            }

            int chosenItemIndex = -1;

            if (itemRolls.Length > 0)
                chosenItemIndex = PickOneOf(itemRolls);

            if (chosenItemIndex == -1)
                return -1;

            return itemList[chosenItemIndex]; // may need to -1 from this index?
        }

        public static T[] FillArray<T>(List<T> list, int start, int count, T value)
        {
            for (var i = start; i < start + count; i++)
            {
                list.Add(value);
            }

            return list.ToArray();
        }

        public static int PickOneOf(params int[] values) // Pango provided assistance in making this much cleaner way of doing the random value choice part, awesome.
        {
            return values[UnityEngine.Random.Range(0, values.Length)];
        }

        public static void RegisterJACommands()
        {
            Debug.Log("[UsableDrugs] Trying to register console commands.");
            try
            {
                ConsoleCommandsDatabase.RegisterCommand(AddJAItems.command, AddJAItems.description, AddJAItems.usage, AddJAItems.Execute);
                ConsoleCommandsDatabase.RegisterCommand(ListRegions.command, ListRegions.description, ListRegions.usage, ListRegions.Execute);
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("Error Registering UsableDrugs Console commands: {0}", e.Message));
            }
        }

        private static class AddJAItems
        {
            public static readonly string command = "ja_Drugs";
            public static readonly string description = "Adds one of each Drugs Additions Items to player's inventory (may need to run multiple times to get specific variant.)";
            public static readonly string usage = "ja_Drugs";

            public static string Execute(params string[] args)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                PlayerEntity playerEntity = player.GetComponent<DaggerfallEntityBehaviour>().Entity as PlayerEntity;
                ItemCollection items = playerEntity.Items;
                DaggerfallUnityItem newItem = null;
                int[] allowedDrugs = { 4700, 4701, 4702, 4703, 4704, 4705, 4706, 4707, 0, 1, 2, 3, 4708, 4709, 4710, 4711, 4712, 4713, 4714 };

                for (int i = 0; i < allowedDrugs.Length; i++)
                {
                    if (allowedDrugs[i] >= 4700 && allowedDrugs[i] < 4708)
                    {
                        newItem = ItemBuilder.CreateItem(ItemGroups.Jewellery, allowedDrugs[i]);
                        items.AddItem(newItem);
                    }
                    else
                    {
                        newItem = ItemBuilder.CreateItem(ItemGroups.Gems, allowedDrugs[i]);
                        items.AddItem(newItem);
                    }
                }

                return "Drugs Items added";
            }
        }

        private static class ListRegions
        {
            public static readonly string command = "listallregions";
            public static readonly string description = "Lists all regions.)";
            public static readonly string usage = "listallregions";

            public static string Execute(params string[] args)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                PlayerEntity playerEntity = player.GetComponent<DaggerfallEntityBehaviour>().Entity as PlayerEntity;

                for (int i = 0; i < 80; i++)
                {
                    DFRegion regionInfo = DaggerfallUnity.Instance.ContentReader.MapFileReader.GetRegion(i);
                    Debug.LogFormat("Region Index # {0} named: {1}", i, regionInfo.Name);
                }

                return "All regions listed.";
            }
        }
    }
}
