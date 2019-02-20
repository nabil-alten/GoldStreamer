using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class EditProfileRepo <T> : BaseRepo<T> where T : class
    {
        GoldStreamerContext _context = null;
        public EditProfileRepo(GoldStreamerContext dbContext)
        {
            _context = dbContext;
        }
    }
}
