using FluentEmail.Core;
using FluentEmail.Core.Models;
using FluentEmail.Razor;
using FluentEmail.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules.Smtp;

namespace WikiLibs.Smtp
{
    [Module(Interface = typeof(ISmtpManager))]
    public class SmtpManager : ISmtpManager
    {
        private readonly Config _config;
        private readonly string _webRoot;
        private readonly IFluentEmailFactory _factory;

        public SmtpManager(Config cfg, IFluentEmailFactory factory, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            _config = cfg;
            _factory = factory;
            _webRoot = env.WebRootPath;
        }

        public async Task SendAsync(Mail msg)
        {
            var email = _factory.Create();

            email.SetFrom(_config.FromEmail, _config.FromName)
                .Subject(msg.Subject)
                .To(msg.Recipients.Select(e => new Address()
                {
                    EmailAddress = e.Email,
                    Name = e.Name
                }).ToList())
                .CC(msg.CCRecipients.Select(e => new Address()
                {
                    EmailAddress = e.Email,
                    Name = e.Name
                }).ToList())
                .UsingTemplateFromFile(_webRoot + "\\MailTemplates\\" + msg.Template + ".cshtml", msg.Model);
            await email.SendAsync();
        }

        [ModuleConfigurator]
        public static void SetupEmailSystem(IServiceCollection services, Config cfg)
        {
            var client = new SmtpClient();
            client.Credentials = new NetworkCredential(cfg.Username, cfg.Password);
            client.Host = cfg.Host;
            client.Port = cfg.Port;

            services.AddFluentEmail(cfg.FromEmail)
                .AddRazorRenderer()
                .AddSmtpSender(client);
        }
    }
}
