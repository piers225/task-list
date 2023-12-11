import React from 'react';
import { render, fireEvent, screen } from '@testing-library/react';
import TaskList from './TaskList'; 

describe('<TaskList />', () => {

  let open : any;

  beforeEach(() => {
    open = jest.fn();
    // @ts-ignore
    global.XMLHttpRequest = jest.fn(() => ({
      open: open,
      send: jest.fn(),
      setRequestHeader: jest.fn(),
    }));
    
  })
  it('Should show no pending message when response is empty', () => {
    render(<TaskList />);
    const element = screen.getByTestId('noPendingMessages');
    expect(element).not.toBeUndefined()
  });
  it('Should show no completed message when response is empty', () => {
    render(<TaskList />);
    const element = screen.getByTestId('noCompletedMessages');
    expect(element).not.toBeUndefined()
  });

  it('Should make a request to get items', () => {
    render(<TaskList />);
    expect(open).toHaveBeenCalledWith('GET', '/api/tasks', true)
  });

  it('Clicking add button and returning `test Value` calls the create end point', () => {
    // @ts-ignore
    global.prompt = jest.fn().mockReturnValue('test Value');
    render(<TaskList />);
    const addButton = screen.getByTestId('add-button');
    fireEvent.click(addButton);
    expect(open).toHaveBeenCalledWith('POST', '/api/tasks', true)
  });
});