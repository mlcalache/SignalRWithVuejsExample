using SignalRExample.NetFramework.API.Models;
using System;
using System.Threading.Tasks;

namespace SignalRExample.NetFramework.API.Hubs
{
    public interface IQuestionHub
    {
        Task NewQuestionAdded(Question question);

        Task NewAnswerAdded(Question question);

        Task AnswerRemovedFromQuestion(Answer answer);

        Task QuestionScoreChanged(Guid questionId, int score);
    }
}