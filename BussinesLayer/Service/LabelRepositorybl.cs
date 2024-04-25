using BussinesLayer.Interface;
using ModelLayer.Label;
using Repository.Entity;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer.Service
{
    public class LabelRepositorybl : ILabel
    {
        private readonly ILabelRL ilabel;
        public LabelRepositorybl(Repository.Interface.ILabelRL label)
        {
            this.ilabel = label;
        }
        public Task CreateLabel(CreateLabel label, int UserId)
        {
            return ilabel.CreateLabel(label,UserId);
        }
        public Task DeleteLabel(int LabelId)
        {
            return ilabel.DeleteLabel(LabelId);
        }
        public Task UpdateLabel(CreateLabel label, int LabelId, int UserId)
        {
            return ilabel.UpdateLabel(label,LabelId,UserId);
        }
        public Task<IEnumerable<LabelEntity>> GetAllLabelbyId()
        {
            return ilabel.GetAllLabelbyId();
        }
        public Task<IEnumerable<object>> GetAllNotesbyId(int LabelId)
        {
            return ilabel.GetAllNotesbyId(LabelId);
        }
    }
}
