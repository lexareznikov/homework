using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace homework.Models
{
    [Table("temp_table")]
    public class FileModel
    {
        [Column("id")]
        public long Id { get; set; }
        [Column("date")]
        public DateTime Date { get; set; }
        [Column("number")]
        public string Number { get; set; }
        [Column("type_id")]
        public int TypeId { get; set; }
    }
}

