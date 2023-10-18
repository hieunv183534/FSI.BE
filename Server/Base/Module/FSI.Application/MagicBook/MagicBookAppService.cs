using FSI.Application.Contracts.MagicBook;
using FSI.Domain.MagicBook;
using FSI.Domain.Project;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace FSI.Application.MagicBook
{
    [IgnoreAntiforgeryToken]
    public class MagicBookAppService : ApplicationService, IMagicBookAppService
    {
        private readonly IRepository<Domain.MagicBook.MagicBook, Guid> _repository;

        public MagicBookAppService(IRepository<Domain.MagicBook.MagicBook, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<MagicBookDto> AddOrUpdateAsync(MagicBookDto input)
        {
            var magicBook = await _repository.FindAsync(x => x.BookName == input.BookName);
            if (magicBook == null)
            {
                magicBook = await _repository.InsertAsync(ObjectMapper.Map<MagicBookDto, Domain.MagicBook.MagicBook>(input));
            }
            else
            {
                magicBook.Pages = ObjectMapper.Map<List<MagicPageDto>, List<MagicPage>>(input.Pages);
                await _repository.UpdateAsync(magicBook);
            }
            return ObjectMapper.Map<Domain.MagicBook.MagicBook, MagicBookDto>(magicBook);
        }

        public async Task DeleteBook(string bookName)
        {
            var book = await _repository.GetAsync(x=> x.BookName == bookName);
            await _repository.DeleteAsync(book);
        }

        public async Task<List<MagicBookDto>> GetListBook(string? filter = "")
        {
            var queryables = await _repository.GetQueryableAsync();
            var books = queryables.Select(x => new MagicBookDto()
            {
                BookName = x.BookName,
                Author = x.Author,
                ImageCover = x.ImageCover,
                Tag = x.Tag,
                Title = x.Title,
            });
            return books.ToList();
        }

        public async Task<MagicBookDto> GetMagicBookAsync(string name)
        {
            var magicBook = await _repository.GetAsync(x => x.BookName == name);
            return ObjectMapper.Map<Domain.MagicBook.MagicBook, MagicBookDto>(magicBook);
        }
    }
}
