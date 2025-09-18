using System.Collections.Generic;
using Person;

namespace DefaultNamespace
{
    public class FlagManager
    {
        private List<Flag> flagList = new List<Flag>();
        private Buffalo.Buffalo buffalo;
        private Player player;
        
        public FlagManager(Buffalo.Buffalo buffalo, Player player)
        {
            this.buffalo = buffalo;
            this.player = player;
            buffalo.FlagsInField = new List<Flag>();
            player.FlagsInField = new List<Flag>();
        }
        
        public void AddFlag(Flag flag)
        {
            flagList.Add(flag);
            buffalo.FlagsInField.Add(flag);
            player.FlagsInField.Add(flag);
        }
        public List<Flag> GetFlags()
        {
            return flagList;
        }
        
        public void PopFlag(Flag flag)
        {
            flagList.Remove(flag);
            buffalo.FlagsInField.Remove(flag);
            player.FlagsInField.Remove(flag);
        }
    }
}