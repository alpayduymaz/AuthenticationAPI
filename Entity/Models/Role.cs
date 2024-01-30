using Entity.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    /// <summary>
    /// Kullanıcı Rollerinin tutulduğu tablodur.
    /// </summary>
    public class Role : BaseEntity
    {
        /// <summary>
        /// Kullanıcı rolü tekil bilgisidir.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Rolü niteleyen isim bilgisidir.
        /// </summary>
        [Required, MaxLength(100)]
        public string Name { get; set; }
    }
}
