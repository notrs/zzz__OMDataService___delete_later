﻿using OMSDataService.DomainObjects.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OMSDataService.DataInterfaces
{
    public interface IBidsheetRepository
    {
        Task<List<Bidsheet>> GetBidsheets();
        Task<Bidsheet> GetBidsheet(int bidsheetId);
        void AddBidsheet(Bidsheet item);
        void UpdateBidsheet(Bidsheet item);
        Task<List<BidsheetSearchResult>> SearchBidsheets(int? locationId, int? commodityId, bool active, bool countHasOffers);
    }
}
