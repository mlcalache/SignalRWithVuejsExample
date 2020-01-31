using SignalRExample.API.Models;
using System;
using System.Threading.Tasks;

namespace SignalRExample.API.Hubs
{
    public interface IQuestionHub
    {
        Task NewQuestionAdd(Question question);
        //Task NewQuestionAdd(Guid questionId);

        Task QuestionScoreChange(Guid questionId, int score);
    }
}