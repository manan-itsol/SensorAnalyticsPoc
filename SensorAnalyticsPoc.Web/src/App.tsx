import LiveChart from "./components/LiveChart";

function App() {
  return (
    <div className="min-h-screen bg-gray-50 flex flex-col">
      {/* Header */}
      <header className="border-b border-gray-200 py-4 bg-white shadow-sm">
        <h1 className="text-2xl font-bold text-center text-gray-800">
          Real-Time Sensor Analytics Dashboard
        </h1>
      </header>

      {/* Main content */}
      <main className="flex-1 p-4">
        <LiveChart />
      </main>

      {/* Footer */}
      <footer className="text-center text-sm text-gray-500 py-3 border-t border-gray-200">
        © 2025 Real-Time Analytics POC
      </footer>
    </div>
  );
}

export default App;
