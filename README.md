# Sensor Analytics POC

This Proof of Concept (POC) demonstrates a real-time **Sensor Analytics
Dashboard** built with **.NET Core API** and **React (Vite +
TypeScript)**.\
It simulates sensor readings, processes streaming data, stores it in
Redis, and visualizes analytics on the frontend in real time.

------------------------------------------------------------------------

## 🧩 Project Structure

    /SensorAnalytics.API        → .NET 9 backend for simulating and broadcasting sensor readings
    /SensorAnalyticsPoc.Web     → React + TypeScript frontend for real-time dashboard

------------------------------------------------------------------------

## ⚙️ Prerequisites

Before running the projects, ensure the following are installed:

-   [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
-   [Node.js (v18+)](https://nodejs.org/)
-   [Redis Server](https://redis.io/download/)

### Redis Setup

You can run Redis locally via Docker or install it natively.

#### Using Docker (recommended)

``` bash
docker run --name redis -d -p 6379:6379 redis
```

#### Verify Redis is running

``` bash
redis-cli ping
# should return: PONG
```

------------------------------------------------------------------------

## 🚀 Running the .NET API (Backend)

### 1️⃣ Navigate to the backend folder

``` bash
cd SensorAnalytics.API
```

### 2️⃣ Restore dependencies

``` bash
dotnet restore
```

### 3️⃣ Update Redis configuration (if needed)

Open `appsettings.json` and ensure your Redis connection string matches
your environment:

``` json
"Redis": {
  "ConnectionString": "localhost:6379"
}
```

### 4️⃣ Run the API

``` bash
dotnet run
```

The API should start on `https://localhost:44327/` 

------------------------------------------------------------------------

## 💻 Running the React Frontend

### 1️⃣ Navigate to the frontend folder

``` bash
cd SensorAnalyticsPoc.Web
```

### 2️⃣ Install dependencies

``` bash
npm install
```

### 3️⃣ Start the development server

``` bash
npm run dev
```

This will start the React app on `http://localhost:5173` (default Vite
port).

### 4️⃣ Connect Frontend with Backend

Ensure the SignalR connection URL in your frontend configuration matches
your running backend (e.g., `https://localhost:44327/hub/sensor`).

------------------------------------------------------------------------

## 🧠 Features Overview

-   Simulates **1000 sensor readings per second**
-   Broadcasts readings to frontend using **SignalR**
-   Persists data to Redis for **24 hours auto-purge**
-   Displays **real-time chart** using `uPlot`
-   Aggregated statistics and anomaly alerts in UI

------------------------------------------------------------------------

## 🧹 Cleaning Up

To stop all running services:

``` bash
docker stop redis && docker rm redis
```

To clean build artifacts:

``` bash
dotnet clean
npm run build
```

------------------------------------------------------------------------

## 📄 License

This POC is for internal evaluation purposes only.
