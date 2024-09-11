using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Messages;
using Nop.Core.Events;
using Nop.Plugin.Widgets.HireForms.Domain;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.HireForms.Services
{
    public class HireRequestNotificationService : IHireRequestNotificationService
    {
        #region Fields

        private readonly IStoreContext _storeContext;
        private readonly LocalizationSettings _localizationSettings;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IEmailAccountService _emailAccountService;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly IProductService _productService;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWorkflowMessageService _workflowMessageService;

        #endregion

        #region CTOR

        public HireRequestNotificationService(IStoreContext storeContext,
            LocalizationSettings localizationSettings,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IEmailAccountService emailAccountService,
            EmailAccountSettings emailAccountSettings,
            IMessageTemplateService messageTemplateService,
            IMessageTokenProvider messageTokenProvider,
            IProductService productService,
            IEventPublisher eventPublisher,
            IWorkflowMessageService workflowMessageService)
        {
            _storeContext = storeContext;
            _localizationSettings = localizationSettings;
            _languageService = languageService;
            _localizationService = localizationService;
            _emailAccountService = emailAccountService;
            _emailAccountSettings = emailAccountSettings;
            _productService = productService;
            _messageTemplateService = messageTemplateService;
            _messageTokenProvider = messageTokenProvider;
            _eventPublisher = eventPublisher;
            _workflowMessageService = workflowMessageService;
        }

        #endregion

        #region Utilities
        protected virtual async Task<int> EnsureLanguageIsActiveAsync(int languageId, int storeId)
        {
            //load language by specified ID
            var language = await _languageService.GetLanguageByIdAsync(languageId);

            if (language == null || !language.Published)
            {
                //load any language from the specified store
                language = (await _languageService.GetAllLanguagesAsync(storeId: storeId)).FirstOrDefault();
            }

            if (language == null || !language.Published)
            {
                //load any language
                language = (await _languageService.GetAllLanguagesAsync()).FirstOrDefault();
            }

            if (language == null)
                throw new Exception("No active language could be loaded");

            return language.Id;
        }
        protected virtual async Task<EmailAccount> GetEmailAccountOfMessageTemplateAsync(MessageTemplate messageTemplate, int languageId)
        {
            var emailAccountId = await _localizationService.GetLocalizedAsync(messageTemplate, mt => mt.EmailAccountId, languageId);
            //some 0 validation (for localizable "Email account" dropdownlist which saves 0 if "Standard" value is chosen)
            if (emailAccountId == 0)
                emailAccountId = messageTemplate.EmailAccountId;

            var emailAccount = (await _emailAccountService.GetEmailAccountByIdAsync(emailAccountId) ?? await _emailAccountService.GetEmailAccountByIdAsync(_emailAccountSettings.DefaultEmailAccountId)) ??
                               (await _emailAccountService.GetAllEmailAccountsAsync()).FirstOrDefault();
            return emailAccount;
        }
        protected virtual async Task<IList<MessageTemplate>> GetActiveMessageTemplatesAsync(string messageTemplateName, int storeId)
        {
            //get message templates by the name
            var messageTemplates = await _messageTemplateService.GetMessageTemplatesByNameAsync(messageTemplateName, storeId);

            //no template found
            if (!messageTemplates?.Any() ?? true)
                return new List<MessageTemplate>();

            //filter active templates
            messageTemplates = messageTemplates.Where(messageTemplate => messageTemplate.IsActive).ToList();

            return messageTemplates;
        }

        #endregion

        #region Methods

        public virtual async Task AddHireRequestTokensAsync(IList<Token> tokens, HireRequest hireRequest)
        {
            tokens.Add(new Token("HireRequest.Name", hireRequest?.Name ?? string.Empty));
            tokens.Add(new Token("HireRequest.Email", hireRequest?.Email ?? string.Empty));
            tokens.Add(new Token("HireRequest.PhoneNumber", hireRequest?.Phone ?? string.Empty));
            tokens.Add(new Token("HireRequest.StartDate", hireRequest?.StartDate));
            tokens.Add(new Token("HireRequest.EndDate", hireRequest?.EndDate));
            tokens.Add(new Token("HireRequest.Quantity", hireRequest?.Quantity ?? 0));
            await Task.CompletedTask;
        }

        public async Task<IList<int>> SendHireRequestAdminNotificationAsync(HireRequest hireRequest)
        {
            var product = await _productService.GetProductByIdAsync(hireRequest.CurrentProductId);
            var store = await _storeContext.GetCurrentStoreAsync();
            var languageId = await EnsureLanguageIsActiveAsync(_localizationSettings.DefaultAdminLanguageId, store.Id);

            var messageTemplates = await GetActiveMessageTemplatesAsync(HireFormDefaults.HireFormSubmittedAdminMessageTemplateSystemName, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            await AddHireRequestTokensAsync(commonTokens, hireRequest);
            await _messageTokenProvider.AddCustomerTokensAsync(commonTokens, hireRequest.CustomerId);

            return await messageTemplates.SelectAwait(async messageTemplate =>
            {
                //email account
                var emailAccount = await GetEmailAccountOfMessageTemplateAsync(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                await _messageTokenProvider.AddStoreTokensAsync(tokens, store, emailAccount);
                await _messageTokenProvider.AddProductTokensAsync(tokens, product, languageId);
                //event notification
                await _eventPublisher.MessageTokensAddedAsync(messageTemplate, tokens);
                var toEmail = emailAccount.Email;
                var toName = emailAccount.DisplayName;
                return await _workflowMessageService.SendNotificationAsync(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToListAsync();
        }

        public async Task<IList<int>> SendHireRequestCustomerNotificationAsync(HireRequest hireRequest)
        {
            var product = await _productService.GetProductByIdAsync(hireRequest.CurrentProductId);
            var store = await _storeContext.GetCurrentStoreAsync();
            var languageId = await EnsureLanguageIsActiveAsync(_localizationSettings.DefaultAdminLanguageId, store.Id);

            var messageTemplates = await GetActiveMessageTemplatesAsync(HireFormDefaults.HireFormSubmittedCustomerMessageTemplateSystemName, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();
            await AddHireRequestTokensAsync(commonTokens, hireRequest);
            await _messageTokenProvider.AddCustomerTokensAsync(commonTokens, hireRequest.CustomerId);
            await _messageTokenProvider.AddProductTokensAsync(commonTokens, product, languageId);

            return await messageTemplates.SelectAwait(async messageTemplate =>
            {
                //email account
                var emailAccount = await GetEmailAccountOfMessageTemplateAsync(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                await _messageTokenProvider.AddStoreTokensAsync(tokens, store, emailAccount);

                //event notification
                await _eventPublisher.MessageTokensAddedAsync(messageTemplate, tokens);
                var toEmail = hireRequest.Email;
                var toName = hireRequest.Name;
                return await _workflowMessageService.SendNotificationAsync(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToListAsync();
        }

        #endregion
    }
}
