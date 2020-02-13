using Microsoft.AspNet.SignalR;
using SignalRExample.NetFramework.API.Hubs;
using SignalRExample.NetFramework.API.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace SignalRExample.NetFramework.API.Controllers
{
    [RoutePrefix("question")]
    public class QuestionController : ApiController
    {
        private static ConcurrentBag<Question> _questions = new ConcurrentBag<Question>();

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetQuestions()
        {
            return Ok(_questions);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetQuestion(Guid id)
        {
            var question = _questions.SingleOrDefault(t => t.Id == id);
            if (question == null) return NotFound();

            return Ok(question);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> AddQuestionAsync([FromBody]Question question)
        {
            question.Id = Guid.NewGuid();
            question.Answers = new List<Answer>();
            _questions.Add(question);

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<QuestionHub>();

            await hubContext
                .Clients
                .All
                .NewQuestionAdded(question);

            return Ok(question);
        }

        [HttpPost]
        [Route("{id}/answer")]
        public async Task<IHttpActionResult> AddAnswerAsync(Guid id, [FromBody]Answer answer)
        {
            var question = _questions.SingleOrDefault(t => t.Id == id);
            if (question == null) return NotFound();

            answer.Id = Guid.NewGuid();
            answer.QuestionId = id;
            question.Answers.Add(answer);

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<QuestionHub>();

            await hubContext
                .Clients
                .All
                .NewAnswerAdded(question);

            return Ok(answer);
        }

        [HttpDelete]
        [Route("{id}/answer/{answerId}")]
        public async Task<IHttpActionResult> RemoveAnswerAsync(Guid id, Guid answerId)
        {
            var question = _questions.SingleOrDefault(t => t.Id == id);
            if (question == null) return NotFound();

            var answer = question.Answers.Find(x => x.Id == answerId);

            question.Answers.Remove(answer);

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<QuestionHub>();

            await hubContext
                .Clients
                .All
                .AnswerRemovedFromQuestion(answer);

            return Ok(answer);
        }

        [HttpPatch]
        [Route("{id}/upvote")]
        public async Task<IHttpActionResult> UpvoteQuestionAsync(Guid id)
        {
            var question = _questions.SingleOrDefault(t => t.Id == id);
            if (question == null) return NotFound();

            // Warning, this increment isnt thread-safe! Use Interlocked methods
            question.Score++;

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<QuestionHub>();

            await hubContext
                .Clients
                .All
                .QuestionScoreChanged(question.Id, question.Score);

            return Ok(question);
        }

        [HttpPatch]
        [Route("{id}/downvote")]
        public async Task<IHttpActionResult> DownvoteQuestionAsync(Guid id)
        {
            var question = _questions.SingleOrDefault(t => t.Id == id);
            if (question == null) return NotFound();

            // Warning, this increment isnt thread-safe! Use Interlocked methods
            question.Score--;

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<QuestionHub>();

            await hubContext
                .Clients
                .All
                .QuestionScoreChanged(question.Id, question.Score);

            return Ok(question);
        }
    }
}