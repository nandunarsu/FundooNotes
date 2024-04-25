using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Collaboration
{
    public class CollabInfoModel
    {
        public int CollaborationId { get; set; }
        public int UserId { get; set; }
        public int NoteId { get; set; }
        public string CollabEmail { get; set; }
    }
}
