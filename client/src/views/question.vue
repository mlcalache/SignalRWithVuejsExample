<template>
  <article class="container" v-if="question">
    <header class="row align-items-center">
      <question-score :question="question" class="col-1" />
      <h3 class="col-11">{{ question.title }}</h3>
    </header>
    <p class="row offset-1 col-11">{{ question.body }}</p>
    <ul class="list-unstyled row" v-if="hasAnswers">
      <li v-for="answer in question.answers" :key="answer.id" class="offset-1 col-11 border-top py-2">
        {{ answer.id }} - {{ answer.body }}
        <button v-b-modal.removeAnswer class="btn btn-danger mt-2 float-right" @click="onAnswerRemoved(answer.id)">
          <i class="fas fa-trash-alt" /> Remove answer
        </button>
      </li>
    </ul>
    <footer>
      <button class="btn btn-primary float-right" v-b-modal.addAnswerModal>
        <i class="fas fa-edit" /> Post your Answer
      </button>
      <button class="btn btn-link float-right" @click="onReturnHome">Back to list</button>
    </footer>
    <add-answer-modal :question-id="this.questionId" @answer-added="onAnswerAdded" />
     
  </article>
</template>
 
<script>
import QuestionScore from "@/components/question-score";
import AddAnswerModal from "@/components/add-answer-modal";


export default {
  components: {
    QuestionScore,
    AddAnswerModal
    
  },
  data() {
    return {
      question: null,
      answers: [],
      questionId: this.$route.params.id
    };
  },
  computed: {
    hasAnswers() {
      return this.question.answers.length > 0;
    }
  },
  created() {
    this.$http.get(`https://localhost:5001/question/${this.questionId}`).then(res => {
        this.question = res.data;
      });

    // Listen to question add coming from SignalR events
    this.$questionHub.$on("answer-added", this.onAnswerAddedToQuestion);
    this.$questionHub.$on("answer-removed", this.onAnswerRemovedFromQuestion);
     
  },
  methods: {
    onReturnHome() {
      this.$router.push({ name: "Home" });
    },
    onAnswerAdded(answer) {
      if (!this.question.answers.find(a => a.id === answer.id)) {
        this.question.answers.push(answer);
      }
    },
    onAnswerAddedToQuestion(question) {
      if(this.question.id == question.id){
        if (this.question.answers.length != question.answers.length)
          this.question.answers = question.answers;
        }
    },
    onAnswerRemoved(answerId) {
      this.$http.delete(`https://localhost:5001/question/${this.questionId}/answer/${answerId}`).then(res => {
          var removedAnswer = this.question.answers.find(a => a.id == res.data.id);
          if (removedAnswer) {
            var index = this.question.answers.indexOf(removedAnswer);
            this.question.answers.splice(index, 1);
          }
        });
    },
    onAnswerRemovedFromQuestion(answer) {
      var removedAnswer = this.question.answers.find(a => a.id == answer.id);
      if (removedAnswer) {
        var index = this.question.answers.indexOf(removedAnswer);
        this.question.answers.splice(index, 1);
      }
    },
    beforeDestroy() {
      // Make sure to cleanup SignalR event handlers when removing the component
      this.$questionHub.$off("answer-added", this.onAnswerAddedToQuestion)
      this.$questionHub.$off("answer-removed",this.onAnswerRemovedFromQuestion)
      
    }
  }
};
</script>