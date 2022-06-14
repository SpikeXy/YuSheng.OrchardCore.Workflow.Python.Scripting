using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrchardCore.Security.Permissions;

namespace YuSheng.OrchardCore.Workflow.Python.Scripting
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageWorkflows = new Permission("ManageWorkflows", "Manage workflows", isSecurityCritical: true);

        public static readonly Permission ExecuteWorkflows = new Permission("ExecuteWorkflows", "Execute workflows", isSecurityCritical: true);

        public Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            return Task.FromResult(new[] { ManageWorkflows,ExecuteWorkflows }.AsEnumerable());
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] { ManageWorkflows,ExecuteWorkflows }
                }
            };
        }
    }
}
