using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence;

namespace Application.Helpers
{
    public class GetImagesPathHelper : IPhotosUrl
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;

        public GetImagesPathHelper(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        
        public async Task<List<object>> GetImagesPath(int questionId)
        {
            
            
            var apiUrl = _configuration["ApiUrl"];
            var result = new List<object>();

            var images = await _context.Images
                .Where(x => x.Question.Id == questionId)
                .Select(x => new {x.Name, x.Url})
                .ToListAsync();

            foreach (var doc in images)
            {
                var url = apiUrl + $"questions/{questionId}/document/{doc.Url}";

                result.Add(new
                {
                    doc.Name,
                    url
                });
            }

            return result;
        }
    }
}