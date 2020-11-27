import React, { useState } from 'react'
import useClient from '../hooks/useClient'
import { useHistory } from 'react-router';

const CreateEvent = () => {
    const history = useHistory();
    const { postEventAsync } = useClient();

    const [state, setState] = useState({
        title: "",
        description: "",
        begin: "",
        end: "",
        error: ""
    })

    const handleChange = (e) => {
        setState(s => ({
            ...s,
            error: "",
            [e.target.name]: e.target.value
        }))
    }

    const handleCreateAsync = async (e) => {
        e.preventDefault();
        
        try {
            const result = await postEventAsync({
                title: state.title,
                description: state.description,
                startDate: state.begin,
                endDate: state.end
            });
            console.log(result)

            history.goBack();
        } catch(error) {
            console.log(error)
            setState(s => ({
                ...s,
                error: "An error happend see console log."
            }))
        }
    }

    return (
        <div>
            <h1>Create Event</h1>

            <form>
                <div>
                    <input id="title" name="title" type="text" value={state.title} onChange={handleChange} />
                </div>
                <div>
                    <input id="description" name="description" type="text" value={state.description} onChange={handleChange} />
                </div>
                <div>
                    <input id="begin" name="begin" type="text" value={state.begin} onChange={handleChange} />
                </div>
                <div>
                    <input id="end" name="end" type="text" value={state.end} onChange={handleChange} />
                </div>
                <div>
                    <button onClick={handleCreateAsync}>Submit</button>
                </div>
                {state.error && <p>{state.error}</p>}
            </form>
        </div>
    )
}

export default CreateEvent
