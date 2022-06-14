using System.ComponentModel.DataAnnotations;

namespace YuSheng.OrchardCore.Workflow.Python.Scripting.ViewModels
{
    public class ScriptTaskViewModel
    {
        [Required]
        public string PythonDllFilePath { get; set; }

        [Required]
        public string Script { get; set; }
    }
}
