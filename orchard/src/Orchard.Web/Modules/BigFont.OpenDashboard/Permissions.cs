using System.Collections.Generic;
using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;

namespace BigFont.OpenDashboard {
    public class Permissions : IPermissionProvider {
        public static readonly Permission ManageOpenDashboard = new Permission { Description = "Manage OpenDashboard", Name = "Manage OpenDashboard" };

        public virtual Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions() {
            return new[] {
                ManageOpenDashboard
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return new[] {
                new PermissionStereotype {
                    Name = "Authenticated",
                    Permissions = new[] {ManageOpenDashboard}
                },
            };
        }
    }
}

