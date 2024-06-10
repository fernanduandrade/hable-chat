export interface Guild {
    name: string
    id: number
}

export interface Channel {
    name: string
    id: number
}

export interface User {
    name: string
    id: string
    userName: string
    lastName: string
}

export interface Message {
    user: any
    id: number
    content: string
    created: string
}