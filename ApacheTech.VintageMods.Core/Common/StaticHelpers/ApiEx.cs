﻿using System;
using System.Collections.Generic;
using System.Threading;
using ApacheTech.VintageMods.Core.Annotation.Attributes;
using ApacheTech.VintageMods.Core.Common.InternalSystems;
using ApacheTech.VintageMods.Core.Extensions.Game;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;
using Vintagestory.Server;

// AppSide Anywhere - Code by: Novocain1

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Common.StaticHelpers
{
    /// <summary>
    ///     Global access to game's sided APIs. 
    /// </summary>
    public static class ApiEx
    {
        /// <summary>
        ///     Gets the mod information.
        /// </summary>
        /// <value>The mod information.</value>
        public static IVintageModInfo ModInfo { get; internal set; }

        /// <summary>
        ///     The core API implemented by the client. The main interface for accessing the client. Contains all sub-components, and some miscellaneous methods.
        /// </summary>
        /// <value>The client-side API.</value>
        public static ICoreClientAPI Client { get; internal set; }

        /// <summary>
        ///     The core API implemented by the server. The main interface for accessing the server. Contains all sub-components, and some miscellaneous methods.
        /// </summary>
        /// <value>The server-side API.</value>
        public static ICoreServerAPI Server { get; internal set; }

        /// <summary>
        ///     Common API Components that are available on the server and the client. Cast to ICoreServerAPI, or ICoreClientAPI, to access side specific features.
        /// </summary>
        /// <value>The universal API.</value>
        public static ICoreAPI Current => Side.IsClient() ? Client : Server;

        /// <summary>
        ///     The concrete implementation of the <see cref="IClientWorldAccessor"/> interface. Contains access to lots of world manipulation and management features.
        /// </summary>
        /// <value>The <see cref="Vintagestory.Client.NoObf.ClientMain"/> instance that controls access to features within  the gameworld.</value>
        public static ClientMain ClientMain { get; internal set; }

        /// <summary>
        ///     The concrete implementation of the <see cref="IServerWorldAccessor"/> interface. Contains access to lots of world manipulation and management features.
        /// </summary>
        /// <value>The <see cref="Vintagestory.Server.ServerMain"/> instance that controls access to features within  the gameworld.</value>
        public static ServerMain ServerMain { get; internal set; }


        private static ClientSystemAsyncActions ClientAsync => Client.GetVanillaClientSystem<ClientSystemAsyncActions>();

        private static ServerSystemAsyncActions ServerAsync => Server.GetVanillaServerSystem<ServerSystemAsyncActions>();

        public static IAsyncActions Async => OneOf<IAsyncActions>(ClientAsync, ServerAsync);

        public static T OneOf<T>(T clientInstance, T serverInstance)
        {
            return ModInfo.Side switch
            {
                EnumAppSide.Server => serverInstance,
                EnumAppSide.Client => clientInstance,
                EnumAppSide.Universal => Side.IsClient() ? clientInstance : serverInstance,
                _ => throw new ArgumentOutOfRangeException(nameof(ModInfo.Side), ModInfo.Side, "Corrupted ModInfo data.")
            };
        }

        public static void Run(Action clientAction, Action serverAction)
        {
            OneOf(clientAction, serverAction).Invoke();
        }

        /// <summary>
        ///     Gets the current app-side.
        /// </summary>
        /// <value>The current app-side.</value>
        public static EnumAppSide Side => GetAppSide();

        private static readonly Dictionary<int, EnumAppSide> FastSideLookup = new();

        private static void CacheCurrentThread()
        {
            FastSideLookup[Thread.CurrentThread.ManagedThreadId] =
                Thread.CurrentThread.Name == "SingleplayerServer"
                    ? EnumAppSide.Server
                    : EnumAppSide.Client;
        }

        private static EnumAppSide GetAppSide()
        {
            if (FastSideLookup.TryGetValue(Thread.CurrentThread.ManagedThreadId, out var side)) return side;
            CacheCurrentThread();
            side = FastSideLookup[Thread.CurrentThread.ManagedThreadId];
            return side;
        }

        public static void Dispose()
        {
            FastSideLookup.Clear();
        }
    }
}
