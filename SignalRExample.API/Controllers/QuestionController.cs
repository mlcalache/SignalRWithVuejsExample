using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRExample.API.Hubs;
using SignalRExample.API.Models;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRExample.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        //private static ConcurrentBag<Question> _questions = new ConcurrentBag<Question> {
        //    new Question {
        //        Id = Guid.Parse("b00c58c0-df00-49ac-ae85-0a135f75e01b"),
        //        Title = "Welcome",
        //        Body = "Welcome to the _mini Stack Overflow_ rip-off!\nThis will help showcasing **SignalR** and its integration with **Vue**",
        //        Answers = new List<Answer>{ new Answer { Body = "Sample answer" }}
        //    }
        //};

        private static ConcurrentDictionary<Guid,Question> _questions = new ConcurrentDictionary<Guid,Question>();

        private readonly IHubContext<QuestionHub, IQuestionHub> _hubContext;

        public QuestionController(IHubContext<QuestionHub, IQuestionHub> questionHub)
        {
            this._hubContext = questionHub;
        }

        [HttpGet()]
        public IEnumerable GetQuestions()
        {
            return _questions.Select(q => new 
            {
                Id = q.Value.Id,
                Title = q.Value.Title,
                Body = q.Value.Body,
                Score = q.Value.Score,
                AnswerCount = q.Value.Answers.Count
            });
        }

        [HttpGet("{id}")]
        public ActionResult GetQuestion(Guid id)
        {
            var question = _questions.SingleOrDefault(t => t.Key == id);
            if (question.Value == null) return NotFound();

            return new JsonResult(question.Value);
        }

        [HttpPost()]
        public async Task<ActionResult<Question>> AddQuestion([FromBody]Question question)
        {
            question.Id = Guid.NewGuid();
            question.Answers = new List<Answer>();
            _questions.TryAdd(question.Id ,question);

            await this._hubContext
                .Clients
                .All
                .NewQuestionAdded(question);

            return question;
        }

        [HttpPost("{id}/answer")]
        public async Task<ActionResult> AddAnswerAsync(Guid id, [FromBody]Answer answer)
        {
            var question = _questions.SingleOrDefault(t => t.Key == id);
            if (question.Value == null) return NotFound();

            answer.Id = Guid.NewGuid();
            answer.QuestionId = id;
            question.Value.Answers.Add(answer);

            await this._hubContext
                .Clients
                .All
                .NewAnswerAdded(question.Value);

            return new JsonResult(answer);
        }

        [HttpDelete("{id}/answer/{answerId}")]
        public async Task<ActionResult> RemoveAnswerAsync(Guid id, Guid answerId)
        {
            var question = _questions.SingleOrDefault(t => t.Key == id);
            if (question.Value == null) return NotFound();

            var answer = question.Value.Answers.Find(x => x.Id == answerId);

            question.Value.Answers.Remove(answer);

            await this._hubContext
                .Clients
                .All
                .AnswerRemovedFromQuestion(answer);

            return new JsonResult(answer);
        }

        [HttpPatch("{id}/upvote")]
        public async Task<ActionResult> UpvoteQuestionAsync(Guid id)
        {
            var question = _questions.SingleOrDefault(t => t.Key == id);
            if (question.Value == null) return NotFound();

            // Warning, this increment isnt thread-safe! Use Interlocked methods
            question.Value.Score++;

            await this._hubContext
                .Clients
                .All
                .QuestionScoreChanged(question.Key, question.Value.Score);

            return new JsonResult(question.Value);
        }

        [HttpPatch("{id}/downvote")]
        public async Task<ActionResult> DownvoteQuestionAsync(Guid id)
        {
            var question = _questions.SingleOrDefault(t => t.Value.Id == id);
            if (question.Value == null) return NotFound();

            // Warning, this increment isnt thread-safe! Use Interlocked methods
            question.Value.Score--;

            await this._hubContext
                .Clients
                .All
                .QuestionScoreChanged(question.Key, question.Value.Score);

            return new JsonResult(question.Value);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> QuestionRemoved(Guid id)
        {
            var question = _questions.SingleOrDefault(t => t.Key == id);
            
            if (question.Value == null) return NotFound();

            // Warning, this increment isnt thread-safe! Use Interlocked methods
            Question test;
            _questions.TryRemove(id,out test);
            
                 await this._hubContext
                .Clients
                .All
                .QuestionRemoved(question.Value);

            

           
            return new JsonResult(question);
        }
    }
}