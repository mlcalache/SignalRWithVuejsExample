using Microsoft.AspNet.SignalR;
using SignalRExample.NetFramework.API.Models;
using System;
using System.Threading.Tasks;

namespace SignalRExample.NetFramework.API.Hubs
{
    public class QuestionHub : Hub, IQuestionHub
    {
        public async Task AnswerRemovedFromQuestion(Answer answer)
        {
            await Clients.All.AnswerRemovedFromQuestion(answer);
        }

        public async Task NewAnswerAdded(Question question)
        {
            await Clients.All.NewAnswerAdded(question);
        }

        public async Task NewQuestionAdded(Question question)
        {
            await Clients.All.NewQuestionAdded(question);
        }

        public async Task QuestionScoreChanged(Guid questionId, int score)
        {
            await Clients.All.QuestionScoreChanged(questionId, score);
        }
    }
}