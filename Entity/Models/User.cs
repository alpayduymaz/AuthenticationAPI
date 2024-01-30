using Entity.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    /// <summary>
    /// Tüm kullanıcıların kaydının tutulduğu tablodur.
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// Kayıt tekil bilgisidir.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Sicil tekil bilgisidir.
        /// </summary>
        public int? SicilId { get; set; }

        /// <summary>
        /// Kullanıcı adı.
        /// </summary>
        [Required, MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Kullanıcı soyadı.
        /// </summary>
        [Required, MaxLength(150)]
        public string Surname { get; set; }

        /// <summary>
        /// Kullanıcı adı soyadı
        /// </summary>
        [Required, MaxLength(250)]
        public string FullName { get; set; }

        /// <summary>
        /// Kullanıcı fotoğrafıdır.
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// Kullanıcı E-Posta adresi.
        /// </summary>
        [Index]
        [Required, MaxLength(50), EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Kullanıcı parolası.
        /// </summary>
        [MaxLength(150)]
        public string Password { get; set; }

        /// <summary>
        /// Çalışma yılı.
        /// </summary>
        public int WorkYear { get; set; }

        /// <summary>
        /// Var olan sicildir.
        /// </summary>
        [InverseProperty("CreatedUser")]
        [NotMapped]
        public ICollection<User> CreateUsers { get; set; }

        /// <summary>
        /// Sicil güncelleme kaydıdır.
        /// </summary>
        [InverseProperty("LastUpdatedUser")]
        [NotMapped]
        public ICollection<User> UpdateUsers { get; set; }
    }
}
