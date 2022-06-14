using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using OrchardCore.Workflows.Abstractions.Models;
using OrchardCore.Workflows.Activities;
using OrchardCore.Workflows.Models;
using OrchardCore.Workflows.Services;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;


namespace YuSheng.OrchardCore.Workflow.Python.Scripting.Activities
{
    public class PythonScriptTask : TaskActivity
    {
        private readonly IWorkflowScriptEvaluator _scriptEvaluator;
        private readonly IStringLocalizer S;
        private readonly IHtmlHelper _htmlHelper;
        private readonly IWorkflowExpressionEvaluator _expressionEvaluator;
        public PythonScriptTask(IWorkflowScriptEvaluator scriptEvaluator,
            IWorkflowExpressionEvaluator expressionEvaluator,
            IHtmlHelper htmlHelper,
            IStringLocalizer<PythonScriptTask> localizer)
        {
            _scriptEvaluator = scriptEvaluator;
            _expressionEvaluator = expressionEvaluator;
            S = localizer;
            _htmlHelper = htmlHelper;

        }

        public override string Name => nameof(PythonScriptTask);

        public override LocalizedString DisplayText => S["Python Script Task"];

        public override LocalizedString Category => S["Control Flow"];

        public WorkflowExpression<string> PythonDllFilePath
        {
            get => GetProperty(() => new WorkflowExpression<string>());
            set => SetProperty(value);
        }

        /// <summary>
        /// The script can call any available functions, including setOutcome().
        /// </summary>
        public WorkflowExpression<object> Script
        {
            get => GetProperty(() => new WorkflowExpression<object>("setOutcome('Done');"));
            set => SetProperty(value);
        }

        public override IEnumerable<Outcome> GetPossibleOutcomes(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            return Outcomes(S["Done"]);
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {

            var tempPythonFileName = Guid.NewGuid().ToString();
            var pythonDllFilePath = await _expressionEvaluator.EvaluateAsync(PythonDllFilePath, workflowContext, null);

            if (!string.IsNullOrEmpty(pythonDllFilePath))
            {
                string code = "";
                try
                {
                    Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pythonDllFilePath);
                    PythonEngine.Initialize();
                    using (Py.GIL())
                    {
                        // create a Python scope
                        using (var scope = Py.CreateScope())
                        {
                            scope.Set("pythonfile", tempPythonFileName);
                            scope.Exec(Script.Expression);
                            using (var streamReader = new StreamReader(tempPythonFileName, Encoding.UTF8))
                            {
                                code = streamReader.ReadToEnd();
                            }
                        }
                        File.Delete(tempPythonFileName);
                    }
                    PythonEngine.Shutdown();
                }
                catch (Exception ex)
                {
                    code = ex.Message;
                }

                workflowContext.Output["PythonScript"] = _htmlHelper.Raw(_htmlHelper.Encode(code)) ;
            }

            return Outcomes("Done");
        }
    }
}
