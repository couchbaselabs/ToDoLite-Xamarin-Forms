using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Auth;

namespace ToDoLiteXamarinForms.Auth
{
    public class Authentication
    {
        public OAuth2Authenticator Auth { get; private set; }
        public string Token { get; set; }

        public Authentication()
        {
            Auth = new OAuth2Authenticator
                (
                 "510926975701287",
                 "",
                 new Uri("https://m.facebook.com/dialog/oauth/"),
                 new Uri("http://www.facebook.com/connect/login_success.html")
                );

            Auth.Completed += auth_Completed;
            Auth.Title = "Todo Lite";
        }

        void auth_Completed(object sender, Xamarin.Auth.AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {

            }
        }
    }
}
