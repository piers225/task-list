import React, { useState } from 'react'
import './login.css';
import { useNavigate } from 'react-router-dom';
import useApi from '../../api/useApi';

export default function Login() {
    const [email, setEmail] = useState('test')
    const [password, setPassword] = useState('test')
    const [invalidCredential, setInvalidCredential] = useState(false)

    const navigate = useNavigate()
    const { client, setAuthApi } = useApi()

    const login = async() => {
        const response = await client.api.authenticationLoginCreate({
            email,
            password
        }).catch(e => { 
            console.error(e)
            setInvalidCredential(true)
        })
        if (response?.data?.key) {
            setAuthApi(response.data.key);
            navigate('/task-list')
        }
    }

    return <div className="login-container">
        <div className="login-form">
            <h1>Login</h1>
            { invalidCredential && <p className='warning'>Incorrect user name or password</p> }
            <div className="input-container">
                <label htmlFor="email">Email</label>
                <input type="text" id="email" name="email" value={email} onChange={(e) => setEmail(e.target.value)} required />
            </div>
            <div className="input-container">
                <label htmlFor="password">Password</label>
                <input type="password" id="password" name="password" value={password} onChange={(e) => setPassword(e.target.value)} required />
            </div>
            <button type="submit" onClick={login}>Log In</button>
        </div>
    </div>
}