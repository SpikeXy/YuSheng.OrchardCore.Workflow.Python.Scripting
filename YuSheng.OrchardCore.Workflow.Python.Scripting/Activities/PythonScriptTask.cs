using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OrchardCore.Workflows.Abstractions.Models;
using OrchardCore.Workflows.Activities;
using OrchardCore.Workflows.Models;
using OrchardCore.Workflows.Services;
using Python.Runtime;

namespace YuSheng.OrchardCore.Workflow.Python.Scripting.Activities
{
    public class PythonScriptTask : TaskActivity
    {
        private readonly IWorkflowScriptEvaluator _scriptEvaluator;
        private readonly IStringLocalizer S;
        private readonly IWorkflowExpressionEvaluator _expressionEvaluator;
        public PythonScriptTask(IWorkflowScriptEvaluator scriptEvaluator,
            IWorkflowExpressionEvaluator expressionEvaluator,
            IStringLocalizer<PythonScriptTask> localizer)
        {
            _scriptEvaluator = scriptEvaluator;
            _expressionEvaluator = expressionEvaluator;
            S = localizer;

        }

        public override string Name => nameof(PythonScriptTask);

        public override LocalizedString DisplayText => S["Python Script Task"];

        public override LocalizedString Category => S["Control Flow"];

        public WorkflowExpression<string> PythonDllFilePath
        {
            get => GetProperty(() => new WorkflowExpression<string>());
            set => SetProperty(value);
        }

        public WorkflowExpression<string> TempPythonFileName
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
            var tempPythonFileName = await _expressionEvaluator.EvaluateAsync(TempPythonFileName, workflowContext, null);
            var pythonDllFilePath = await _expressionEvaluator.EvaluateAsync(PythonDllFilePath, workflowContext, null);

            if ( !string.IsNullOrEmpty(pythonDllFilePath) && !string.IsNullOrEmpty(tempPythonFileName))
			{
                string code = "";
                try
				{
                    var pythonTask = Task.Run(() =>
                    {

                        Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pythonDllFilePath);

                        using (Py.GIL())
                        {
                            // create a Python scope
                            using (var scope = Py.CreateScope())
                            {
                                scope.Exec(Script.Expression);
                                using (var streamReader = new StreamReader(tempPythonFileName, Encoding.UTF8))
                                {
                                    code = streamReader.ReadToEnd();
                                }
                            }
                            File.Delete(tempPythonFileName);
                        }
                    });
                    TimeSpan ts = TimeSpan.FromMinutes(5);
                    if (!pythonTask.Wait(ts))
                        code = "The task timeout .";
                }
				catch (System.Exception ex)
				{
                    code = ex.Message;
				}
                workflowContext.Output.Add("PythonScript", code);            
            }
            
            return Outcomes("Done");
        }
    }
}
