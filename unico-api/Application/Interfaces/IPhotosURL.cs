using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPhotosUrl
    {
         Task<List<object>> GetImagesPath(int questionId);
    }
}