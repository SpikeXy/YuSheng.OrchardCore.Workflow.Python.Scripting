using System;
using System.Linq;
using System.Threading.Tasks;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Workflows.Activities;
using OrchardCore.Workflows.Display;
using OrchardCore.Workflows.Models;
using OrchardCore.Workflows.ViewModels;
using YuSheng.OrchardCore.Workflow.Python.Scripting.Activities;
using YuSheng.OrchardCore.Workflow.Python.Scripting.ViewModels;

namespace YuSheng.OrchardCore.Workflow.Python.Scripting.Drivers
{
    public class ScriptTaskDisplayDriver : ActivityDisplayDriver<PythonScriptTask, ScriptTaskViewModel>
    {
        protected override void EditActivity(PythonScriptTask source, ScriptTaskViewModel model)
        {
            model.TempPythonFileName = source.TempPythonFileName.ToString();
            model.PythonDllFilePath = source.PythonDllFilePath.ToString();
            model.Script = source.Script.Expression;
        }

        protected override void UpdateActivity(ScriptTaskViewModel model, PythonScriptTask activity)
        {
            activity.TempPythonFileName = new WorkflowExpression<string>(model.TempPythonFileName);
            activity.PythonDllFilePath = new WorkflowExpression<string>(model.PythonDllFilePath);
            activity.Script = new WorkflowExpression<object>(model.Script);
        }
    }
}
