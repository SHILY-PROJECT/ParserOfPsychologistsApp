using System;
using System.Web;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Microsoft.Extensions.DependencyInjection;
using HtmlAgilityPack;
using ParserOfPsychologists.Application.Parser;
using ParserOfPsychologists.Application.Models;
using ParserOfPsychologists.Application.Models.ResponseBackend;
using ParserOfPsychologists.Application.Toolkit;
using ParserOfPsychologists.Application.Interfaces;
using ParserOfPsychologists.Application.Common;
using ParserOfPsychologists.Application.Configuration;
using ParserOfPsychologists.Application.CustomEventArgs;

namespace ParserOfPsychologists.Application.Configuration;

public static class ApplicationRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        ApplicationRegistration.AddErrorsForVerifyContent();

        var cfg = HttpClientConfiguration.CreateConfiguration();

        services
            .AddScoped<IApplicationFacade, ApplicationFacade>()
            .AddScoped<ICityHandlerModule, CityHandlerModule>()
            .AddScoped<IParser, MainParser>()
            .AddScoped<IPageNavigator, PageNavigator>()
            .AddScoped<IParserSettings, ParserSettings>()
            .AddScoped<IHttpClientConfiguration, HttpClientConfiguration>(opt => cfg)
            .AddScoped<IKeeperOfResult, KeeperOfResult>()
            .AddScoped<IAuthorization, AuthorizationModule>()
            .AddScoped<IAccountManager, AccountManager>()
            .AddScoped<AccountData>()
            .AddScoped<CaptchaModel>()
            .AddTransient(opt => HttpHelper.CreateHttpClient(cfg));

        return services;
    }

    private static void AddErrorsForVerifyContent() => HttpHelper.AddErrorsForVerifyContent(new[]
    {
        "Доступ к сайту b17.ru для вашего IP адреса временно заблокирован"
    });
}