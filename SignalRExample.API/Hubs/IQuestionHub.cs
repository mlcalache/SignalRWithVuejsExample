using SignalRExample.API.Models;
using System;
using System.Threading.Tasks;

namespace SignalRExample.API.Hubs
{
    public interface IQuestionHub
    {
        Task NewQuestionAdded(Question question);

        Task NewAnswerAdded(Question question);

        Task AnswerRemovedFromQuestion(Answer answer);

        Task QuestionScoreChanged(Guid questionId, int score);

        Task QuestionRemoved(Question question);
    }
}