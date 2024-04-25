using ModelLayer.Label;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer.Interface
{
    public interface ILabel
    {
        public Task CreateLabel(CreateLabel label, int UserId);
        public Task DeleteLabel(int LabelId);
        public Task UpdateLabel(CreateLabel label, int LabelId, int UserId);
        public Task<IEnumerable<LabelEntity>> GetAllLabelbyId();
        public Task<IEnumerable<object>> GetAllNotesbyId(int LabelId);
    }
}
