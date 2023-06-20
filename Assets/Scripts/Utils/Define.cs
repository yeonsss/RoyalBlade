using System.Collections.Generic;

namespace Utils
{
    public static class Define
    {
        public enum UIEvent
        {
            Click,
            Pressed,
            PointerDown, PointerUp,
            ValueChange
        }

        public static string[] backGroundPaths = new string[5]
        {
            "BackGround/BackGround-1",
            "BackGround/BackGround-2",
            "BackGround/BackGround-3",
            "BackGround/BackGround-4",
            "BackGround/BackGround-5"
        };

        public const float attackSpeed = 0.35f;
        public const int baseLifePoint = 3;
        public const int specialJumpHeight = 10;

        public const string CHARACTER_STAT_PATH = "Data/CharacterStat";
        public const string ITEM_STAT_PATH = "Data/ItemStat";
        public const string MONSTER_STAT_PATH = "Data/MonsterStat";
        public const string STAGE_INFO_PATH = "Data/StageInfo";
        public const string WEAPON_STAT_PATH = "Data/WeaponStat";
    }
}