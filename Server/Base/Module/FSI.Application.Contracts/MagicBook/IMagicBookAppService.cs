using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.MagicBook
{
    public interface IMagicBookAppService
    {
        Task<MagicBookDto> GetMagicBookAsync(string name);

        Task<List<MagicBookDto>> GetListBook(string filter);

        Task<MagicBookDto> AddOrUpdateAsync(MagicBookDto input);

        Task DeleteBook(string bookName);
    }
}
