using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.PlayerServices;

namespace BannerRoyalMPLib
{
    public static class ViewModelLib
    {
        // Token: 0x06000904 RID: 2308 RVA: 0x00027414 File Offset: 0x00025614
        public static List<TooltipProperty> TryAddToolTip(this List<TooltipProperty> toolTipList, string toolTipName, object toolTipValue, string prefix = "", string suffix = "", int textHeight = 0, TooltipProperty.TooltipPropertyFlags flags = TooltipProperty.TooltipPropertyFlags.None)
        {
            TooltipProperty toolTip = ViewModelLib.TryAddToolTip(toolTipName, toolTipValue, prefix, suffix, textHeight, flags);
            bool flag = toolTip != null;
            if (flag)
            {
                toolTipList.Add(toolTip);
            }
            return toolTipList;
        }

        // Token: 0x06000905 RID: 2309 RVA: 0x00027448 File Offset: 0x00025648
        public static TooltipProperty TryAddToolTip(string toolTipName, object toolTipValue, string prefix = "", string suffix = "", int textHeight = 0, TooltipProperty.TooltipPropertyFlags flags = TooltipProperty.TooltipPropertyFlags.None)
        {
            string stringValue = toolTipValue.ToString();
            bool flag = stringValue.Trim() == "Invalid";
            if (flag)
            {
                stringValue = "";
            }
            bool flag2 = prefix.Trim() == "Invalid";
            if (flag2)
            {
                prefix = "";
            }
            bool flag3 = suffix.Trim() == "Invalid";
            if (flag3)
            {
                suffix = "";
            }
            bool flag4 = stringValue.Length <= 0;
            TooltipProperty result;
            if (flag4)
            {
                result = null;
            }
            else
            {
                bool flag5 = toolTipValue.GetType() == typeof(float);
                if (flag5)
                {
                    float parsedValue = -1f;
                    float.TryParse(stringValue, out parsedValue);
                    bool flag6 = parsedValue <= 0f;
                    if (flag6)
                    {
                        return null;
                    }
                    parsedValue = (float)Math.Round((double)parsedValue, 2);
                    stringValue = parsedValue.ToString();
                }
                bool flag7 = toolTipValue.GetType() == typeof(int);
                if (flag7)
                {
                    int parsedValue2 = -1;
                    int.TryParse(stringValue, out parsedValue2);
                    bool flag8 = parsedValue2 <= 0;
                    if (flag8)
                    {
                        return null;
                    }
                }
                result = new TooltipProperty(toolTipName, prefix + stringValue + suffix, textHeight, false, flags);
            }
            return result;
        }

        // Token: 0x06000906 RID: 2310 RVA: 0x00027578 File Offset: 0x00025778
        public static TooltipProperty AddSeperator(TooltipProperty.TooltipPropertyFlags seperatorType)
        {
            return new TooltipProperty("", "", 0, false, seperatorType);
        }

        // Token: 0x06000907 RID: 2311 RVA: 0x0002759C File Offset: 0x0002579C
        public static TooltipProperty AddBlank()
        {
            return new TooltipProperty(" ", " ", 0, false, TooltipProperty.TooltipPropertyFlags.None);
        }

        // Token: 0x06000908 RID: 2312 RVA: 0x000275C0 File Offset: 0x000257C0
        public static void AddTextLine(this MBBindingList<GenericVM<string>> textLineList, string text)
        {
            textLineList.Add(new GenericVM<string>(text));
        }

        // Token: 0x06000909 RID: 2313 RVA: 0x000275D0 File Offset: 0x000257D0
        public static void AddMultiTextLine(this MBBindingList<GenericVM<string>> textLineList, string text, int maxCharsPerLine)
        {
            List<string> textLines = text.FormatMultiLineString(maxCharsPerLine);
            foreach (string line in textLines)
            {
                textLineList.AddTextLine(line);
            }
        }

