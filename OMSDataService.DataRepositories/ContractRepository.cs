﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OMSDataService.DataInterfaces;
using OMSDataService.DomainObjects.Models;
using OMSDataService.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OMSDataService.DataRepositories
{
    public class ContractRepository : IContractRepository
    { 

        private readonly IMapper _mapper;
        private ApiContext _context;

        public ContractRepository(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<OfferSearchResult>> GetOffers(int accountId)
        {
            return await (from c in _context.Contracts
                          join cd in _context.ContractDetails on c.ContractID equals cd.ContractID
                          join a in _context.Accounts on c.AccountID equals a.AccountID
                          join cmd in _context.Commodities on cd.CommodityID equals cmd.CommodityID
                          join ct in _context.ContractTypes on cd.ContractTypeID equals ct.ContractTypeID
                          join l in _context.Locations on c.LocationID equals l.LocationID
                          where c.AccountID == accountId
                          orderby c.ContractID
                          select new OfferSearchResult
                          {
                              AccountID = c.AccountID,
                              AccountName = a.AccountName,
                              Basis = cd.Basis.HasValue ? cd.Basis.Value.ToString("N4") : "",
                              CashPrice = cd.CashPrice.HasValue ? cd.CashPrice.Value.ToString("N4") : "",
                              CommodityID = cd.CommodityID,
                              CommodityName = cmd.CommodityName,
                              ContractDate = c.ContractDate.ToString("MM/dd/yyyy"),
                              ContractID = c.ContractID,
                              ContractNumber = c.ContractNumber,
                              ContractTypeID = cd.ContractTypeID,
                              ContractTypeName = ct.ContractTypeCode,
                              DeliveryEndDate = cd.DeliveryEndDate.HasValue ? cd.DeliveryEndDate.Value.ToString("MM/dd/yyyy") : "",
                              DeliveryStartDate = cd.DeliveryStartDate.HasValue ? cd.DeliveryStartDate.Value.ToString("MM/dd/yyyy") : "",
                              FuturesPrice = cd.Futures.HasValue ? cd.Futures.Value.ToString("N4") : "",
                              LocationID = c.LocationID,
                              LocationName = l.LocationName,
                              Quantity = cd.Quantity.HasValue ? cd.Quantity.Value.ToString("N4") : ""
                          }).ToListAsync();
        }

        [Obsolete]
        public async Task<List<ContractSearchResult>> GetContracts(string accountExternalRef)
        {
            var contracts = await _context.Query<ContractSearchResult>().FromSqlRaw("Execute dbo.SearchContracts @AccountExternalRef = {0}", accountExternalRef).ToListAsync();

            foreach (var contract in contracts)
            {
                contract.AppliedRemainingGraphData = new List<ContractGraphData>();
                contract.SettledUnsettledGraphData = new List<ContractGraphData>();

                if (string.IsNullOrEmpty(contract.Quantity) || decimal.Parse(contract.Quantity) <= 0)
                {
                    contract.AppliedRemainingGraphData.Add(new ContractGraphData()
                    {
                        Label = "Applied Quantity",
                        Value = 0M,
                        Color = "#075D3B"
                    });

                    contract.AppliedRemainingGraphData.Add(new ContractGraphData()
                    {
                        Label = "Remaining Quantity",
                        Value = 0M,
                        Color = "#FFE119"
                    });

                    contract.SettledUnsettledGraphData.Add(new ContractGraphData()
                    {
                        Label = "Settled Quantity",
                        Value = 0M,
                        Color = "#075D3B"
                    });

                    contract.SettledUnsettledGraphData.Add(new ContractGraphData()
                    {
                        Label = "Unsettled Quantity",
                        Value = 0M,
                        Color = "#FFE119"
                    });
                }

                else
                {
                    contract.AppliedRemainingGraphData.Add(new ContractGraphData()
                    {
                        Label = "Applied Quantity",
                        Value = Math.Round(decimal.Parse(contract.AppliedQuantity) / decimal.Parse(contract.Quantity), 2),
                        Color = "#075D3B"
                    });

                    contract.AppliedRemainingGraphData.Add(new ContractGraphData()
                    {
                        Label = "Remaining Quantity",
                        Value = Math.Round(decimal.Parse(contract.RemainingQuantity) / decimal.Parse(contract.Quantity), 2),
                        Color = "#FFE119"
                    });

                    contract.SettledUnsettledGraphData.Add(new ContractGraphData()
                    {
                        Label = "Settled Quantity",
                        Value = Math.Round(decimal.Parse(contract.SettledQuantity) / decimal.Parse(contract.Quantity), 2),
                        Color = "#075D3B"
                    });

                    contract.SettledUnsettledGraphData.Add(new ContractGraphData()
                    {
                        Label = "Unsettled Quantity",
                        Value = Math.Round(decimal.Parse(contract.UnsettledQuantity) / decimal.Parse(contract.Quantity), 2),
                        Color = "#FFE119"
                    });
                }
            }

            return contracts;
        }

        public async Task<Contract> GetContract(int contractId)
        {
            var item = await _context.Contracts
                  .FirstOrDefaultAsync(c => c.ContractID == contractId);  

            return item;
        }

        public void AddContract(Contract item)
        {
            _context.Contracts.Add(item);
            _context.SaveChanges();
        }

        public void UpdateContract(Contract item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
        }


    }


}
