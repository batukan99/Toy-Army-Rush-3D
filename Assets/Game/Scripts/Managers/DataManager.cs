using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Core.Events;
using Game.Core.Enums;
using Game.Core.Constants;
using EventType = Game.Core.Enums.EventType;

namespace Game.Managers
{
    public class DataManager : MonoSingleton<DataManager>, IProvidable
    {
        private readonly string LevelData = "data_level";
        private readonly string MoneyData = "data_money";
        private readonly string DamageUpgradeData = "data_upgrade_damage";
        private readonly string IncomeUpgradeData = "data_upgrade_income";
        private readonly string HealthUpgradeData = "data_upgrade_health";

        public int Level { get; private set; }
        public int Money { get; private set; }
        public int DamageUpgrade { get; private set; }
        public int IncomeUpgrade { get; private set; }
        public int HealthUpgrade { get; private set; }

        public void GetDatas()
        {
            Level = PlayerPrefs.GetInt(LevelData, 1);
            Money = PlayerPrefs.GetInt(MoneyData, GameSettings.StartingMoney);
            DamageUpgrade = PlayerPrefs.GetInt(DamageUpgradeData, 1);
            IncomeUpgrade = PlayerPrefs.GetInt(IncomeUpgradeData, 1);
            HealthUpgrade = PlayerPrefs.GetInt(HealthUpgradeData, 1);
        }
        public int GetUpgradeButtonLevel(UpgradeButtonType upgradeButtonType) 
        {
            int level = 0;
            switch(upgradeButtonType)
            {
                case UpgradeButtonType.DamageUpgrade:
                level = DamageUpgrade;
                break;
                
                case UpgradeButtonType.IncomeUpgrade:
                level = IncomeUpgrade;
                break;

                case UpgradeButtonType.HealthUpgrade:
                level = HealthUpgrade;
                break;
            }
            return level;
        }
        
        public void SetLevel(int _level)
        {
            Level = _level;
            PlayerPrefs.SetInt(LevelData, Level);
            PlayerPrefs.Save();
        }
        public void SetMoney(int _money)
        {
            Money = _money;
            PlayerPrefs.SetInt(MoneyData, _money);
            PlayerPrefs.Save();
        }

        public void SetIncomeUpgrade(int _incomeUpgrade)
        {
            IncomeUpgrade = _incomeUpgrade;
            PlayerPrefs.SetInt(IncomeUpgradeData, _incomeUpgrade);
            PlayerPrefs.Save();
        }
        public void SetUpgradeButtonLevel(UpgradeButtonType upgradeButtonType) 
        {
            int level = GetUpgradeButtonLevel(upgradeButtonType) + 1;
            switch(upgradeButtonType)
            {
                case UpgradeButtonType.DamageUpgrade:
                DamageUpgrade = level;
                PlayerPrefs.SetInt(DamageUpgradeData, level);
                break;
                
                case UpgradeButtonType.IncomeUpgrade:
                IncomeUpgrade = level;
                PlayerPrefs.SetInt(IncomeUpgradeData, level);
                break;

                case UpgradeButtonType.HealthUpgrade:
                HealthUpgrade = level;
                PlayerPrefs.SetInt(HealthUpgradeData, level);
                break;
            }
            PlayerPrefs.Save();
        }
        
        private void OnEnable()
        {
            ManagerProvider.Register(this);
            DontDestroyOnLoad(this);
        }
    }
}