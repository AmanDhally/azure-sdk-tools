﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

namespace Microsoft.WindowsAzure.Commands.ServiceManagement.IaaS
{
    using System;
    using System.Linq;
    using System.Management.Automation;
    using Commands.Utilities.Common;
    using Helpers;
    using Microsoft.WindowsAzure.Commands.ServiceManagement.Model;
    using Microsoft.WindowsAzure.Management.Compute;
    using Microsoft.WindowsAzure.Management.Compute.Models;
    using Properties;

    [Cmdlet(VerbsLifecycle.Start, "AzureVM", DefaultParameterSetName = "ByName"), OutputType(typeof(ManagementOperationContext))]
    public class StartAzureVMCommand : IaaSDeploymentManagementCmdletBase
    {
        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The name of the Virtual Machine to start.", ParameterSetName = "ByName")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The Virtual Machine to restart.", ParameterSetName = "Input")]
        [ValidateNotNullOrEmpty]
        [Alias("InputObject")]
        public PersistentVM VM { get; set; }

        protected override void ExecuteCommand()
        {
            ServiceManagementProfile.Initialize();

            base.ExecuteCommand();

            if (this.CurrentDeploymentNewSM == null)
            {
                return;
            }

            string roleName = (this.ParameterSetName == "ByName") ? this.Name : this.VM.RoleName;
            
            // Generate a list of role names matching wildcard patterns or
            // the exact name specified in the -Name parameter.
            var roleNames = PersistentVMHelper.GetRoleNames(this.CurrentDeploymentNewSM.RoleInstances, roleName);

            // Insure at least one of the role name instances can be found.
            if ((roleNames == null) || (!roleNames.Any()))
            {
                throw new ArgumentOutOfRangeException(String.Format(Resources.RoleInstanceCanNotBeFoundWithName, Name));
            }

            var parameters = new VirtualMachineStartRolesParameters();
            foreach(var r in roleNames)
            {
                parameters.Roles.Add(r);
            }

            this.ExecuteClientActionNewSM(
                null,
                this.CommandRuntime.ToString(),
                () => this.ComputeClient.VirtualMachines.StartRoles(this.ServiceName, this.CurrentDeploymentNewSM.Name, parameters));
        }
    }
}
