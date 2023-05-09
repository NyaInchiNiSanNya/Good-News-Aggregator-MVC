using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstract;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.DTOs.Article;
using Entities_Context;
using Entities_Context.Entities.UserNews;
using IServices;
using Microsoft.EntityFrameworkCore;

namespace Services.Article
{

    public class SourceService:ISourceService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _Mapper;

        public SourceService(IUnitOfWork unitOfWork,IMapper Mapper)
        {
            if (unitOfWork is null)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }

            _unitOfWork = unitOfWork;
            
            if (Mapper is null)
            {
                throw new ArgumentNullException(nameof(Mapper));
            }

            _Mapper = Mapper;
        }

        public async Task<String> GetSourceNameByIdAsync(Int32 Id)
        {
            Source? Source = await _unitOfWork.Source
                .GetByIdAsync(Id);
            
            if (Source is not null)
            {
                return Source.Name;
            }

            return "Неизвестен";
        }

        public async Task<List<SourceDTO>> GetAllSourcesDTOAsync()
        {
            List<SourceDTO>? allSources = await _unitOfWork.Source
                .GetAsQueryable()
                .Where(x=>!String.IsNullOrEmpty(x.RssFeedUrl))
                .ProjectTo<SourceDTO>(_Mapper.ConfigurationProvider)
                .ToListAsync();

            if (allSources is  null)
            {
                return null;
            }

            return allSources;
        }
    }
}
