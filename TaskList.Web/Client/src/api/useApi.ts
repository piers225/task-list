import { useMemo, useRef } from "react"
import { Api } from "./Api"

const useApi = () => {

    const key = useRef(window.localStorage.getItem("api-key"))

    const client = useMemo(() => {
        return new Api({ baseApiParams : { 
            headers : {
                'Authorization' : `Bearer ${key?.current}`
            }
        }})
    }, [key.current])

    return {
        client : client,
        setAuthApi : (newKey : string) => {
            key.current = newKey;
            window.localStorage.setItem("api-key", newKey)
        }
    }
}

export default useApi