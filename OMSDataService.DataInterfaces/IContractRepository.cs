﻿using OMSDataService.DomainObjects.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OMSDataService.DataInterfaces
{
    public interface IContractRepository
    {
        Task<List<Contract>> GetContracts();
        Task<Contract> GetContract(int contractId);
        void AddContract(Contract item);
        void UpdateContract(Contract item);

    }
}
