﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Notes
{
    public class NoteResponse
    {
        public int NoteId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Colour { get; set; }
        public bool IsArchived { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

       // public int UserId { get; set; }

    }
}
