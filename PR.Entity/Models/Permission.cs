using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PR.Entity.Models
{
    [Table("Permission")]
    public partial class Permission
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        [StringLength(150)]
        public string Name { get; set; }
        [Column("lastName")]
        [StringLength(150)]
        public string LastName { get; set; }
        [Column("idTypePermission")]
        public int IdPermissionType { get; set; }
        [Column("date", TypeName = "datetime")]
        public DateTime Date { get; set; }

        [ForeignKey(nameof(IdPermissionType))]
        [InverseProperty(nameof(TypePermission.Permission))]
        public virtual TypePermission PermissionType { get; set; }
    }
}