        // Token: 0x0600090A RID: 2314 RVA: 0x0002762C File Offset: 0x0002582C
        public static List<string> FormatMultiLineString(this string stringObject, int maxCharsPerLine)
        {
            List<string> stringLines = new List<string>();
            int lineCount = TaleWorlds.Library.MathF.Ceiling((float)stringObject.Length / (float)maxCharsPerLine);
            string[] stringParts = stringObject.Split(new char[]
            {
                ' '
            });
            string currentLine = "";
            foreach (string stringPart in stringParts)
            {
                string tryNewLine = currentLine + " " + stringPart;
                tryNewLine = tryNewLine.Trim();
                bool flag = tryNewLine.Length <= maxCharsPerLine;
                if (flag)
                {
                    currentLine = tryNewLine;
                }
                else
                {
                    stringLines.Add(currentLine);
                    currentLine = stringPart;
                }
                bool flag2 = stringPart == stringParts.Last<string>();
                if (flag2)
                {
                    stringLines.Add(currentLine);
                }
            }
            return stringLines;
        }

        // Token: 0x0600090B RID: 2315 RVA: 0x000276EC File Offset: 0x000258EC
        public static string FormatAddSpaces(this string stringObject)
        {
            stringObject = Regex.Replace(stringObject, "(\\B[A-Z])", " $1");
            return stringObject;
        }

