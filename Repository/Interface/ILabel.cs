using ModelLayer.Notes;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface ILabel
    {
        public Task CreateLabel(LabelEntity labelEntity);
        public Task DeleteLabel(int LabelId);
        public Task UpdateLabel(LabelEntity labelEntity);
        public Task<IEnumerable<LabelEntity>> GetAllLabelbyId(int LabelId);
        public Task<IEnumerable<object>> GetAllNotesbyId(int LabelId);

    }
}
