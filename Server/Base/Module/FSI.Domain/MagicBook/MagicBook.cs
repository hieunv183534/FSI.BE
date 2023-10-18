using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace FSI.Domain.MagicBook
{
    public class MagicBook : Entity<Guid>
    {
        public string BookName { get; set; }

        public List<MagicPage> Pages { get; set; }

        public string Tag { get; set; }

        public string ImageCover { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }
    }
}
