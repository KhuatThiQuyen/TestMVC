using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using System;

[assembly: OwinStartup(typeof(School_Manager.StartUp))]

namespace School_Manager
{
    public partial class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {

                LoginPath = new PathString("/Users/Login"),
                LogoutPath = new PathString("/Users/LogOff"),
                CookieSecure = CookieSecureOption.SameAsRequest,
                ExpireTimeSpan = TimeSpan.FromMinutes(30.0)
            });
        }

    }
    
}   