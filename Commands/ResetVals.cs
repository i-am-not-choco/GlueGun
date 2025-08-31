using System;
using System.Collections.Generic;
using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using Mirror;
using UnityEngine;

namespace GlueGun.Commands;



[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class ResetVals : ICommand
{
    public string Command { get; } = "GlueResetValues";
    public string[] Aliases { get; } = ["GRV"];
    public string Description { get; } = "Resets values for you used by GlueGun, would be throw if wasthrowed was a thing ";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (sender == null)
        {
            response = "null player";
            return false;
        }

        if (!sender.HasPermissions(Plugin.Instance.Config.PermsNeedToGive.ToArray()) &&
            Plugin.Instance.Config.NeedAllPermsInList ||
            sender.HasPermissions(Plugin.Instance.Config.PermsNeedToGive.ToArray()) &&
            !Plugin.Instance.Config.NeedAllPermsInList)
        {
            response = "no perm";
            return false;
        }

        GlueGun.PerPlayerList[Player.Get(sender).PlayerId] = (new List<GameObject>(), null);
        response = "Resetted Values";
        return true;
    }
}
