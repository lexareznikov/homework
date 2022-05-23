using homework.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homework.Models
{
    public class FileViewModel
    {
        [Column(1)]
        public DateTime Date { get; set; }
        [Column(2)]
        public DateTime Time { get; set; }
        [Column(3)]
        public string Phone { get; set; }
        [Column(4)]
        public string Type { get; set; }
    }
}
