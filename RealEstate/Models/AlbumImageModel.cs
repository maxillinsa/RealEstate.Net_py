using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Models;

namespace RealEstate.Models
{
   public class AlbumImageView
        {
            [Key]
            public Album album { get; set ;}
          
            public virtual IList<Image> Images { get; set; }
          
        }

}
