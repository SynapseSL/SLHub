using System.Collections.Generic;
using Synapse.Config;
using System.IO;
using System.Reflection;
using System.Linq;
using System;

namespace SLHub.Addons
{
    public class AddonHandler
    {
        internal AddonHandler(PluginClass plugin)
        {
            Plugin = plugin;

            var directory = Path.Combine(Plugin.PluginDirectory, "Addons");
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            foreach (var assemblypath in Directory.GetFiles(directory, "*.dll"))
            {
                try
                {
                    var assembly = Assembly.Load(File.ReadAllBytes(assemblypath));
                    LoadAddons(assembly);
                }
                catch(Exception e)
                {
                    Synapse.Api.Logger.Get.Error($"Error while Loading Assembly of HubAddon: {assemblypath}\n\n{e}");
                }
            }

            Synapse.Server.Get.Events.Round.WaitingForPlayersEvent += NewRound;
        }

        public PluginClass Plugin { get; }

        public List<IHubAddon> Addons { get; } = new List<IHubAddon>();

        /// <summary>
        /// Loads all the HubAddons that are inside one Assembly
        /// </summary>
        /// <param name="assembly"></param>
        public void LoadAddons(Assembly assembly)
        {
            foreach (var type in assembly.DefinedTypes.Where(x => typeof(IHubAddon).IsAssignableFrom(x)))
            {
                try
                {
                    LoadAddon(Activator.CreateInstance(type, new object[] { }) as IHubAddon);
                }
                catch (Exception e)
                {
                    Synapse.Api.Logger.Get.Error($"HubAddon failed to Load Type:{type.Name}\n\n{e}");
                }
            }
        }

        /// <summary>
        /// Loads a HubAddon
        /// </summary>
        /// <param name="addon"></param>
        public void LoadAddon(IHubAddon addon)
        {
            if (Addons.Contains(addon)) return;
            Addons.Add(addon);

            InjectConfig(addon);
            addon.Load();
        }

        /// <summary>
        /// Reloads all Addons
        /// </summary>
        public void ReloadAddons()
        {
            foreach(var addon in Addons)
            {
                InjectConfig(addon);
                addon.Reload();
            }
        }

        /// <summary>
        /// This activates the NewRound method of all Addons
        /// </summary>
        private void NewRound()
        {
            foreach (var addon in Addons)
                addon.NewRound();
        }

        /// <summary>
        /// This Injects the Config of the Addon into a field or property with the Attribute AddonConfig
        /// </summary>
        /// <param name="addon"></param>
        private void InjectConfig(IHubAddon addon)
        {
            foreach(var field in addon.GetType().GetFields())
            {
                var attribute = field.GetCustomAttribute<AddonConfig>();
                if (attribute == null) continue;

                var configtype = FieldInfo.GetFieldFromHandle(field.FieldHandle).FieldType;

                if (!typeof(IConfigSection).IsAssignableFrom(configtype)) continue;

                var obj = Activator.CreateInstance(configtype);
                var config = Plugin.SYML.GetOrSetDefaultUnsafe(attribute.Section, obj);
                field.SetValue(addon, config);
            }

            foreach(var property in addon.GetType().GetProperties())
            {
                var attribute = property.GetCustomAttribute<AddonConfig>();
                if (attribute == null) continue;

                var configtype = property.PropertyType;

                if (!typeof(IConfigSection).IsAssignableFrom(configtype)) continue;

                var obj = Activator.CreateInstance(configtype);
                var config = Plugin.SYML.GetOrSetDefaultUnsafe(attribute.Section, obj);
                property.SetValue(addon, config);
            }
        }
    }
}
