using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entity
{
    public class LabelEntity
    {
        public int LabelId { get; set; }
        public String LabelName { get; set; }
        public int NoteId { get; set; }
        public int UserId { get; set; }
    }
}
