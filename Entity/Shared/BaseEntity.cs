using Entity.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Shared
{
    public class BaseEntity
    {
        /// <summary>
        /// Kaydı oluşturan kişi kullanıcı tekil bilgisidir.
        /// </summary>
        public int? CreatedUserId { get; set; }

        /// <summary>
        /// Kaydı son güncelleyen kullanıcı tekil bilgisidir.
        /// </summary>
        public int? LastUpdatedUserId { get; set; }

        /// <summary>
        /// Kaydın aktiflik, pasiflik ve silinme durumlarının tutulduğu alandır.
        /// </summary>
        [Index]
        public DataStatus DataStatus { get; set; }

        /// <summary>
        /// Kaydın oluşturulma zaman bilgisidir.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Kaydın güncellenme zaman bilgisidir.
        /// </summary>
        public DateTime? LastUpdatedAt { get; set; }

        /// <summary>
        /// Kaydı oluştuna kişi bilgilerini döndürür.
        /// </summary>
        public User CreatedUser { get; set; }

        /// <summary>
        /// Kaydı son güncelleyen kişi bilgilerini döndürür.
        /// </summary>
        public User LastUpdatedUser { get; set; }
    }

    /// <summary>
    /// Veri işlem durumlarının enum değeridir.
    /// </summary>
    public enum DataStatus
    {
        /// <summary>
        /// Verinin silinme durumudur.
        /// </summary>
        Deleted = 1,

        /// <summary>
        /// Verinin aktiflik durumudur.
        /// </summary>
        Activated,

        /// <summary>
        /// Verinin pasiflik durumudur.
        /// </summary>
        DeActivated
    }
}

