using System.Collections.Generic;
using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;

namespace BigFont.DealerDashboard {
    public class Permissions : IPermissionProvider {
        public static readonly Permission ManageDealerDashboard = new Permission { Description = "Manage dealer dashboard", Name = "Manage Dealer Dashboard" };

        public virtual Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions() {
            return new[] {
                ManageDealerDashboard
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] {ManageDealerDashboard}
                },
                new PermissionStereotype {
                    Name = "Dealer",
                    Permissions = new[] {ManageDealerDashboard}
                },
            };
        }
    }
}

