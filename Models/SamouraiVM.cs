using BO;
using System.Collections.Generic;

namespace Dojo.Models
{
    public class SamouraiVM
    {
        public Samourai Samourai { get; set; }
        public List<Arme> Armes { get; set; }
        public List<ArtMartial> ArtMartials { get; set; }
        public int? IdSelectedArme { get; set; }
        public List<int> IdsSelectedArtMartiaux { get; set; }
    }
}