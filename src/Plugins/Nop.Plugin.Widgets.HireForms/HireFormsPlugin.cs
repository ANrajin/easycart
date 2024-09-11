using Nop.Core;
using Nop.Core.Domain.Messages;
using Nop.Plugin.Widgets.HireForms.Components;
using Nop.Services.Cms;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.HireForms
{
    public class HireFormsPlugin : BasePlugin, IWidgetPlugin, IAdminMenuPlugin
    {
        #region ctor
        private readonly ILocalizationService _localizationService;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IWebHelper _webHelper;

        public HireFormsPlugin(ILocalizationService localizationService,
            IMessageTemplateService messageTemplateService,
            IWebHelper webHelper)
        {
            _localizationService = localizationService;
            _messageTemplateService = messageTemplateService;
            _webHelper = webHelper;
        }
        #endregion

        public bool HideInWidgetList => false;

        public Type GetWidgetViewComponent(string widgetZone)
        {
            return typeof(HireFormViewComponent);
        }

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.ProductDetailsOverviewTop });
        }

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/HireRequestPlugin/Configure";
        }

        public override async Task InstallAsync()
        {
            var hireFormAdminMessageTemplate = (await _messageTemplateService.GetMessageTemplatesByNameAsync(HireFormDefaults.HireFormSubmittedAdminMessageTemplateSystemName)).FirstOrDefault();
            if (hireFormAdminMessageTemplate == null)
            {
                hireFormAdminMessageTemplate = new MessageTemplate
                {
                    Name = HireFormDefaults.HireFormSubmittedAdminMessageTemplateSystemName,
                    Subject = "%Store.Name%. Hire form submitted",
                    Body = $"<p>{Environment.NewLine}<a href=\"%Store.URL%\">%Store.Name%</a>{Environment.NewLine}<br />{Environment.NewLine}<br />{Environment.NewLine}A new hire form submitted. \"%HireForm.CustomerName%\".{Environment.NewLine}</p>{Environment.NewLine}",
                    IsActive = true,
                };
                await _messageTemplateService.InsertMessageTemplateAsync(hireFormAdminMessageTemplate);
            }

            var hireFormCustomerMessageTemplate = (await _messageTemplateService.GetMessageTemplatesByNameAsync(HireFormDefaults.HireFormSubmittedCustomerMessageTemplateSystemName)).FirstOrDefault();

            if (hireFormCustomerMessageTemplate == null)
            {
                hireFormCustomerMessageTemplate = new MessageTemplate
                {
                    Name = HireFormDefaults.HireFormSubmittedCustomerMessageTemplateSystemName,
                    Subject = "%Store.Name%. Thanks for contacting us",
                    Body = $"<p>{Environment.NewLine}<a href=\"%Store.URL%\">%Store.Name%</a>{Environment.NewLine}<br />{Environment.NewLine}<br />{Environment.NewLine}Hello, %HireForm.CustomerName%,{Environment.NewLine}<br />{Environment.NewLine} Thanks for submitting hire form.",
                    IsActive = true,
                };
                await _messageTemplateService.InsertMessageTemplateAsync(hireFormCustomerMessageTemplate);
            }

            await _localizationService.AddOrUpdateLocaleResourceAsync(GetLocaleResources());
            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            var hireFormAdminMessageTemplate = (await _messageTemplateService.GetMessageTemplatesByNameAsync(HireFormDefaults.HireFormSubmittedAdminMessageTemplateSystemName)).FirstOrDefault();
            if (hireFormAdminMessageTemplate != null)
                await _messageTemplateService.DeleteMessageTemplateAsync(hireFormAdminMessageTemplate);

            var hireFormCustomerMessageTemplate = (await _messageTemplateService.GetMessageTemplatesByNameAsync(HireFormDefaults.HireFormSubmittedCustomerMessageTemplateSystemName)).FirstOrDefault();
            if (hireFormCustomerMessageTemplate != null)
                await _messageTemplateService.DeleteMessageTemplateAsync(hireFormCustomerMessageTemplate);

            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Widgets.HireForms");
            await base.UninstallAsync();
        }

        public override async Task UpdateAsync(string currentVersion, string targetVersion)
        {
            if (currentVersion == "1.3" && targetVersion == "1.4")
            {
                await _localizationService.AddOrUpdateLocaleResourceAsync(GetLocaleResources());
            }
            await base.UpdateAsync(currentVersion, targetVersion);
        }
        #region Locale Resources 
        public Dictionary<string, string> GetLocaleResources()
        {
            return new Dictionary<string, string>
            {
                ["Plugins.Widgets.HireForms.AdminMenuTitle"] = "Hire Form",
                ["Plugins.Widgets.HireForms.AdminMenuTitle.HirePeriodsTitle"] = "Hire options",
                ["Plugins.Widgets.HireForms.AdminMenuTitle.Configure"] = "Configure",
                ["Plugins.Widgets.HireForms.AdminMenuTitle.HireRequestTitle"] = "Hire requests",
                ["Plugins.Widgets.HireForms.HireRequests"] = "Hire requests",
                ["Plugins.Widgets.HireForms.HireRequests.NoRequests"] = "There are no request to export",
                ["Plugins.Widgets.HireForms.HirePeriods"] = "Hire Periods",
                ["Plugins.Widgets.HireForms.HirePeriods.Domain.Name"] = "Name",
                ["Plugins.Widgets.HireForms.HirePeriods.Domain.Name.Hint"] = "Give an appropriate name for the hire period (ex. For 12 hours).",
                ["Plugins.Widgets.HireForms.HirePeriods.HirePeriodSearchModel.Name"] = "Period name",
                ["Plugins.Widgets.HireForms.HirePeriods.HirePeriodSearchModel.Name.Hint"] = "Search by period.",
                ["Plugins.Widgets.HireForms.HirePeriods.Validation.HirePeriod"] = "Please select a hire period",
                ["Plugins.Widgets.HireForms.HirePeriods.Validation.Name"] = "Name is required.",
                ["Plugins.Widgets.HireForms.HirePeriods.Validation.Email"] = "Please enter a valid email.",
                ["Plugins.Widgets.HireForms.HirePeriods.Validation.Phone"] = "Please enter a phone number.",
                ["Plugins.Widgets.HireForms.HirePeriods.Validation.Quantity"] = "Quantity must be at least one.",
                ["Plugins.Widgets.HireForms.HirePeriods.Form.SelectStartDate"] = "Select Start Date",
                ["Plugins.Widgets.HireForms.HirePeriods.Form.SelectEndDate"] = "Select End Date",
                ["Plugins.Widgets.HireForms.HirePeriods.Form.HireFormName"] = "Name",
                ["Plugins.Widgets.HireForms.HirePeriods.Form.HireFormEmail"] = "Email",
                ["Plugins.Widgets.HireForms.HirePeriods.Form.HireFormPhone"] = "Phone",
                ["Plugins.Widgets.HireForms.HirePeriods.Form.EnterQuantity"] = "Enter quantity",
                ["Plugins.Widgets.HireForms.HirePeriods.Form.Validation.Quantity"] = "Quantity is required.",
                ["Plugins.Widgets.HireForms.HirePeriods.AddNew"] = "Create a period",
                ["Plugins.Widgets.HireForms.HirePeriods.EditPeriodDetails"] = "Edit period details",
                ["Plugins.Widgets.HireForms.HirePeriods.BackToList"] = "Back to hire periods",
                ["Plugins.Widgets.HireForms.HireRequests.BackToList"] = "Back to hire requests",
                ["Plugins.Widgets.HireForms.HirePeriods.Name"] = "Hire periods",
                ["Plugins.Widgets.HireForms.HirePeriods.HirePeriodInfo"] = "Hire period info",
                ["Plugins.Widgets.HireForms.HireRequests.HirePeriodInfo"] = "Hire request info",
                ["Plugins.Widgets.HireForms.HirePeriods.HirePeriodAdded"] = "A new hire period added successfully",
                ["Plugins.Widgets.HireForms.HirePeriods.HirePeriodUpdated"] = "Hire period updated successfully",
                ["Plugins.Widgets.HireForms.HirePeriods.SelectHirePeriod"] = "Select Hire Period",
                ["Plugins.Widgets.HireForms.HirePeriods.PageTitle"] = "Hire periods",
                ["Plugins.Widgets.HireForms.AvailableForHire"] = "Available for hire",
                ["Plugins.Widgets.HireForms.Table.HireRequest.Name"] = "Name",
                ["Plugins.Widgets.HireForms.Table.HireRequest.Email"] = "Email",
                ["Plugins.Widgets.HireForms.Table.HireRequest.Phone"] = "Phone",
                ["Plugins.Widgets.HireForms.Table.HireRequest.PeriodName"] = "Period Name",
                ["Plugins.Widgets.HireForms.Table.HireRequest.CreatedOn"] = "Created on",
                ["Plugins.Widgets.HireForms.Table.HireRequest.StartDate"] = "Start date",
                ["Plugins.Widgets.HireForms.Table.HireRequest.EndDate"] = "End date",
                ["Plugins.Widgets.HireForms.Table.HireRequest.Search.EamilOrName"] = "Customer Name or Email",
                ["Plugins.Widgets.HireForms.Table.HireRequest.Search.EamilOrName.Hint"] = "Search by customer name or email.",
                ["Plugins.Widgets.HireForms.Table.HireRequest.Search.HirePeriodId"] = "Period",
                ["Plugins.Widgets.HireForms.Table.HireRequest.Search.HirePeriodId.Hint"] = "Searh by period.",
                ["Plugins.Widgets.HireForms.HireRequests.EditRequestDetails"] = "Edit details",
                ["Plugins.Widgets.HireForms.HireRequests.Edit.Name"] = "Name",
                ["Plugins.Widgets.HireForms.HireRequests.Edit.Name.Hint"] = "The name of the requester",
                ["Plugins.Widgets.HireForms.HireRequests.Edit.Email"] = "Email",
                ["Plugins.Widgets.HireForms.HireRequests.Edit.Email.Hint"] = "Email of the requester",
                ["Plugins.Widgets.HireForms.HireRequests.Edit.Phone"] = "Phone",
                ["Plugins.Widgets.HireForms.HireRequests.Edit.Phone.Hint"] = "Phone number of the requester",
                ["Plugins.Widgets.HireForms.HireRequests.Edit.Quantity"] = "Quantity",
                ["Plugins.Widgets.HireForms.HireRequests.Edit.Quantity.Hint"] = "Quantity of the requested product",
                ["Plugins.Widgets.HireForms.HireRequests.Edit.CreatedOn"] = "Created on",
                ["Plugins.Widgets.HireForms.HireRequests.Edit.CreatedOn.Hint"] = "Entry date of this request",
                ["Plugins.Widgets.HireForms.HireRequests.Edit.StartDate"] = "Start date",
                ["Plugins.Widgets.HireForms.HireRequests.Edit.StartDate.Hint"] = "Start date of the request",
                ["Plugins.Widgets.HireForms.HireRequests.Edit.EndDate"] = "End date",
                ["Plugins.Widgets.HireForms.HireRequests.Edit.EndDate.Hint"] = "End date of the request",
                ["Plugins.Widgets.HireForms.HireRequests.Edit.HirePeriodName"] = "Period",
                ["Plugins.Widgets.HireForms.HireRequests.Edit.HirePeriodName.Hint"] = "The type of the period",
                ["Plugins.Widgets.HireForms.Submission.Success"] = "Your hire request has been submitted!",
                ["Plugins.Widgets.HireForms.Configure.Instructions"] = "Please provide below informations.",
                ["Plugins.Widgets.HireForms.Configure.InfoBody"] = "Information body",
                ["Plugins.Widgets.HireForms.Configure.InfoBody.Hint"] = "Information body",
                ["Plugins.Widgets.HireForms.PublicButton.RequestHire"] = "Request hire",
                ["Plugins.Widgets.HireForms.Public.RequestHire.ParseError"] = "Something went wrong!",

            };
        }

        public async Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            var menu = new SiteMapNode()
            {
                Title = await _localizationService.GetResourceAsync("Plugins.Widgets.HireForms.AdminMenuTitle"),
                Visible = true,
                IconClass = "far fa-dot-circle",
            };

            var hirePeriods = new SiteMapNode()
            {
                Title = await _localizationService.GetResourceAsync("Plugins.Widgets.HireForms.AdminMenuTitle.HirePeriodsTitle"),
                Visible = true,
                IconClass = "far fa-dot-circle",
                ControllerName = "HirePeriod",
                ActionName = "Index",
                SystemName = "Widgets.HireForms.HirePeriods",
            };

            menu.ChildNodes.Add(hirePeriods);

            var requestNode = new SiteMapNode()
            {
                Title = await _localizationService.GetResourceAsync("Plugins.Widgets.HireForms.AdminMenuTitle.HireRequestTitle"),
                Visible = true,
                IconClass = "far fa-dot-circle",
                ControllerName = "HireRequest",
                ActionName = "Index",
                SystemName = "Widgets.HireForms.HireRequests",
            };

            menu.ChildNodes.Add(requestNode);

            var configureNode = new SiteMapNode()
            {
                Title = await _localizationService.GetResourceAsync("Plugins.Widgets.HireForms.AdminMenuTitle.Configure"),
                Visible = true,
                IconClass = "far fa-dot-circle",
                ControllerName = "HireRequestPlugin",
                ActionName = "Configure",
                SystemName = "Widgets.HireForms.Configure",
            };

            menu.ChildNodes.Add(configureNode);

            rootNode.ChildNodes.Insert(rootNode.ChildNodes.Count, menu);
        }

        #endregion

    }
}