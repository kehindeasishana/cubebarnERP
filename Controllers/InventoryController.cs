using Services.Financial;
using Services.Inventory;
using Services.Purchasing;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
 //   [Authorize(Roles ="SuperAdmin, Cashier, Accountant, account Manager, Store Keeper, Store Manager, Inventory Manager")]
   [Authorize]
    public class InventoryController : BaseController
    {
        private readonly IInventoryService _inventoryService;
        private readonly IFinancialService _financialService;
        private readonly IPurchasingService _purchasingService;

        public InventoryController(IInventoryService inventoryService,
            IFinancialService financialService,
            IPurchasingService purchasingService)
        {
            _inventoryService = inventoryService;
            _financialService = financialService;
            _purchasingService = purchasingService;
        }
        [Audit]
        public ActionResult Items()
        {
            var items = _inventoryService.GetAllItems();
            var model = new Models.ViewModels.Items.Items();
            foreach(var item in items)
            {
                model.ItemsList.Add(new Models.ViewModels.Items.Items.ItemListLine
                {
                    ItemId = item.Id,
                    No = item.No,
                    Description = item.Description,
                    QtyOnHand = item.ComputeQuantityOnHand()
                });
            }
            return View(model);
        }

        public ActionResult AddItem()
        {
            var model = new Models.ViewModels.Items.Items.AddItem();
            return View(model);
        }
        [Audit]
        [HttpPost, ActionName("AddItem")]
        [FormValueRequiredAttribute("SaveItem")]
        public ActionResult AddItem(Models.ViewModels.Items.Items.AddItem model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Description))
                    throw new Exception("Description cannot be empty.");

                _inventoryService.AddItem(new Core.Domain.Items.Item() {
                    Code = model.Code,
                    Description = model.Description,
                });

                return RedirectToAction("Items");
            }
            catch
            {
                return View(model);
            }
        }

        // GET: Inventory/Edit/5
        public ActionResult EditItem(int id)
        {
            var item = _inventoryService.GetItemById(id);
            var accounts = _financialService.GetAccounts();
            var measurements = _inventoryService.GetMeasurements();
            //var taxes = _financialService.GetTaxes();
            var itemCategories = _inventoryService.GetItemCategories();
            var vendors = _purchasingService.GetVendors();
            //var itemTaxGroups = _financialService.GetItemTaxGroups();

            var model = new Models.ViewModels.Items.EditItem();
            model.PrepareEditItemViewModel(item);
            model.Accounts = ModelViewHelper.Accounts();
            model.UnitOfMeasurements = ModelViewHelper.Measurements();
            model.Taxes = ModelViewHelper.Taxes();
            model.ItemTaxGroups = ModelViewHelper.ItemTaxGroups();
            model.Vendors = ModelViewHelper.Vendors();
            model.ItemCategories = ModelViewHelper.ItemCategories();
            model.ItemTaxGroupId = model.ItemTaxGroupId == null ? -1 : model.ItemTaxGroupId;
            model.InventoryAccountId = model.InventoryAccountId == null ? -1 : model.InventoryAccountId;
            model.SellAccountId = model.SellAccountId == null ? -1 : model.SellAccountId;
            model.InventoryAdjustmentAccountId = model.InventoryAdjustmentAccountId == null ? -1 : model.InventoryAdjustmentAccountId;
            model.PurchaseMeasurementId = model.PurchaseMeasurementId == null ? -1 : model.PurchaseMeasurementId;
            model.CostOfGoodsSoldAccountId = model.CostOfGoodsSoldAccountId == null ? -1 : model.CostOfGoodsSoldAccountId;

            return View(model);
        }

        // POST: Inventory/Edit/5
        [Audit]
        [HttpPost, ActionName("EditItem")]
        [FormValueRequiredAttribute("Save")]
        public ActionResult EditItem(Models.ViewModels.Items.EditItem model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Description))
                    throw new Exception("Item description cannot be empty.");

                var item = _inventoryService.GetItemById(model.Id);
                item.Id = model.Id;
                item.SmallestMeasurementId = model.SmallestMeasurementId;
                //item.InventoryId = model.InventoryId;
                item.ItemTaxGroupId = model.ItemTaxGroupId;
                item.PreferredVendorId = model.PreferredVendorId;
                item.No = model.No;
                item.Code = model.Code;
                item.Description = model.Description;
                item.PurchaseDescription = model.PurchaseDescription;
                item.SellDescription = model.SellDescription;
                item.Cost = model.Cost;
                item.Price = model.Price;
                item.ItemTaxGroupId = model.ItemTaxGroupId == -1 ? null : model.ItemTaxGroupId;
                item.SalesAccountId = model.SellAccountId == -1 ? null : model.SellAccountId;
                item.InventoryAdjustmentAccountId = model.InventoryAdjustmentAccountId == -1 ? null : model.InventoryAdjustmentAccountId;
                item.InventoryAccountId = model.InventoryAccountId == -1 ? null : model.InventoryAccountId;
                item.SellMeasurementId = model.SellMeasurementId == -1 ? null : model.SellMeasurementId;
                item.PurchaseMeasurementId = model.PurchaseMeasurementId == -1 ? null : model.PurchaseMeasurementId;
                item.CostOfGoodsSoldAccountId = model.CostOfGoodsSoldAccountId == -1 ? null : model.CostOfGoodsSoldAccountId;
                _inventoryService.UpdateItem(item);
                return RedirectToAction("Items");
            }
            catch
            {
                return View();
            }
        }
        [Audit]
        [HttpGet]
        public JsonResult GetItemByNo(string itemNo)
        {
            var item = _inventoryService.GetItemByNo(itemNo);
            var data = new { Price = item.Price, Cost = item.Cost, Measurement = item.SellMeasurement.Description };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [Audit]
        public ActionResult InventoryControlJournal()
        {
            var invControlJournals = _inventoryService.GetInventoryControlJournals();
            var model = new List<Models.ViewModels.Items.InventoryControlJournal>();
            foreach (var icj in invControlJournals)
            {
                model.Add(new Models.ViewModels.Items.InventoryControlJournal()
                {
                    In = icj.INQty,
                    Out = icj.OUTQty,
                    Item = icj.Item.Description,
                    Measurement = icj.Measurement.Code,
                    Date = icj.Date
                });
            }
            return View(model);
        }
    }
}
