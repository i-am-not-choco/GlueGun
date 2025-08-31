using System;
using System.Collections.Generic;
using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using UnityEngine;

namespace GlueGun.Commands
{
    public class GlueToThing
    {
        [CommandHandler(typeof(RemoteAdminCommandHandler))]
        public class GlueCommand : ICommand
        {
            public string Command { get; } = "Glue";
            public string[] Aliases { get; } = ["Glue"];
            public string Description { get; } = " LEts you make a AdminToy a child to something  Needs glue gun to use";

            public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
            {
                if (sender == null)
                {
                    response = "null player";
                    return false;
                }
                if (!sender.HasPermissions(Plugin.Instance.Config.PermsNeedToGive.ToArray()) && Plugin.Instance.Config.NeedAllPermsInList || sender.HasPermissions(Plugin.Instance.Config.PermsNeedToGive.ToArray()) && !Plugin.Instance.Config.NeedAllPermsInList)
                {
                    response = "no perm";
                    return false;
                }
                if (GlueGun.PerPlayerList[Player.Get(sender).PlayerId].Item2 == null) 
                {
                    
                   response = "unparent";
                    return false;
                }

                List<GameObject> noreusesame = new List<GameObject>();
                string respond = "";
                foreach (var gameObject in GlueGun.PerPlayerList[Player.Get(sender).PlayerId].Item1)
                {
                    if (noreusesame.Contains(gameObject))
                        goto next;
                    noreusesame.Add(gameObject);
                    if(gameObject.TryGetComponent( out AdminToys.Scp079CameraToy a)) 
                    {
                       a.transform.parent = GlueGun.PerPlayerList[Player.Get(sender).PlayerId].Item2.transform;
                       respond += ", " + gameObject.name; 
                    }
                    else if(gameObject.TryGetComponent( out AdminToys.ShootingTarget b)) 
                    {
                       b.transform.parent = GlueGun.PerPlayerList[Player.Get(sender).PlayerId].Item2.transform;
                       respond += ", " + gameObject.name;
                    }
                    else if(gameObject.TryGetComponent( out AdminToys.InvisibleInteractableToy c)) 
                    {
                       c.transform.parent = GlueGun.PerPlayerList[Player.Get(sender).PlayerId].Item2.transform;
                       respond += ", " + gameObject.name;
                    }
                    else if(gameObject.TryGetComponent( out AdminToys.LightSourceToy   d))
                    {
                       d.transform.parent = GlueGun.PerPlayerList[Player.Get(sender).PlayerId].Item2.transform;
                       respond += ", " + gameObject.name;
                    } 
                    else if(gameObject.TryGetComponent( out AdminToys.SpeakerToy  e))
                    {
                       e.transform.parent = GlueGun.PerPlayerList[Player.Get(sender).PlayerId].Item2.transform;
                       respond += ", " + gameObject.name;
                    }
                    else if(gameObject.TryGetComponent( out AdminToys.CapybaraToy f))
                    {
                       f.transform.parent = GlueGun.PerPlayerList[Player.Get(sender).PlayerId].Item2.transform;
                       respond += ", " + gameObject.name; 
                    }
                    else if(gameObject.TryGetComponent( out  AdminToys.PrimitiveObjectToy  g))
                    {
                       g.transform.parent = GlueGun.PerPlayerList[Player.Get(sender).PlayerId].Item2.transform;
                       respond += ", " + gameObject.name;
                    }
                    next: ;
                }
                response = respond + " all glued to " + GlueGun.PerPlayerList[Player.Get(sender).PlayerId].Item2.name;
                return true;
            }

        }
    }
}