import { useEffect, useState } from 'react'
import './tasklist.css'
import TaskListItems from './item-list/TaskListItems';
import { TaskItemDDL } from '../../api/Api';
import useApi from '../../api/useApi';

export default function TaskList() {

    const [pendingTasks, setPendingTasks] = useState<TaskItemDDL[]>([]);
    const [completedTasks, setCompletedTasks] = useState<TaskItemDDL[]>([]);
    const { client } = useApi()

    const getTasks = async() => {
        const { data } = await client.api.tasksList()
        setPendingTasks(data.filter(f => f.status === 'Pending'))
        setCompletedTasks(data.filter(f => f.status === 'Completed'))
    }

    const createTask = async() => {
        const taskName = prompt('Add Task Name');
        if (!taskName) {
            return;
        }
        await client.api.tasksCreate({
            id : 0, 
            status: 'Pending',
            name : taskName
        });
        await getTasks();
    }

    const movePending = async(task : TaskItemDDL) => {
        task.status = 'Pending'
        await client.api.tasksUpdate(task.id!.toString(), { taskId : task.id! }, task)
        await getTasks()
    }
    const moveActioned = async(task : TaskItemDDL) => {
        task.status = 'Completed'
        await client.api.tasksUpdate(task.id!.toString(), { taskId : task.id! }, task)
        await getTasks()
    }

    useEffect(() => {
        (async function() {
            await getTasks()
        })()
    }, [])

    return <div className="task-container">
        <h1>Task List</h1>
        <div className="task-form">
            <div className="add-task">
                <button id="add-button" data-testid="add-button" onClick={createTask}>Add Task</button>
            </div>
            <hr />
            <h2>Pending Tasks</h2>
            <TaskListItems onclick={moveActioned} taskItems={pendingTasks} />          
            { pendingTasks.length === 0 && <i data-testid="noPendingMessages">None</i> }

            <h2>Completed Tasks</h2>
            <TaskListItems onclick={movePending} taskItems={completedTasks} />
            { completedTasks.length === 0 && <i data-testid="noCompletedMessages">None</i> }
        
        </div>
    </div>
}