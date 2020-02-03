<template>
  <div>
    <h1>
      This totally looks like Stack Overflow
      <button
        v-b-modal.addQuestionModal
        class="btn btn-primary mt-2 float-right"
      >
        <i class="fas fa-plus" /> Ask a question
      </button>
    </h1>
    <ul class="list-group question-previews mt-4">
      <question-preview
        v-for="question in questions"
        :key="question.id"
        :question="question"
        class="list-group-item list-group-item-action mb-3"
      />
    </ul>
    <add-question-modal @question-added="onQuestionAdded" />
  </div>
</template>
 
<script>
import QuestionPreview from "@/components/question-preview";
import AddQuestionModal from "@/components/add-question-modal";

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

    // Listen to question add coming from SignalR events
    this.$questionHub.$on("question-added", this.onQuestionAdded);
    this.$questionHub.$on("answer-added", this.onAnswerAddedToQuestion);
    this.$questionHub.$on("answer-removed", this.onAnswerRemovedFromQuestion);
  },
  methods: {
    onQuestionAdded(question) {
      question.answerCount = 0;
      if (!this.questions.find(item => question.id == item.id))
        this.questions = [question, ...this.questions];
    },
    onAnswerAddedToQuestion(question) {
      var q = this.questions.find(item => question.id == item.id);
      q.answerCount++;
    },
    onAnswerRemovedFromQuestion(question) {
      var q = this.questions.find(item => question.id == item.id);
      q.answerCount--;
    },
    beforeDestroy() {
      // Make sure to cleanup SignalR event handlers when removing the component
      this.$questionHub.$off("question-added", this.onQuestionAdded);
      this.$questionHub.$off("answer-added", this.onAnswerAddedToQuestion);
      this.$questionHub.$off("answer-removed",this.onAnswerRemovedFromQuestion);
    }
  }
};
</script>
 
<style>
.question-previews .list-group-item {
  cursor: pointer;
}
</style>