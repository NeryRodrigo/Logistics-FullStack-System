import axios from 'axios';

// ⚠️ IMPORTANTE: 
// Revisa en qué puerto se abrió tu Backend (.NET API).
// Usualmente es https://localhost:7152 o similar.
// Copia la URL de tu Swagger (sin el /swagger/index.html) y pégala aquí.
const API_URL = 'http://localhost:5000/api'; 

export const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});