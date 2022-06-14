using OrchardCore.Workflows.Display;
using OrchardCore.Workflows.Models;
using YuSheng.OrchardCore.Workflow.Python.Scripting.Activities;
using YuSheng.OrchardCore.Workflow.Python.Scripting.ViewModels;

namespace YuSheng.OrchardCore.Workflow.Python.Scripting.Drivers
{
    public class ScriptTaskDisplayDriver : ActivityDisplayDriver<PythonScriptTask, ScriptTaskViewModel>
    {
        protected override void EditActivity(PythonScriptTask source, ScriptTaskViewModel model)
        {
            model.PythonDllFilePath = source.PythonDllFilePath.ToString();
            model.Script = source.Script.Expression;
        }

        protected override void UpdateActivity(ScriptTaskViewModel model, PythonScriptTask activity)
        {
            activity.PythonDllFilePath = new WorkflowExpression<string>(model.PythonDllFilePath);
            activity.Script = new WorkflowExpression<object>(model.Script);
        }
    }
}
