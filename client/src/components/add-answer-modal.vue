<template>
  <b-modal id="addAnswerModal" ref="addAnswerModal" hide-footer title="Add new Answer" @hidden="onHidden">
    <b-form @submit.prevent="onSubmit" @reset.prevent="onCancel">
      <b-form-group label="Your Answer:" label-for="answerInput">
        <b-form-textarea id="answerInput"
                      v-model="form.body"
                      placeholder="What do you need answered?"
                      :rows="6"
                      :max-rows="10">
        </b-form-textarea>
      </b-form-group>
 
      <button class="btn btn-primary float-right ml-2" type="submit" >Submit</button>
      <button class="btn btn-secondary float-right" type="reset">Cancel</button>
    </b-form>
  </b-modal>
</template>
 
<script>
export default {
  data () {
    return {
      form: {
        body: ''
      },      
      questionId: this.$route.params.id
    }
  },
  methods: {
    onSubmit () {
      this.$http.post(`https://localhost:44308/question/${this.questionId}/answer`, this.form).then(res => {
        this.$emit('answer-added', res.data)
        this.$refs.addAnswerModal.hide()
      })
    },
    onCancel () {
      this.$refs.addAnswerModal.hide()
    },
    onHidden () {
      Object.assign(this.form, {
        body: ''
      })
    }
  }
}
</script>