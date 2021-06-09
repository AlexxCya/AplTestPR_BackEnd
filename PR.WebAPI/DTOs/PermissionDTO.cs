using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PR.WebAPI.DTOs
{
    public class PermissionDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }
        public int idPermissionType { get; set; }
        public string permissionType { get; set; }
        public string date { get; set; }
    }
}
