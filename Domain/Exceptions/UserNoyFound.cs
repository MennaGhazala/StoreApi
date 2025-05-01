using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class UserNoyFound(string email)
        : NotFoundException($" User With Email :{email}  not Found")
    {

    }
}
