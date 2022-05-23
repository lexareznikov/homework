using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace homework.Models.Domain
{
    [Keyless]
    public class ReportModel
    {
        [Column("phone")]
        public string Phone { get; set; }
        [Column("date_call")]
        public DateTime DateCall { get; set; }
        [Column("date_difference")]
        public string DateDifference { get; set; }

    }
}
