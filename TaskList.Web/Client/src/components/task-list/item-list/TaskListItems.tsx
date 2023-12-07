import React from 'react'
import { TaskItemDDL } from '../../../api/Api'

export default function TaskListItems({ taskItems, onclick } : { taskItems : TaskItemDDL[], onclick : (taskId : number) => void }) {
    return <ul className="task-list">
            { taskItems.map(m => 
                <li onClick={() => onclick(m.id!)} key={m.id} className="task">
                    <div className="task-description">{ m.name } <span className='emoji'>➡️</span></div>
                </li>
            )}
        </ul>
}