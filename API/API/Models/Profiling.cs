using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("TB_M_Profiling")]
    public class Profiling
    {
        [Key]
        public string NIK { get; set; }
        [JsonIgnore]
        public virtual Education Education { get; set; }
        [JsonIgnore]
        public virtual Account Account { get; set; }
        [ForeignKey("Education")]
        [JsonIgnore]
        public int Education_Id { get; set; }
    }
}
