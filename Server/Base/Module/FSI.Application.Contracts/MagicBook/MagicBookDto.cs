using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace FSI.Application.Contracts.MagicBook
{
    public class MagicBookDto : Entity<Guid>
    {
        public string BookName { get; set; }

        public List<MagicPageDto> Pages { get; set; }

        public string Tag { get; set; }

        public string ImageCover { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }
    }
}
