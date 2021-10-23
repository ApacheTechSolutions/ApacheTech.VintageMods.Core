﻿using System.IO;
using System.Reflection;
using ApacheTech.VintageMods.Core.Annotation.Attributes;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using JetBrains.Annotations;
using Vintagestory.API.Config;

namespace ApacheTech.VintageMods.Core.Services.FileSystem
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class ModPaths
    {
        private static Assembly ModAssembly { get; }

        private static IVintageModInfo ModInfo { get; }

        /// <summary>
        /// 	Initialises static members of the <see cref="ModPaths" /> class.
        /// </summary>
        static ModPaths()
        {
            ModAssembly = AssemblyEx.GetModAssembly();
            ModInfo = ModAssembly.GetCustomAttribute<VintageModInfoAttribute>();

            ModDataRootPath = CreateDirectory(Path.Combine(VintageModsRootPath, ModInfo.RootDirectoryName));
            ModDataGlobalPath = CreateDirectory(Path.Combine(ModDataRootPath, "Global"));
            ModRootPath = Path.GetDirectoryName(ModAssembly.Location)!;
            ModAssetsPath = Path.Combine(Path.GetDirectoryName(ModAssembly.Location)!, "assets"); ;
        }

        /// <summary>
        ///     Gets the root path for all VintageMods mod files.
        /// </summary>
        /// <value>A path on the filesystem, used to store mod files.</value>
        public static string VintageModsRootPath { get; } = CreateDirectory(Path.Combine(GamePaths.DataPath, "ModData", "VintageMods"));

        /// <summary>
        ///     Gets the path used for storing data files for a particular mod.
        /// </summary>
        /// <value>A path on the filesystem, used to store mod files.</value>
        public static string ModDataRootPath { get; }

        /// <summary>
        ///     Gets the path used for storing global data files.
        /// </summary>
        /// <value>A path on the filesystem, used to store mod files.</value>
        public static string ModDataGlobalPath { get; }

        /// <summary>
        ///     Gets the path used for storing per-world data files.
        /// </summary>
        /// <value>A path on the filesystem, used to store mod files.</value>
        public static string ModDataWorldPath { get; internal set; }

        /// <summary>
        ///     Gets the path that the mod library is stored in.
        /// </summary>
        /// <value>A path on the filesystem, used to store mod files.</value>
        public static string ModRootPath { get; }

        /// <summary>
        ///     Gets the main asset origin directory for the mod.
        /// </summary>
        /// <value>A path on the filesystem, used to store mod files.</value>
        public static string ModAssetsPath { get; }

        internal static string CreateDirectory(string path)
        {
            var dir = new DirectoryInfo(path);
            if (dir.Exists) return dir.FullName;
            ApiEx.Universal?.Logger.VerboseDebug($"[VintageMods] Creating folder: {dir}");
            dir.Create();
            return dir.FullName;
        }

    }
}