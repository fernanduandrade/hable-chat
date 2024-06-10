import { defineStore } from 'pinia'
import { extend } from '@vue/shared'

const component = extend({})

type VueComponent = InstanceType<typeof component>

type IModalProps = {
  component: null | VueComponent
  props?: object
  title?: string
  description?: string
  opened?: boolean
  subscribe?: string

}

interface IModalState {
  modalState: IModalProps
  modalEmitValue: unknown
}

const basicState = { component: null, subscribe: '', title: '', description: '', props: { }, opened: false }

export default defineStore('modal-store', {
  state: (): IModalState => ({ modalState: basicState, modalEmitValue: null }),
  actions: {
    open(payload: IModalProps) {
      const { props, component, title, description, subscribe } = payload
      this.modalState = { component, props: props || {}, title, subscribe, description, opened: true }
    },
    async close() {
      const { subscribe } = this.modalState
      if (subscribe)
        this.modalEmitValue = subscribe

      this.modalState = basicState
    },
    setFormValue(value: unknown) {
      this.modalEmitValue = value
    },
  },
})