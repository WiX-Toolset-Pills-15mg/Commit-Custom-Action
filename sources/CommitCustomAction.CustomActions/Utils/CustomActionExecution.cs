// WiX Toolset Pills 15mg
// Copyright (C) 2019-2022 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Diagnostics;
using System.Reflection;
using CommitCustomAction.CustomActions.Business;
using Microsoft.Deployment.WindowsInstaller;

namespace CommitCustomAction.CustomActions.Utils
{
    internal class CustomActionExecution
    {
        public string CustomActionName { get; set; }

        public Session Session { get; set; }

        public Action Action { get; set; }

        public ActionResult Run()
        {
            string computedCustomActionName = CustomActionName ?? "<Unnamed Custom Action>";

            Session.Log($"--> Begin {computedCustomActionName}");
            try
            {
                Action();
            }
            catch (WixToolsetPillWarningException ex)
            {
                Session.Log($"WARNING: {ex}");
            }
            catch (Exception ex)
            {
                Session.Log($"ERROR: {ex}");
                return ActionResult.Failure;
            }
            finally
            {
                Session.Log($"<-- End {computedCustomActionName}");
            }

            return ActionResult.Success;
        }

        public static ActionResult Run(Session session, Action action)
        {
            StackTrace stackTrace = new StackTrace();
            MethodBase methodInfo = stackTrace.GetFrame(1).GetMethod();

            CustomActionAttribute customActionAttribute = methodInfo.GetCustomAttribute<CustomActionAttribute>();
            bool customActionHasExplicitName = customActionAttribute != null && !string.IsNullOrEmpty(customActionAttribute.Name);

            string customActionName = customActionHasExplicitName
                ? customActionAttribute.Name
                : methodInfo.Name;
            
            CustomActionExecution execution = new CustomActionExecution
            {
                CustomActionName = customActionName,
                Session = session,
                Action = action
            };

            return execution.Run();
        }

        public static ActionResult Run(Session session, string customActionName, Action action)
        {
            CustomActionExecution execution = new CustomActionExecution
            {
                CustomActionName = customActionName,
                Session = session,
                Action = action
            };

            return execution.Run();
        }
    }
}
