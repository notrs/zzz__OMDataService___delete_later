﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OMSDataService.DataInterfaces;
using OMSDataService.DomainObjects.Models;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Authorization;

namespace OMSDataService.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BidsheetController : ControllerBase
    {
        private IBidsheetRepository _repo;
        private readonly ILogger _logger;
        public BidsheetController(IBidsheetRepository repo, ILogger logger)
        {
             _repo = repo;
            _logger = logger;
        }

        [ActionName("GetBidsheets")]
        [HttpGet]
        public async Task<IActionResult> GetBidsheets()
        {
            try
            {
                var list = await _repo.GetBidsheets();
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.Write(LogEventLevel.Error, ex, "GetBidsheets failed: {ex.message}");
                var returnResult = ex.InnerException?.InnerException?.Message ?? ex.Message;
                return BadRequest(returnResult);
            }
        }

        [ActionName("GetBidsheetsToRollOfferTo")]
        [HttpGet]
        public async Task<IActionResult> GetBidsheetsToRollOfferTo(int locationId, int commodityId, int marketZoneId)
        {
            try
            {
                var list = await _repo.GetBidsheetsToRollOfferTo(locationId, commodityId, marketZoneId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.Write(LogEventLevel.Error, ex, "GetBidsheetsToRollOfferTo failed: {ex.message}");
                var returnResult = ex.InnerException?.InnerException?.Message ?? ex.Message;
                return BadRequest(returnResult);
            }
        }

        [ActionName("GetBidsheet")]
        [HttpGet]
        public async Task<IActionResult> GetBidsheet(int bidsheetId)
        {
            try
            {
                var list = await _repo.GetBidsheet(bidsheetId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.Write(LogEventLevel.Error, ex, "GetBidsheet failed: {ex.message}");
                var returnResult = ex.InnerException?.InnerException?.Message ?? ex.Message;
                return BadRequest(returnResult);
            }
        }

        [ActionName("GetBidsheetWithFutureValues")]
        [HttpGet]
        public async Task<IActionResult> GetBidsheetWithFutureValues(int bidsheetId)
        {
            try
            {
                var list = await _repo.GetBidsheetWithFutureValues(bidsheetId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.Write(LogEventLevel.Error, ex, "GetBidsheetWithFutureValues failed: {ex.message}");
                var returnResult = ex.InnerException?.InnerException?.Message ?? ex.Message;
                return BadRequest(returnResult);
            }
        }

        [ActionName("UpdateBidsheet")]
        [HttpPost]
        public IActionResult UpdateBidsheet([FromBody] Bidsheet item)
        {
            try
            {
                _repo.UpdateBidsheet(item);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Write(LogEventLevel.Error, ex, "UpdateBidsheet failed: {ex.message}");
                var returnResult = ex.InnerException?.InnerException?.Message ?? ex.Message;
                return BadRequest(returnResult);
            }
        }

        [ActionName("GetNewBidsheet")]
        [HttpGet]
        public async Task<IActionResult> GetNewBidsheet()
        {
            try
            {
                var bidsheet = await _repo.GetNewBidsheet();

                return Ok(bidsheet);
            }
            catch (Exception ex)
            {
                _logger.Write(LogEventLevel.Error, ex, "GetNewBidsheet failed: {ex.message}");
                var returnResult = ex.InnerException?.InnerException?.Message ?? ex.Message;
                return BadRequest(returnResult);
            }
        }

        [ActionName("AddBidsheet")]
        [HttpPost]
        public IActionResult AddBidsheet([FromBody] Bidsheet item)
        {
            try
            {
                _repo.AddBidsheet(item);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Write(LogEventLevel.Error, ex, "AddBidsheet failed: {ex.message}");
                var returnResult = ex.InnerException?.InnerException?.Message ?? ex.Message;
                return BadRequest(returnResult);
            }
        }

        [ActionName("SearchBidsheets")]
        [HttpGet]
        public async Task<IActionResult> SearchBidsheets(int? locationId, int? commodityId, bool active, bool countHasOffers, bool countHasOffersByAccountOnly, int? accountID)
        {
            try
            {
                var list = await _repo.SearchBidsheets(locationId, commodityId, active, countHasOffers, countHasOffersByAccountOnly, accountID);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.Write(LogEventLevel.Error, ex, "SearchBidsheets failed: {ex.message}");
                var returnResult = ex.InnerException?.InnerException?.Message ?? ex.Message;
                return BadRequest(returnResult);
            }
        }

        [ActionName("SearchBidsheetsArchive")]
        [HttpGet]
        public async Task<IActionResult> SearchBidsheetsArchive(int? locationId, int? commodityId, DateTime? archiveStartDate, DateTime? archiveEndDate)
        {
            try
            {
                var list = await _repo.SearchBidsheetsArchive(locationId, commodityId, archiveStartDate, archiveEndDate);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.Write(LogEventLevel.Error, ex, "SearchBidsheetsArchive failed: {ex.message}");
                var returnResult = ex.InnerException?.InnerException?.Message ?? ex.Message;
                return BadRequest(returnResult);
            }
        }
    }
}