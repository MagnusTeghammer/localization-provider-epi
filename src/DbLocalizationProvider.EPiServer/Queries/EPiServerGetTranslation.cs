﻿using DbLocalizationProvider.Abstractions;
using DbLocalizationProvider.AspNet.Queries;
using DbLocalizationProvider.Queries;
using EPiServer.Framework.Localization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;

namespace DbLocalizationProvider.EPiServer.Queries
{
    public class EPiServerGetTranslation
    {
        public class Handler : IQueryHandler<GetTranslation.Query, string>
        {
            public string Execute(GetTranslation.Query query)
            {
                var service = ServiceLocator.Current.GetInstance<LocalizationService>();
                return service.GetStringByCulture(query.Key, query.Language);
            }
        }

        public class HandlerWithLogging : IQueryHandler<GetTranslation.Query, string>
        {
            private readonly GetTranslationHandler _inner;
            private readonly ILogger _logger;

            public HandlerWithLogging(GetTranslationHandler inner)
            {
                _inner = inner;
                _logger = LogManager.GetLogger(typeof(Handler));
            }

            public string Execute(GetTranslation.Query query)
            {
                var result = _inner.Execute(query);
                if(result == null)
                    _logger.Warning($"MISSING: Resource Key (culture: {query.Language.Name}): {query.Key}. Probably class is not decorated with either [LocalizedModel] or [LocalizedResource] attribute.");

                return result;
            }
        }
    }
}
