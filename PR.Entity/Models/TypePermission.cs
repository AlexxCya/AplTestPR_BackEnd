using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PR.Entity.Models
{
    [Table("typePermission")]
    public partial class TypePermission
    {
        public TypePermission()
        {
            Permission = new HashSet<Permission>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("description")]
        [StringLength(30)]
        public string Description { get; set; }

        public virtual ICollection<Permission> Permission { get; set; }
    }
}
