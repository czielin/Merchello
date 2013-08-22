﻿using System;
using Merchello.Core.Models;
using Umbraco.Core.Persistence.Repositories;

namespace Merchello.Core.Persistence.Repositories
{
    /// <summary>
    /// Marker Interface for the customer repository
    /// </summary>
    public interface ICustomerRepository : IRepository<Guid, ICustomer>
    {
    }
}