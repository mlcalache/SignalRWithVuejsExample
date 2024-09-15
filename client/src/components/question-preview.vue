<template>
  <li class="card container" >
    <div class="card-body row">
      <question-score :question="question" class="col-1" />
      <div class="col-11">
        <h5 class="card-title">{{ question.title }}</h5>
        <p>
          {{question.body}}</p>
        <a href="#" class="card-link" @click="onOpenQuestion">
          View question <span class="badge badge-success">{{ question.answerCount }}</span>
        </a>  
       <question-remove :question="question" class="col-1"  />
    
      </div>
    
    </div>
  </li>
  
</template>
 
<script>
import QuestionScore from '@/components/question-score'
import QuestionRemove from '@/components/question-remove'
export default {
  components: {
    QuestionScore,
    QuestionRemove
  },
  props: {
    question: {
      type: Object,
      required: true
    }
  },
  methods: {
    onOpenQuestion () {
      this.$router.push({name: 'Question', params: {id: this.question.id}})
    }
  }
}
</script>