using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Workflows.Helpers;
using YuSheng.OrchardCore.Workflow.Python.Scripting.Activities;
using YuSheng.OrchardCore.Workflow.Python.Scripting.Drivers;

namespace YuSheng.OrchardCore.Workflow.Python.Scripting
{
    [Feature("YuSheng.OrchardCore.Workflow.Python.Scripting")]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {

            services.AddActivity<PythonScriptTask, ScriptTaskDisplayDriver>(); ;


        }
    }
}
