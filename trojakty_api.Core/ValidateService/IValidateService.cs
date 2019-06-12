using System.Collections.Generic;
using System.Threading.Tasks;
using trojakty_api.Core.ValidateService.DTOs;
using trojkaty_api.DataAccess.Models;

namespace trojakty_api.Core.ValidateService
{
    public interface IValidateService
    {
        Task<List<ValidatedQuestionDTO>> GetAllAsync();
        Task<ValidateQuestion> ConfirmValidationAsync(Question question, bool publish);
    }

}