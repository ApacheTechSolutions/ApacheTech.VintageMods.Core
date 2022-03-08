﻿using System;

// ReSharper disable VirtualMemberNeverOverridden.Global

namespace ApacheTech.VintageMods.Core.Abstractions.Features
{
    /// <summary>
    ///     Acts as a base class for all settings POCO Classes for a given feature.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public abstract class FeatureSettings : IDisposable
    {
        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}