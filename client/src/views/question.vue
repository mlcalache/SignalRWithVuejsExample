<template>
  <article class="container" v-if="question">
    <header class="row align-items-center">
      <question-score :question="question" class="col-1" />
      <h3 class="col-11">{{ question.title }}</h3>
    </header>
    <p class="row offset-1 col-11">
      {{ question.body }}
    </p>
    <ul class="list-unstyled row" v-if="hasAnswers">
      <li v-for="answer in question.answers" :key="answer.id" class="offset-1 col-11 border-top py-2">
        {{ answer.body }}
      </li>
    </ul>
    <footer>
      <button class="btn btn-primary float-right" v-b-modal.addAnswerModal><i class="fas fa-edit"/> Post your Answer</button>
      <button class="btn btn-link float-right" @click="onReturnHome">Back to list</button>
    </footer>
    <add-answer-modal :question-id="this.questionId" @answer-added="onAnswerAdded"/>
  </article>
</template>
 
<script>
import QuestionScore from '@/components/question-score'
import AddAnswerModal from '@/components/add-answer-modal'
 
export default {
  components: {
    QuestionScore,
    AddAnswerModal
  },
  data () {
    return {
      question: null,
      answers: [],
      questionId: this.$route.params.id
    }
  },
  computed: {
    hasAnswers () {
      return this.question.answers.length > 0
    }
  },
  created () {
    this.$http.get(`https://localhost:44308/question/${this.questionId}`).then(res => {
      this.question = res.data
    })
  },
  methods: {
    onReturnHome () {
      this.$router.push({name: 'Home'})
    }, 
    onAnswerAdded (answer) {
      if (!this.question.answers.find(a => a.id === answer.id)) {
        this.question.answers.push(answer)
      }
    }
  }
}
</script>