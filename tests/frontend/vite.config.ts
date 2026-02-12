import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import path from 'path'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      "@": path.resolve(__dirname, "../../ExameDesenvolvedorDeTestes/web/src"),
    },
  },
  test: {
    globals: true,
    environment: 'jsdom',
    include: ['unit/**/*.test.ts', 'unit/**/*.test.tsx', 'integration/**/*.test.ts', 'integration/**/*.test.tsx'],
    setupFiles: [],
  },
})
