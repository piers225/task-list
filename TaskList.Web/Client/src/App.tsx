import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import './custom.css';
import Login from './components/login/Login';
import TaskList from './components/task-list/TaskList';

export default function() {

    return (
        <Routes>
          <Route path={'/'} element={<Login />} />
          <Route path={'/task-list'} element={<TaskList />} />
        </Routes>
    );
  }
