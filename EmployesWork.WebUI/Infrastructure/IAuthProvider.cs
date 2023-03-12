using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployesWork.WebUI.Infrastructure
{
    public interface IAuthProvider
    {
        
            bool Authenticate(string username, string password);
        
    }
}
