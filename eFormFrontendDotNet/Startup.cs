using Microsoft.Owin;
using Owin;
//using Microting;
//using System;

[assembly: OwinStartupAttribute(typeof(eFormFrontendDotNet.Startup))]
namespace eFormFrontendDotNet
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
