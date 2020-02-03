using SignalRExample.API.Models;
using System;
using System.Threading.Tasks;

namespace SignalRExample.API.Hubs
{
    public interface IQuestionHub
    {
        Task NewQuestionAdded(Question question);

        Task NewAnswerAdded(Question question);

        Task AnswerRemovedFromQuestion(Question question);

        Task QuestionScoreChanged(Guid questionId, int score);
    }
}