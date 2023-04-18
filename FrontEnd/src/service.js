import axios from 'axios';
axios.defaults.baseURL= 'http://localhost:5030'

axios.interceptors.response.use(
    (response) => {
      return response;
    },
    (error) => {
      console.error('An error occurred:', error);
      return Promise.reject(error);
    }
  );

export default {
  getTasks: async () => {
    const result = await axios.get(`/items`);
    return result.data;
  },

  addTask: async(newToDo)=>{
    console.log('addTask', newToDo)
    await axios.post(`/items`, {name:newToDo,isComplete:false});
    return {};
  },

  setCompleted: async(id, isComplete)=>{
    console.log('setCompleted', {id, isComplete});
    await axios.put(`/items/${id}`);
  },

  deleteTask:async(id)=>{
    console.log('deleteTask')
    await axios.delete(`/items/${id}`);
  }
};

