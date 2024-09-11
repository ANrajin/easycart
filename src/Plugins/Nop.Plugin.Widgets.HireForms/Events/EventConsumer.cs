using Nop.Core.Events;
using Nop.Plugin.Widgets.HireForms.Domain;
using Nop.Plugin.Widgets.HireForms.Services;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Areas.Admin.Models.Messages;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.HireForms.Events
{
    public class EventConsumer : IConsumer<EntityInsertedEvent<HireRequest>>, IConsumer<ModelPreparedEvent<BaseNopModel>>
    {

        private readonly IHireRequestNotificationService _hireRequestNotificationService;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly ILocalizationService _localizationService;

        public EventConsumer(IHireRequestNotificationService hireRequestNotificationService,
            IMessageTokenProvider messageTokenProvider,
            ILocalizationService localizationService)
        {
            _hireRequestNotificationService = hireRequestNotificationService;
            _messageTokenProvider = messageTokenProvider;
            _localizationService = localizationService;
        }
        public async Task HandleEventAsync(EntityInsertedEvent<HireRequest> eventMessage)
        {
            _ = await _hireRequestNotificationService.SendHireRequestAdminNotificationAsync(eventMessage.Entity);
            _ = await _hireRequestNotificationService.SendHireRequestCustomerNotificationAsync(eventMessage.Entity);
        }

        public async Task HandleEventAsync(ModelPreparedEvent<BaseNopModel> eventMessage)
        {
            if (eventMessage?.Model is MessageTemplateModel messageTemplateModel && (messageTemplateModel.Name.Equals(HireFormDefaults.HireFormSubmittedCustomerMessageTemplateSystemName) || messageTemplateModel.Name.Equals(HireFormDefaults.HireFormSubmittedAdminMessageTemplateSystemName)))
            {

                var allowedTokens = await _messageTokenProvider.GetListOfAllowedTokensAsync(new[] { TokenGroupNames.StoreTokens, TokenGroupNames.CustomerTokens, TokenGroupNames.ProductTokens });
                var additionalTokens = new string[]
                {
                    "%HireRequest.Name%",
                    "%HireRequest.Email%",
                    "%HireRequest.PhoneNumber%",
                    "%HireRequest.StartDate%",
                    "%HireRequest.EndDate%",
                    "%HireRequest.Quantity%"
                };

                var exetndedTokens = string.Join(", ", allowedTokens.Concat(additionalTokens));
                messageTemplateModel.AllowedTokens = $"{exetndedTokens}{Environment.NewLine}{Environment.NewLine}" +
                    $"{await _localizationService.GetResourceAsync("Admin.ContentManagement.MessageTemplates.Tokens.ConditionalStatement")}{Environment.NewLine}";
            }
        }
    }
}
