using BussinesLayer.Interface;
using Repository.Entity;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer.Service
{
    public class LabelRepositorybl : Interface.ILabel
    {
        private readonly Repository.Interface.ILabel ilabel;
        public LabelRepositorybl(Repository.Interface.ILabel label)
        {
            this.ilabel = label;
        }
        public Task CreateLabel(LabelEntity labelEntity)
        {
            return ilabel.CreateLabel(labelEntity);
        }
        public Task DeleteLabel(int LabelId)
        {
            return ilabel.DeleteLabel(LabelId);
        }
        public Task UpdateLabel(LabelEntity labelEntity)
        {
            return ilabel.UpdateLabel(labelEntity); 
        }
        public Task<IEnumerable<LabelEntity>> GetAllLabelbyId(int LabelId)
        {
            return ilabel.GetAllLabelbyId(LabelId);
        }
        public Task<IEnumerable<object>> GetAllNotesbyId(int LabelId)
        {
            return ilabel.GetAllNotesbyId(LabelId);
        }
    }
}
