using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VLKAssignement.DataAccess.Models
{
    [Table("Users")]
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid();
        }
        [Key]
        public Guid Id { get; private set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }
    }
}
