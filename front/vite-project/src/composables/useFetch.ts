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
  console.log(sessionStorage.getItem('token'))
  const defaultOption: Options = {
    returnBlob: false,
    headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': `Bearer ${sessionStorage.getItem('token') ? JSON.parse(sessionStorage.getItem('token') || '') : ''}`
    },
    method: 'GET',
  }

  if (Object.entries(options).length > 0)
    options = { ...defaultOption, ...options }
  else
    options = { ...defaultOption }

  const fetchData = async() => {
    
    try {
      if (signal.aborted)
        return

      const response = await fetch(url, { signal, ...options })

      if(response.status == 401) {
        const newAccessToken = await refreshToken()
        options.headers!['Authorization'] = `Bearer ${newAccessToken}`;
        await fetchData()
      }
      data.value =  await response.json() as any
    }
    catch (exception: any) {
      error.value = exception.message
    }
  }

  return { data, error, abort, fetchData }
}

async function refreshToken():Promise<string> {
  const refreshToken = JSON.parse(sessionStorage.getItem('refreshToken') || '')
  const respose = await fetch('https://localhost:7266/api/auth/refresh', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({refreshToken})
    })


    const result = await respose.json()
    console.log(result)
    sessionStorage.removeItem('token')
    sessionStorage.removeItem('refreshToken')

    sessionStorage.setItem('token', JSON.stringify(result.accessToken))
    sessionStorage.setItem('refreshToken', JSON.stringify(result.refreshToken))
    return result.accessToken
}