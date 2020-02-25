import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'

export default {
    install(Vue) {
        const connection = new HubConnectionBuilder()
            .withUrl(`${Vue.prototype.$http.defaults.baseURL}/question-hub`)
            .configureLogging(LogLevel.Information)
            .build()

        // use new Vue instance as an event bus
        const questionHub = new Vue()

        // every component will use this.$questionHub to access the event bus
        Vue.prototype.$questionHub = questionHub

        // Forward server side SignalR events through $questionHub, where components will listen to them
        connection.on('QuestionScoreChanged', (questionId, score) => {
            questionHub.$emit('score-changed', { questionId, score })
        })

        connection.on('NewQuestionAdded', (question) => {
            questionHub.$emit('question-added', question)
        })

        connection.on('NewAnswerAdded', (question) => {
            questionHub.$emit('answer-added', question)
        })

        connection.on('AnswerRemovedFromQuestion', (question) => {
            questionHub.$emit('answer-removed', question)
        })
        
        connection.on('QuestionRemoved', (question) => {
            questionHub.$emit('question-removed', question)
        })

        let startedPromise = null

        function start() {
            startedPromise = connection.start().catch(err => {
                console.error('Failed to connect with hub', err)
                // return new Promise((resolve, reject) => setTimeout(() => start().then(resolve).catch(reject), 5000))
            })
            return startedPromise
        }
        connection.onclose(() => start())

        start()
    }
}