using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.DTOs.Article;
using Entities_Context;
using Entities_Context.Entities.UserNews;
using IServices;
using IServices.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Services.Article
{

    public class SourceService:ISourceService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public SourceService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<String> GetSourceNameByIdAsync(Int32 Id)
        {
            if (Id < 1)
            {
                throw new ArgumentException(nameof(Id));
            }

            Source? source = await _unitOfWork.Source
                .GetByIdAsync(Id);
            
            if (source is not null)
            {
                return source.Name;
            }

            Log.Warning("unable to determine source {0}",Id);
            
            return "Неизвестен";
        }

        public async Task<List<SourceDto>> GetAllSourcesDtoAsync()
        {
            List<SourceDto>? allSources = await _unitOfWork.Source
                .GetAsQueryable()
                .Where(x=>!String.IsNullOrEmpty(x.RssFeedUrl))
                .ProjectTo<SourceDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return allSources;
        }
    }
}
