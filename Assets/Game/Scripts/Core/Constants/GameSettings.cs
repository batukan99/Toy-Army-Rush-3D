using UnityEngine;

namespace Game.Core.Constants
{
    public static class GameSettings
    {
        public const int StartingMoney = 100;
        public const int CollectableMoneyValue = 20;

        // Upgrade Settings
        public const int BaseUpgradeCost = 20;
        public const int AdditionalUpgradeCostPerLevel = 250;
        public const int IncomeUpgradeValue = 50;

        public static int GetUpgradeCost(int level)
        {
            // Multiply upgrade cost every 2 level
            int multiplier = 1 + Mathf.FloorToInt(level / 2);

            return BaseUpgradeCost + (level * multiplier * AdditionalUpgradeCostPerLevel);
        }
    }
}
