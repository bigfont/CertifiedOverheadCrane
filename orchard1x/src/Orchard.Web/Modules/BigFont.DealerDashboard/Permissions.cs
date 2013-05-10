using System.Collections.Generic;
using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;

namespace BigFont.DealerDashboard {
    public class Permissions : IPermissionProvider {
        public static readonly Permission SomePermission = new Permission { Description = "Some permission", Name = "SomePermission" };

        public virtual Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions() {
            return new[] {
                SomePermission
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] {SomePermission}
                },
                new PermissionStereotype {
                    Name = "Anonymous",
                    Permissions = new[] {SomePermission}
                },
                new PermissionStereotype {
                    Name = "Authenticated",
                    Permissions = new[] {SomePermission}
                },
                new PermissionStereotype {
                    Name = "Editor",
                    Permissions = new[] {SomePermission}
                },
                new PermissionStereotype {
                    Name = "Moderator",
                    Permissions = new[] {SomePermission}
                },
                new PermissionStereotype {
                    Name = "Author",
                    Permissions = new[] {SomePermission}
                },
                new PermissionStereotype {
                    Name = "Contributor",
                    Permissions = new[] {SomePermission}
                },
            };
        }
    }
}
