using System.Collections.Generic;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using MEC;
using Version = System.Version;

namespace GlueGun
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance { get; private set; }
        
        public override string Name { get; } = "Glue-Gun";
        public override string Description { get; } = "Lets you make a AdminToy a child to anything with network id";
        public override string Author { get; } = "Choco";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredApiVersion { get; } = new(LabApiProperties.CompiledVersion);

        public static Dictionary<ushort, int> CustomItems;


        private GlueGun _glueGun;
        public override void Enable()
        {
            Instance = this;
            CustomItems = new();
            _glueGun = new GlueGun();
           LabApi.Events.Handlers.PlayerEvents.Joined += _glueGun.onver;
           LabApi.Events.Handlers.PlayerEvents.DroppingItem += _glueGun.ChooseId;
           LabApi.Events.Handlers.PlayerEvents.ThrowingItem += _glueGun.RemoveVals;
           LabApi.Events.Handlers.PlayerEvents.ShootingWeapon += _glueGun.Shotting;
           LabApi.Events.Handlers.PlayerEvents.ChangedItem += _glueGun.OnChoose;
        }

        public override void Disable()
        {
            CustomItems = null;
            Instance = null;
            LabApi.Events.Handlers.PlayerEvents.Joined -= _glueGun.onver;
            LabApi.Events.Handlers.PlayerEvents.DroppingItem -= _glueGun.ChooseId;
            LabApi.Events.Handlers.PlayerEvents.ThrowingItem -= _glueGun.RemoveVals;
            LabApi.Events.Handlers.PlayerEvents.ShootingWeapon -= _glueGun.Shotting;
            LabApi.Events.Handlers.PlayerEvents.ChangedItem -= _glueGun.OnChoose;
            _glueGun = null;
        }
    }
}