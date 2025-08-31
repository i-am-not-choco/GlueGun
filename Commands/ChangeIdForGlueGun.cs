using System;
using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using Mirror;
using UnityEngine;

namespace GlueGun.Commands;

public class ChangeIdForGlueGun
{
    
    
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class GiveCommand : ICommand
    {
        public string Command { get; } = "GlueIdChanger";
        public string[] Aliases { get; } = ["GIC"]; 
        public string Description { get; } = "Changes NetTargetId for Glue";

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
            
            foreach (var NetId in GameObject.FindObjectsByType<NetworkIdentity>(findObjectsInactive: FindObjectsInactive.Exclude, FindObjectsSortMode.None))
            {
                try
                {
                    if (arguments.Array[1] == null || NetId.netId == Convert.ToUInt16(arguments.Array[2]))
                    {
                       
                        switch (arguments.Array[1].ToLower())
                        {
                            case "p" :
                            case "parent":
                                GlueGun.PerPlayerList[Player.Get(sender).PlayerId] = (GlueGun.PerPlayerList[Player.Get(sender).PlayerId].Item1, NetId.gameObject);
                                response = $"Changed to {NetId.netId}, {NetId.gameObject}";
                                return true;
                            case"c":
                            case"child":
                                if(NetId.gameObject.TryGetComponent(out AdminToys.Scp079CameraToy cam)||NetId.gameObject.TryGetComponent(out AdminToys.CapybaraToy capy)|| NetId.gameObject.TryGetComponent(out AdminToys.SpeakerToy speak) || NetId.gameObject.TryGetComponent(out AdminToys.LightSourceToy light) || NetId.gameObject.TryGetComponent(out AdminToys.InvisibleInteractableToy intrac) || NetId.gameObject.TryGetComponent(out AdminToys.ShootingTarget target) ||NetId.gameObject.TryGetComponent(out AdminToys.PrimitiveObjectToy primitive) || NetId.gameObject.TryGetComponent(out AdminToys.AdminToyBase adminBase))
                                {
                                    GlueGun.PerPlayerList[Player.Get(sender).PlayerId].Item1.Add(NetId.gameObject);
                                    response = $"You have added a {NetId.gameObject.name} to glue to something";
                                    return true;
                                }
                                break;
                            default:
                                response = "must have a argument as c, child or p, parent\n" +
                                           "p/parent to choose what to glue the toys to must be followed with a netid\n" +
                                           "c/child to add a admintoy to glue to something use netid is showed when you spawn a toy must be follwed with a netid";
                                return false;
                                
                        }
                    }
                }
                catch
                {
                    response = "not a netid";
                    return false;
                }
            }
            
            response = $"no Netid found with {arguments.Array[2]}";
            return false;
        }
    }
    
    
    
    
    
    
    
}