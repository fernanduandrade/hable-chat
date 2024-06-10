import { createApp } from 'vue'
import App from './App.vue'
import routes from './routes/router'
import { createPinia } from 'pinia'
import { library } from '@fortawesome/fontawesome-svg-core'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import {
    faGreaterThan,
    faSpinner,
    faXmark,
    faArrowRightToBracket
} from '@fortawesome/free-solid-svg-icons'

library.add(faGreaterThan, faSpinner, faArrowRightToBracket, faXmark)

const pinia = createPinia()

createApp(App)
    .use(routes)
    .use(pinia)
    .component('font-awesome-icon', FontAwesomeIcon)
    .mount('#app')
