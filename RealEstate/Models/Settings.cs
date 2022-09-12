using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Models
{
    public static class Settings
    {
        public static List<HouseDirection> HouseDirections = new List<HouseDirection>
        {
            new HouseDirection{ Id = 1, Name = "Đông"},
            new HouseDirection{ Id = 2, Name = "Đông Bắc"},
            new HouseDirection{ Id = 3, Name = "Đông Nam"},
            new HouseDirection{ Id = 4, Name = "Tây"},
            new HouseDirection{ Id = 5, Name = "Tây Bắc"},
            new HouseDirection{ Id = 6, Name = "Tây Name"},
            new HouseDirection{ Id = 7, Name = "Nam"},
            new HouseDirection{ Id = 8, Name = "Bắc"}
        };
    }
}