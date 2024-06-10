import { ref } from 'vue'

type HttpRequest = 'POST' | 'DELETE' | 'GET' | 'PUT' | 'PATCH'

interface Options {
  headers?: Record<string, string>
  method?: HttpRequest
  returnBlob?: boolean
  body?: string
}

export default function useFetch<TResult>(url: string, options: Options = {}) {
  const data = ref<TResult>()
  const error = ref('')

  const { signal, abort } = new AbortController()

  const fetchData = async() => {
    const defaultOption: Options = {
      returnBlob: false,
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
      },
      method: 'GET',
    }

    if (Object.entries(options).length > 0)
      options = { ...defaultOption, ...options }
    else
      options = { ...defaultOption }

    try {
      if (signal.aborted)
        return

      const response = await fetch(url, { signal, ...options })

      data.value =  await response.json()
    }
    catch (exception: any) {
      error.value = exception.message
    }
  }

  return { data, error, abort, fetchData }
}