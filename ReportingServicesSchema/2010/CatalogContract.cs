//-----------------------------------------------------------------------
// <copyright file="CatalogContract.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------

namespace ReportingServicesSchema.ReportingServices2010
{
    using System;
    using ReportingServicesSchema.Contracts;

    /// <summary>
    /// Contract implementation to provide type safety against various catalog item types across reporting services versions.
    /// </summary>
    public partial class CatalogItem : ICatalogItem
    {
        /// <summary>
        ///  A boolean value that indicates whether the ExecutionDate property is specified
        /// </summary>
        public bool ExecutionDateSpecified
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// The date and time at which a report execution snapshot was last created for a report.
        /// </summary>
        public DateTime ExecutionDate
        {
            get
            {
                return DateTime.MinValue;
            }
        }
    }
}