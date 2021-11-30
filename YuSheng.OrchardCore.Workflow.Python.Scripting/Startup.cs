using Fluid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Modules;
using OrchardCore.Workflows.Helpers;
using Python.Runtime;
using YuSheng.OrchardCore.Workflow.Python.Scripting.Activities;
using YuSheng.OrchardCore.Workflow.Python.Scripting.Drivers;

namespace YuSheng.OrchardCore.Workflow.Python.Scripting
{
    [Feature("OrchardCore.Workflows")]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {

            services.AddActivity<PythonScriptTask, ScriptTaskDisplayDriver>(); ;


        }
    }
}
