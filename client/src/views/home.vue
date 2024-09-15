<template>
  <div>
    <h1>
      FAQ
      <button v-b-modal.addQuestionModal class="btn btn-primary mt-2 float-right" >
        <i class="fas fa-plus" /> Ask a question
      </button>
    </h1>
    <ul class="list-group question-previews mt-4">
      <question-preview
        v-for="question in questions"
        :key="question.id"
        :question="question"
        class="list-group-item list-group-item-action mb-3" />
    </ul>
    <add-question-modal @question-added="onQuestionAdded" />
  </div>
</template>
 
<script>
import QuestionPreview from "@/components/question-preview";
import AddQuestionModal from "@/components/add-question-modal";
import bus from "@/bus";

export default {
  components: {
    QuestionPreview,
    AddQuestionModal   
  },
  data() {
    return {
      questions: []
    };
  },
  created() {
    this.$http.get("https://localhost:5001/question").then(res => {
      this.questions = res.data;
    });

    // Listen to question hub coming from SignalR events
    this.$questionHub.$on("question-added", this.onQuestionAdded);
    this.$questionHub.$on("answer-added", this.onAnswerAddedToQuestion);
    this.$questionHub.$on("answer-removed", this.onAnswerRemovedFromQuestion);
    this.$questionHub.$on('question-removed', this.onQuestionRemoved);

    bus.$on('a', this.onQuestionRemoved);

    // Listen to inventory hub from SignalR events
    // this.$inventoryHub.$on("new-inventory-update-added", this.onNewInventoryUpdateAdded);    
  },
  methods: {
    onQuestionAdded(question) {
      question.answerCount = 0;
      if (!this.questions.find(item => question.id == item.id))
      {
        this.questions = [question, ...this.questions];
      }
    },
    onAnswerAddedToQuestion(question) {
      var q = this.questions.find(item => question.id == item.id);
      q.answerCount++;
    },
    onAnswerRemovedFromQuestion(answer) {
      var q = this.questions.find(item => answer.questionId == item.id);
      q.answerCount--;
    },
    onNewInventoryUpdateAdded(inventoryUpdates) {
      console.log(inventoryUpdates);
      alert('Inventory updated');
    },
    onQuestionRemoved (question) {
     
     var removedQuestion = this.questions.find(a => a.id == question.id);
      if (removedQuestion) {
        var index = this.questions.indexOf(removedQuestion);
        this.questions.splice(index, 1);
      }    
    },
    beforeDestroy() {
      // Make sure to cleanup SignalR event handlers when removing the component
      this.$questionHub.$off("question-added", this.onQuestionAdded);
      this.$questionHub.$off("answer-added", this.onAnswerAddedToQuestion);
      this.$questionHub.$off("answer-removed",this.onAnswerRemovedFromQuestion);
      this.$questionHub.$off('question-removed', this.onQuestionRemoved);
      
      // this.$inventoryHub.$off("new-inventory-update-added", this.onNewInventoryUpdateAdded);    
    }
  }
};
</script>
 
<style>
.question-previews .list-group-item {
  cursor: pointer;
}
</style>