using System;
using CommandSystem;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Attachments;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;


namespace GlueGun.Commands
{
   
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class GiveCommand : ICommand
    {
        public string Command { get; } = "GlueGun";
        public string[] Aliases { get; } = ["GG"]; 
        public string Description { get; } = "Gives you Glue-Gun, LEts you make a AdminToy a child to something, Shoot to choose AdminToys can have more then one, Drop to choose parent have flashlight if you want to choose a player, throw to reset values  ";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender == null)
            {
                response = "null player";
                return false;
            }
            if (!sender.HasPermissions(Plugin.Instance.Config.PermsNeedToGive.ToArray())  && Plugin.Instance.Config.NeedAllPermsInList || sender.HasPermissions(Plugin.Instance.Config.PermsNeedToGive.ToArray())  && !Plugin.Instance.Config.NeedAllPermsInList   )
            {
                response = "no perm";
                return false;
            }



            var Gluegun = Player.Get(sender).AddItem(ItemType.GunCOM15);
            Firearm gluegun = (Firearm)Gluegun.Base;
            gluegun.ApplyAttachmentsCode(42, false);
            Plugin.CustomItems.Add(Gluegun.Serial, 3);
            response = "You have got GlueGun";
            return true;
        }
    }
}