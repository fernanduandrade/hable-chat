import { defineStore } from 'pinia'
import { Channel, Guild, Message } from '../types'
import { HubConnection } from '@microsoft/signalr'

interface IApplicationSate {
    messages: Message[],
    guilds: Guild[],
    channels: Channel[],
    selectedGuild: number,
    selectedChannel: number,
    hubConnection: HubConnection | null,
    channelName: string
    user: any
}

const basicState = {
    messages: [],
    channels: [],
    guilds: [],
    selectedGuild: 0,
    selectedChannel: 0,
    channelName: '',
    hubConnection: null,
    user: null
}

export default defineStore('application-store', {
    state: (): IApplicationSate => (basicState),
    actions: {
        setMessages(values: Message[]) {
            this.messages = values
        },
        setGuilds(values: Guild[]) {
            this.guilds = values
        },
        setChannel(values: Channel[]) {
            this.channels = values
        },
        setGuildId(id: number) {
            this.selectedGuild = id
        },
        setChannelId(id: number) {
            this.selectedChannel = id
        },
        setConnection(conn: HubConnection) {
            this.hubConnection = conn
        },
        setChannelName(name: string) {
            this.channelName = name
        },
        setUser(user: any) {
            this.user = user
        }
    },
  })