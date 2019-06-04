using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WikiLibs.Shared.Modules.Smtp;
using Module = WikiLibs.Shared.Attributes.Module;

namespace WikiLibs.Smtp
{
    [Module(Interface = typeof(ISmtpManager))]
    public class SmtpManager : ISmtpManager
    {
        private readonly Config _config;
        private readonly MailjetClient _client;

        public SmtpManager(Config cfg)
        {
            _config = cfg;
            _client = new MailjetClient(cfg.ApiKey, cfg.ApiSecret);
            _client.Version = ApiVersion.V3;
        }

        private JToken GetVarValueAsJToken(object value, PropertyInfo prop)
        {
            if (value == null)
                return (null);
            if (prop.PropertyType == typeof(int))
                return ((int)value);
            if (prop.PropertyType == typeof(uint))
                return ((uint)value);
            if (prop.PropertyType == typeof(long))
                return ((long)value);
            if (prop.PropertyType == typeof(ulong))
                return ((ulong)value);
            if (prop.PropertyType == typeof(float))
                return ((float)value);
            if (prop.PropertyType == typeof(double))
                return ((double)value);
            if (prop.PropertyType == typeof(string))
                return ((string)value);
            if (prop.PropertyType == typeof(bool))
                return ((bool)value);
            return (null);
        }

        private JObject GenVariables(object viewModel)
        {
            var obj = new JObject();

            foreach (var prop in viewModel.GetType().GetProperties())
            {
                var p = GetVarValueAsJToken(prop.GetValue(viewModel), prop);
                if (p != null)
                    obj.Add(prop.Name, p);
            }
            return (obj);
        }

        private JArray GenRecipients(Mail msg, JObject vars)
        {
            var recipients = new JArray();

            foreach (var recv in msg.Recipients)
            {
                recipients.Add(new JObject()
                {
                    { "Email", recv.Email },
                    { "Name", recv.Name },
                    { "Vars", vars }
                });
            }
            return (recipients);
        }

        public async Task<bool> SendAsync(Mail msg)
        {
            var vars = GenVariables(msg.Model);
            var recipients = GenRecipients(msg, vars);
            var req = new MailjetRequest()
            {
                Resource = Send.Resource
            }
            .Property("FromEmail", _config.FromEmail)
            .Property("FromName", _config.FromName)
            .Property("Subject", msg.Subject)
            .Property("Mj-TemplateID", msg.Template)
            .Property("Mj-TemplateLanguage", true)
            .Property("Recipients", recipients);

            var response = await _client.PostAsync(req);
            return (response.IsSuccessStatusCode);
        }
    }
}
