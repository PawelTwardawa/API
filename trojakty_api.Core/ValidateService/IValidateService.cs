using System.Collections.Generic;
using System.Threading.Tasks;
using trojkaty_api.DataAccess.Models;

namespace trojakty_api.Core.ValidateService
{
    public interface IValidateService
    {
        Task<List<Question>> GetAll();
        Task<ValidateQuestion> ConfirmValidation(Question question, bool publish);
        //ValidateQuestion ConfirmValidation(Question question, bool publish);
    }
}