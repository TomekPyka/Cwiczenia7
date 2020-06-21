using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenia7.Model
{
    public class RefreshToken
    {
        public string Id { get; set; }
        public string IndexNumber { get; set; }
        public virtual Student IndexNumberNavigation { get; set; }
    }
}
