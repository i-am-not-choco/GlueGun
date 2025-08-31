using System.Collections.Generic;
using InventorySystem.Items.Firearms;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Permissions;
using Mirror;
using UnityEngine;


namespace GlueGun;

    public class GlueGun
    {
        public static Dictionary<int, (List<GameObject>, GameObject)> PerPlayerList = new();

        public void Shotting(PlayerShootingWeaponEventArgs ev)
        {
            if (!Plugin.CustomItems.ContainsKey(ev.FirearmItem.Serial) ||
                Plugin.CustomItems[ev.FirearmItem.Serial] != 3)
                return;
            if (!ev.Player.HasPermissions(Plugin.Instance.Config.PermsNeedToGive.ToArray()) &&
                Plugin.Instance.Config.NeedAllPermsInList ||
                ev.Player.HasPermissions(Plugin.Instance.Config.PermsNeedToGive.ToArray()) &&
                !Plugin.Instance.Config.NeedAllPermsInList)
                return;
            ev.IsAllowed = false;
            if (!Physics.Raycast(origin: ev.Player.Camera.position,
                    layerMask: ~((1 << LayerMask.NameToLayer("Hitbox")) | (1 << LayerMask.NameToLayer("Player"))), maxDistance: 60f,
                    direction: ev.Player.Camera.forward, hitInfo: out var hit))
                return;
            if (hit.collider.gameObject.TryGetComponent(out AdminToys.SpeakerToy speak) || hit.collider.gameObject.TryGetComponent(out AdminToys.LightSourceToy light) || hit.collider.gameObject.TryGetComponent(out AdminToys.InvisibleInteractableToy intrac) || hit.collider.gameObject.TryGetComponent(out AdminToys.ShootingTarget Target) || hit.collider.gameObject.TryGetComponent(out AdminToys.PrimitiveObjectToy primitive) || hit.collider.gameObject.TryGetComponent(out AdminToys.AdminToyBase adminBase))
            {
                PerPlayerList[ev.Player.PlayerId].Item1.Add(hit.collider.gameObject);
                ev.Player.SendConsoleMessage($"You have added a {hit.collider.gameObject.name} to glue to something"); 
                return;
            }

            var a = hit.collider.gameObject.GetComponentInParent<AdminToys.Scp079CameraToy>();
            if (a != null)
            {
                PerPlayerList[ev.Player.PlayerId].Item1.Add(a.gameObject);
                ev.Player.SendConsoleMessage("You have added a Scp079Camera to glue to something"); 
            }
            var b = hit.collider.gameObject.GetComponentInParent<AdminToys.CapybaraToy>();
            if (b != null)
            {
                PerPlayerList[ev.Player.PlayerId].Item1.Add(b.gameObject);
                ev.Player.SendConsoleMessage("You have added a Capybara to glue to something"); 
            }





        }

        public void onver(PlayerJoinedEventArgs ev)
        {
            if (!ev.Player.HasPermissions(Plugin.Instance.Config.PermsNeedToGive.ToArray())  && Plugin.Instance.Config.NeedAllPermsInList || ev.Player.HasPermissions(Plugin.Instance.Config.PermsNeedToGive.ToArray())  && !Plugin.Instance.Config.NeedAllPermsInList)
                return;
            if (PerPlayerList.ContainsKey(ev.Player.PlayerId))
                return;
            PerPlayerList.Add(ev.Player.PlayerId, (new List<GameObject>(), null));
        }

        public void ChooseId(PlayerDroppingItemEventArgs ev)
        {
            if (!Plugin.CustomItems.ContainsKey(ev.Item.Serial) || Plugin.CustomItems[ev.Item.Serial] != 3)
                return;
            if (!ev.Player.HasPermissions(Plugin.Instance.Config.PermsNeedToGive.ToArray()) && Plugin.Instance.Config.NeedAllPermsInList ||
                ev.Player.HasPermissions(Plugin.Instance.Config.PermsNeedToGive.ToArray()) && !Plugin.Instance.Config.NeedAllPermsInList)
                return;
            ev.IsAllowed = false;
            Firearm gluegun = (Firearm)ev.Item.Base;
            RaycastHit hit;
            if (!gluegun.IsEmittingLight)
            {
                if (!Physics.Raycast(origin: ev.Player.Camera.position, maxDistance: 60f, direction: ev.Player.Camera.forward, hitInfo: out hit, layerMask: ~((1 << LayerMask.NameToLayer("Hitbox")) | (1 << LayerMask.NameToLayer("Player")))))
                    return;
                //   Logger.Info($" name:{hit.collider.gameObject.name},layersexclued:{hit.collider.excludeLayers}, LayersInculded:{hit.collider.includeLayers},GameobLayer: {hit.collider.gameObject.layer},name of layer:{LayerMask.LayerToName(hit.collider.gameObject.layer)}");
                /* foreach (var u in hit.collider.gameObject.GetComponentsInParent<Component>())
                {
                    Logger.Info("parent" + u);
                }
                foreach (var u in hit.collider.gameObject.GetComponentsInChildren<Component>())
                {
                    Logger.Info("Child" + u);
                }*/
                
                
            }
            else
            {
                if (!Physics.Raycast(origin: ev.Player.Camera.position, maxDistance: 60f, direction: ev.Player.Camera.forward, hitInfo: out hit))
                    return;
            }

            var a = hit.collider.GetComponentInChildren<NetworkIdentity>();
            if (a != null)
            {
                PerPlayerList[ev.Player.PlayerId] = (PerPlayerList[ev.Player.PlayerId].Item1, a.gameObject);
                ev.Player.SendConsoleMessage($"you will being gluing to {a.gameObject.name}, netid:{a.netId}");
                return;
            }
            a = hit.collider.GetComponentInParent<NetworkIdentity>();
            if (a == null)
             return;
            PerPlayerList[ev.Player.PlayerId] = (PerPlayerList[ev.Player.PlayerId].Item1, a.gameObject);
            ev.Player.SendConsoleMessage($"you will being gluing to {a.gameObject.name}, netid:{a.netId}");
        }

        
        // this does not get called
        public void RemoveVals(PlayerThrowingItemEventArgs ev)
        {
            if (!Plugin.CustomItems.ContainsKey(ev.Pickup.Serial) || Plugin.CustomItems[ev.Pickup.Serial] != 3)
                return;
            if (!ev.Player.HasPermissions(Plugin.Instance.Config.PermsNeedToGive.ToArray()) && Plugin.Instance.Config.NeedAllPermsInList || ev.Player.HasPermissions(Plugin.Instance.Config.PermsNeedToGive.ToArray()) && !Plugin.Instance.Config.NeedAllPermsInList)
                return;
            ev.IsAllowed = false;
            PerPlayerList[ev.Player.PlayerId] = (new List<GameObject>(), null);
            ev.Player.SendConsoleMessage("Reseting values for GlueGun");
        }

        public void OnChoose(PlayerChangedItemEventArgs ev)
        {
            if (ev.NewItem == null)
                return;
            if (!Plugin.CustomItems.ContainsKey(ev.NewItem.Serial) || Plugin.CustomItems[ev.NewItem.Serial] != 3)
                return;
            if (!ev.Player.HasPermissions(Plugin.Instance.Config.PermsNeedToGive.ToArray()) && Plugin.Instance.Config.NeedAllPermsInList || ev.Player.HasPermissions(Plugin.Instance.Config.PermsNeedToGive.ToArray()) && !Plugin.Instance.Config.NeedAllPermsInList)
                return;
            
            
            
            ev.Player.SendHint("You have Choose GlueGun");
        }

   
        








    }