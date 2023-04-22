using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstract;
using Entities_Context;
using Entities_Context.Entities.UserNews;
using IServices;
using Microsoft.EntityFrameworkCore;

namespace Services.Article
{

    public class SourceService:ISourceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SourceService(IUnitOfWork unitOfWork)
        {
            if (unitOfWork is null)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }

            _unitOfWork = unitOfWork;
        }

        public async Task<String> GetSourceNameByIdAsync(Int32 Id)
        {
            Source? Source = await _unitOfWork.Source.GetByIdAsync(Id);
            
            if (Source is not null)
            {
                return Source.Name;
            }

            return "Неизвестен";
        }
    }
}
