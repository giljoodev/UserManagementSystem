using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UserManagementSystem.Database.Entity
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int    Index        { get; set; }
        [Column(Order = 1, TypeName = "NVARCHAR(20)")]
        public string Name         { get; set; }
        [Column(Order = 2)]
        public short  Age          { get; set; }
        [Column(Order = 3, TypeName = "NVARCHAR(12)")]
        public string PhoneNumber  { get; set; }
        [Column(Order = 4)]
        public bool IsInit         { get; set; } = false;
    }
}
