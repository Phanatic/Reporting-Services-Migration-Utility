//-----------------------------------------------------------------------
// <copyright file="ICatalogItem.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------

namespace ReportingServicesSchema.Contracts
{
    using System;

    /// <summary>
    /// Contract to provide type safety against various catalog item types across reporting services versions.
    /// </summary>
    public interface ICatalogItem
    {
        /// <summary>
        /// The name of the user who originally added the item to the report server database.
        /// </summary>
        string CreatedBy { get; }

        /// <summary>
        /// The user name of the account which last modified the report artifact
        /// </summary>
        string ModifiedBy { get; }

        /// <summary>
        /// The description of the report artifact
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The date and time that the item was added to the report server database.
        /// </summary>
        DateTime CreationDate { get; }

        /// <summary>
        ///  A boolean value that indicates whether the ExecutionDate property is specified
        /// </summary>
        bool ExecutionDateSpecified { get; }

        /// <summary>
        /// The date and time at which a report execution snapshot was last created for a report.
        /// </summary>
        DateTime ExecutionDate { get; }

        /// <summary>
        /// The date on which the artifact was modified.
        /// </summary>
        DateTime ModifiedDate { get; }
    }
}
