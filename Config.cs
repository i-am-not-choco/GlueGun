using System.Collections.Generic;

namespace GlueGun
{
    public class Config 
    {
        public bool NeedAllPermsInList { get; set; } = false;
        public List<string> PermsNeedToGive { get; set; }= ["CustomItemPerms","GlueGunPerm","owner"];
    }
}