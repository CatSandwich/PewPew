using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UpgradeGroup : IEnumerable<Player.Upgrades.Upgrade>
    {
        public string Header;
        public List<Player.Upgrades.Upgrade> Upgrades = new List<Player.Upgrades.Upgrade>();

        public UpgradeGroup(string header)
        {
            Header = header;
        }
        
        public void Add(Player.Upgrades.Upgrade upgrade) 
            => Upgrades.Add(upgrade);
        
        public IEnumerator<Player.Upgrades.Upgrade> GetEnumerator() => Upgrades.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Upgrades.GetEnumerator();
    }
}