        // Token: 0x0600090C RID: 2316 RVA: 0x00027714 File Offset: 0x00025914
        public static ImageIdentifier TryGetImageIdentifierType(object genericObject, object objectArg = null)
        {
            bool flag = genericObject is BasicCharacterObject;
            ImageIdentifier result;
            if (flag)
            {
                result = new ImageIdentifier(CharacterCode.CreateFrom(genericObject as BasicCharacterObject));
            }
            else
            {
                bool flag2 = genericObject is CharacterCode;
                if (flag2)
                {
                    result = new ImageIdentifier(genericObject as CharacterCode);
                }
                else
                {
                    bool flag3 = genericObject is Banner;
                    if (flag3)
                    {
                        result = new ImageIdentifier(genericObject as Banner);
                    }
                    else
                    {
                        bool flag4 = genericObject is ItemObject;
                        if (flag4)
                        {
                            result = new ImageIdentifier(genericObject as ItemObject, "");
                        }
                        else
                        {
                            bool flag5 = genericObject is CraftingPiece;
                            if (flag5)
                            {
                                result = new ImageIdentifier(genericObject as CraftingPiece, objectArg as string);
                            }
                            else
                            {
                                bool flag6 = genericObject is BannerCode;
                                if (flag6)
                                {
                                    result = new ImageIdentifier(genericObject as BannerCode, (bool)objectArg);
                                }
                                else
                                {
                                    bool flag7 = genericObject is PlayerId;
                                    if (flag7)
                                    {
                                        result = new ImageIdentifier((PlayerId)genericObject, (int)objectArg);
                                    }
                                    else
                                    {
                                        result = null;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        // Token: 0x0600090D RID: 2317 RVA: 0x00027814 File Offset: 0x00025A14
        public static string FormatWeaponClassType(WeaponComponent weaponData, bool isPlural = true)
        {
            WeaponClass weaponClass = weaponData.PrimaryWeapon.WeaponClass;
            string defaultWeaponClass = weaponClass.ToString();
            defaultWeaponClass = defaultWeaponClass.FormatAddSpaces();
            bool flag = weaponData.PrimaryWeapon.WeaponDescriptionId == "OneHandedBastardAxe";
            if (flag)
            {
                defaultWeaponClass = "Bastard Axe";
            }
            else
            {
                bool flag2 = weaponData.PrimaryWeapon.WeaponDescriptionId == "OneHandedBastardSword";
                if (flag2)
                {
                    defaultWeaponClass = "Bastard Sword";
                }
            }
            bool flag3 = weaponClass == WeaponClass.LargeShield || weaponClass == WeaponClass.SmallShield;
            if (flag3)
            {
                defaultWeaponClass = "Shield";
            }
            if (isPlural)
            {
                bool flag4 = weaponClass == WeaponClass.ThrowingKnife;
                if (flag4)
                {
                    defaultWeaponClass = "Throwing Knives";
                }
                else
                {
                    defaultWeaponClass += "s";
                }
            }
            return defaultWeaponClass;
        }

        // Token: 0x0600090E RID: 2318 RVA: 0x000278D0 File Offset: 0x00025AD0
        public static string FormatItemObjectType(ItemObject.ItemTypeEnum itemType, bool isPlural = true)
        {
            string defaultItemType = itemType.ToString();
            bool flag = itemType == ItemObject.ItemTypeEnum.Arrows || itemType == ItemObject.ItemTypeEnum.Bolts;
            string result;
            if (flag)
            {
                result = itemType.ToString();
            }
            else
            {
                bool flag2 = itemType == ItemObject.ItemTypeEnum.Cape;
                if (flag2)
                {
                    defaultItemType = "Shoulder Armor";
                }
                bool flag3 = itemType == ItemObject.ItemTypeEnum.Thrown;
                if (flag3)
                {
                    defaultItemType = "Throwing Weapon";
                }
                bool flag4 = itemType == ItemObject.ItemTypeEnum.Horse;
                if (flag4)
                {
                    defaultItemType = "Mount";
                }
                bool flag5 = itemType == ItemObject.ItemTypeEnum.HorseHarness;
                if (flag5)
                {
                    defaultItemType = "Mount Armor";
                }
                defaultItemType = defaultItemType.FormatAddSpaces();
                if (isPlural)
                {
                    defaultItemType += "s";
                }
                result = defaultItemType;
            }
            return result;
        }

        // Token: 0x0600090F RID: 2319 RVA: 0x0002796C File Offset: 0x00025B6C
        public static ButtonObjectListVM<T> AddObjectToGroupedList<T>(this MBBindingList<ButtonObjectListVM<T>> groupedList, string groupName, ButtonObjectVM<T> objectToAdd)
        {
            ButtonObjectListVM<T> childListVM = groupedList.FirstOrDefault((ButtonObjectListVM<T> list) => list.ListName == groupName);
            bool flag = childListVM == null;
            if (flag)
            {
                childListVM = new ButtonObjectListVM<T>();
                childListVM.ListName = groupName;
                bool flag2 = groupName == "Infantry" || groupName == "Ranged" || groupName == "Cavalry";
                if (flag2)
                {
                    childListVM.ListIconName = "icon_class_" + groupName.ToLower();
                }
                groupedList.Add(childListVM);
            }
            childListVM.ObjectsList.Add(objectToAdd);
            return childListVM;
        }

        // Token: 0x06000910 RID: 2320 RVA: 0x00027A2C File Offset: 0x00025C2C
        public static bool IsThrowing(this WeaponComponentData weapon)
        {
            return weapon != null && ViewModelLib.ThrowingClasses.Contains(weapon.WeaponClass);
        }

        // Token: 0x06000911 RID: 2321 RVA: 0x00027A60 File Offset: 0x00025C60
        public static bool IsBowXbow(this WeaponComponentData weapon)
        {
            return weapon != null && ViewModelLib.BowClasses.Contains(weapon.WeaponClass);
        }

        // Token: 0x06000912 RID: 2322 RVA: 0x00027A94 File Offset: 0x00025C94
        public static bool IsShield(this WeaponComponentData weapon)
        {
            return weapon != null && ViewModelLib.ShieldClasses.Contains(weapon.WeaponClass);
        }

        // Token: 0x06000913 RID: 2323 RVA: 0x00027AC8 File Offset: 0x00025CC8
        public static bool IsAmmo(this WeaponComponentData weapon)
        {
            return weapon != null && ViewModelLib.AmmoClasses.Contains(weapon.WeaponClass);
        }

        // Token: 0x06000914 RID: 2324 RVA: 0x00027AFC File Offset: 0x00025CFC
        public static List<SkillObject> GetAllSkillObjects()
        {
            List<SkillObject> skills = new List<SkillObject>();
            skills.AppendList(ViewModelLib.MeleeSkills);
            skills.AppendList(ViewModelLib.RangedSkills);
            skills.AppendList(ViewModelLib.MovementSkills);
            return skills;
        }

        // Token: 0x04000319 RID: 793
        public static readonly Color DisabledColor = new Color(1f, 0.2f, 0.2f, 1f);

        // Token: 0x0400031A RID: 794
        public static readonly Color EnabledColor = new Color(0.977f, 0.953f, 0.867f, 1f);

        // Token: 0x0400031B RID: 795
        public static readonly Color AlliedColor = new Color(0.44705883f, 0.68235296f, 1f, 1f);

        // Token: 0x0400031C RID: 796
        public static readonly Color EnemyColor = new Color(1f, 0.49019608f, 0.49019608f, 1f);

        // Token: 0x0400031D RID: 797
        public static readonly Color NeutralColor = new Color(0.6666667f, 0.6666667f, 0.6666667f, 1f);

        // Token: 0x0400031E RID: 798
        public static readonly Color BuffColor = new Color(0.293f, 0.746f, 0.414f, 1f);

        // Token: 0x0400031F RID: 799
        public static readonly Color DebuffColor = new Color(0.957f, 0.461f, 0.211f, 1f);

        // Token: 0x04000320 RID: 800
        public static readonly Dictionary<WeaponFlags, Color> WeaponFlagsToDisplay = new Dictionary<WeaponFlags, Color>
        {
            {
                WeaponFlags.BonusAgainstShield,
                ViewModelLib.BuffColor
            },
            {
                WeaponFlags.CanPenetrateShield,
                ViewModelLib.BuffColor
            },
            {
                WeaponFlags.CantReloadOnHorseback,
                ViewModelLib.DebuffColor
            },
            {
                WeaponFlags.CanKnockDown,
                ViewModelLib.BuffColor
            },
            {
                WeaponFlags.CanCrushThrough,
                ViewModelLib.BuffColor
            },
            {
                WeaponFlags.MultiplePenetration,
                ViewModelLib.BuffColor
            }
        };

        // Token: 0x04000321 RID: 801
        public static readonly Dictionary<EquipmentIndex, ItemObject.ItemTypeEnum> ItemTypeForEquipmentIndex = new Dictionary<EquipmentIndex, ItemObject.ItemTypeEnum>
        {
            {
                EquipmentIndex.NumAllWeaponSlots,
                ItemObject.ItemTypeEnum.HeadArmor
            },
            {
                EquipmentIndex.Cape,
                ItemObject.ItemTypeEnum.Cape
            },
            {
                EquipmentIndex.Body,
                ItemObject.ItemTypeEnum.BodyArmor
            },
            {
                EquipmentIndex.Gloves,
                ItemObject.ItemTypeEnum.HandArmor
            },
            {
                EquipmentIndex.Leg,
                ItemObject.ItemTypeEnum.LegArmor
            },
            {
                EquipmentIndex.ArmorItemEndSlot,
                ItemObject.ItemTypeEnum.Horse
            },
            {
                EquipmentIndex.HorseHarness,
                ItemObject.ItemTypeEnum.HorseHarness
            }
        };

        public static EquipmentIndex GetItemEquipmentIndex(ItemObject item)
        {
            if(ItemObject.ItemTypeEnum.HeadArmor == item.ItemType)
            {
                return EquipmentIndex.NumAllArmorSlots;
            }
            else if (ItemObject.ItemTypeEnum.Cape == item.ItemType)
            {
                return EquipmentIndex.Cape;
            }
            else if (ItemObject.ItemTypeEnum.BodyArmor == item.ItemType)
            {
                return EquipmentIndex.Body;
            }
            else if (ItemObject.ItemTypeEnum.HandArmor == item.ItemType)
            {
                return EquipmentIndex.Gloves;
            }
            else if (ItemObject.ItemTypeEnum.LegArmor == item.ItemType)
            {
                return EquipmentIndex.Leg;
            }
            else if (WeaponFilter.Contains(item.ItemType)) 
            {
                return EquipmentIndex.Weapon0;
            }
            else
            {
                return EquipmentIndex.None;
            }
        }

        public static List<ItemObject.ItemTypeEnum> ArmorFilter = new List<ItemObject.ItemTypeEnum>
        {
            ItemObject.ItemTypeEnum.HeadArmor,
            ItemObject.ItemTypeEnum.BodyArmor,
            ItemObject.ItemTypeEnum.HandArmor,
            ItemObject.ItemTypeEnum.LegArmor,
        };

        // Token: 0x04000322 RID: 802
        public static List<ItemObject.ItemTypeEnum> WeaponFilter = new List<ItemObject.ItemTypeEnum>
        {
            ItemObject.ItemTypeEnum.OneHandedWeapon,
            ItemObject.ItemTypeEnum.TwoHandedWeapon,
            ItemObject.ItemTypeEnum.Polearm,
            ItemObject.ItemTypeEnum.Shield,
            ItemObject.ItemTypeEnum.Bow,
            ItemObject.ItemTypeEnum.Crossbow,
            ItemObject.ItemTypeEnum.Thrown,
            ItemObject.ItemTypeEnum.Arrows,
            ItemObject.ItemTypeEnum.Bolts
        };

        // Token: 0x04000323 RID: 803
        public static List<ItemObject.ItemTypeEnum> PrimaryWeaponFilter = new List<ItemObject.ItemTypeEnum>
        {
            ItemObject.ItemTypeEnum.OneHandedWeapon,
            ItemObject.ItemTypeEnum.TwoHandedWeapon,
            ItemObject.ItemTypeEnum.Polearm,
            ItemObject.ItemTypeEnum.Bow,
            ItemObject.ItemTypeEnum.Crossbow
        };

        // Token: 0x04000324 RID: 804
        public static readonly List<WeaponClass> SwingingClasses = new List<WeaponClass>
        {
            WeaponClass.Dagger,
            WeaponClass.OneHandedSword,
            WeaponClass.TwoHandedSword,
            WeaponClass.OneHandedAxe,
            WeaponClass.TwoHandedAxe,
            WeaponClass.Mace,
            WeaponClass.Pick,
            WeaponClass.TwoHandedMace,
            WeaponClass.OneHandedPolearm,
            WeaponClass.TwoHandedPolearm,
            WeaponClass.LowGripPolearm
        };

        // Token: 0x04000325 RID: 805
        public static readonly List<WeaponClass> ThrustClasses = new List<WeaponClass>
        {
            WeaponClass.Dagger,
            WeaponClass.OneHandedSword,
            WeaponClass.TwoHandedSword,
            WeaponClass.OneHandedPolearm,
            WeaponClass.TwoHandedPolearm,
            WeaponClass.LowGripPolearm
        };

        // Token: 0x04000326 RID: 806
        public static readonly List<WeaponClass> ThrowingClasses = new List<WeaponClass>
        {
            WeaponClass.ThrowingAxe,
            WeaponClass.ThrowingKnife,
            WeaponClass.Javelin,
            WeaponClass.Stone
        };

        // Token: 0x04000327 RID: 807
        public static readonly List<WeaponClass> BowClasses = new List<WeaponClass>
        {
            WeaponClass.Crossbow,
            WeaponClass.Bow
        };

        // Token: 0x04000328 RID: 808
        public static readonly List<WeaponClass> ShieldClasses = new List<WeaponClass>
        {
            WeaponClass.SmallShield,
            WeaponClass.LargeShield
        };

        // Token: 0x04000329 RID: 809
        public static readonly List<WeaponClass> AmmoClasses = new List<WeaponClass>
        {
            WeaponClass.Arrow,
            WeaponClass.Bolt,
            WeaponClass.Cartridge
        };

        // Token: 0x0400032A RID: 810
        public static List<SkillObject> MeleeSkills = new List<SkillObject>
        {
            DefaultSkills.OneHanded,
            DefaultSkills.TwoHanded,
            DefaultSkills.Polearm,
            DefaultSkills.Athletics
        };

        // Token: 0x0400032B RID: 811
        public static List<SkillObject> RangedSkills = new List<SkillObject>
        {
            DefaultSkills.Throwing,
            DefaultSkills.Bow,
            DefaultSkills.Crossbow,
            DefaultSkills.Riding
        };

        // Token: 0x0400032C RID: 812
        public static List<SkillObject> MovementSkills = new List<SkillObject>();
    }
}
