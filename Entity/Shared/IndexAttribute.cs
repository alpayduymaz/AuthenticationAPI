using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Shared
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IndexAttribute : Attribute
    {
        /// <summary>
        /// Bulunduğu tekil tabloda bir index değeri oluşturur.
        /// </summary>
        public bool IsUnique { get; set; }
    }
}
