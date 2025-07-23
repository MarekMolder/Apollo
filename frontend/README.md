# 📦 Inventory Frontend (Vue 3 + Vite)

This is the frontend for the Inventory Stock Management System. It is built with **Vue 3**, **Vite**, and **TypeScript**, and communicates with the backend ASP.NET Core API over HTTP.

---

## 📁 Project Structure

```
frontend/
├── public/              # Static assets (favicon, etc)
├── src/
│   ├── assets/          # Images, styles
│   ├── components/      # Reusable Vue components
│   ├── layouts/         # Shared layout wrappers
│   ├── pages/           # Route-level views (used with Vue Router)
│   ├── router/          # Vue Router setup
│   ├── store/           # Pinia stores for state management
│   ├── services/        # Axios API services
│   ├── types/           # TypeScript types and interfaces
│   ├── App.vue          # Root component
│   └── main.ts          # Entry point
├── .env                 # Environment variables
├── index.html           # HTML entry
├── package.json         # Project config and dependencies
└── vite.config.ts       # Vite configuration
```

---

## 🚀 Tech Stack

- [Vue 3](https://vuejs.org/)
- [Vite](https://vitejs.dev/)
- [TypeScript](https://www.typescriptlang.org/)
- [Bootstrap 5](https://getbootstrap.com/)
- [Vue Router](https://router.vuejs.org/)
- [Axios](https://axios-http.com/)
- [Pinia](https://pinia.vuejs.org/) (state management)
- [ESLint](https://eslint.org/)

---

## 🧰 Project Setup

```bash
npm install
```

### 🔄 Development Server

```bash
npm run dev
```

Runs the app in development mode with hot-reload at `http://localhost:5173`.

Make sure your backend API is also running (e.g. at `http://localhost:5294` or Docker).

---

### ⚙️ Production Build

```bash
npm run build
```

Builds the app for production to the `dist/` folder.

---

## 🌐 Environment Configuration

Create a `.env` file at the root of the project if needed:

```env
VITE_API_URL=http://localhost:8080/api/v1
```

You can then use this in code via `import.meta.env.VITE_API_URL`.

---

## 🧪 Backend API Dependency

This frontend relies on a working backend API (ASP.NET Core) available at the configured base URL.

Make sure to:

- Start the backend server (`dotnet run` or Docker)
- Ensure CORS is enabled in the backend for frontend URL

---

## 📄 License

This project is provided for educational and internal use only.