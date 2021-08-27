using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Upgrades
{
    public abstract class Upgrade<T> : ScriptableObject
    {
        public string Name;
        public string Key;

        public int ActiveIndex
        {
            get => PlayerPrefs.GetInt(Key, 0);
            set
            {
                #if UNITY_EDITOR
                if (value < 0 || value >= Tiers.Count)
                    UnityEngine.Debug.LogError($"Tried setting upgrade {Name} to level {value}");
                else
                #endif
                    PlayerPrefs.SetInt(Key, value);
            }
        }
        public UpgradeTier<T> Active => Tiers[ActiveIndex];
        public T Value => Active.Value;
        
        public List<UpgradeTier<T>> Tiers;

        public void Debug()
        {
            if (ActiveIndex == Tiers.Count - 1) ActiveIndex = 0;
            else ActiveIndex++;
        }
    }
}
