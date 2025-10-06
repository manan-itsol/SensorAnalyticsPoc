import { useEffect, useRef, useState } from "react";
import type { ISensorReading } from "../models/SensorReading";
import { connection, ensureConnectionStarted } from "../signalr";

export default function Alerts() {
  const alertBufferRef = useRef<ISensorReading[]>([]);
  const [alerts, setAlerts] = useState<ISensorReading[]>([]);

  useEffect(() => {
    async function start() {
      await ensureConnectionStarted();

      connection.on("AnomalyAlert", (alertsBatch: ISensorReading[]) => {
        alertBufferRef.current.push(...alertsBatch);
        if (alertBufferRef.current.length > 50)
          alertBufferRef.current.splice(0, alertBufferRef.current.length - 50);
      });

      const interval = setInterval(() => {
        setAlerts([...alertBufferRef.current]);
      }, 500);

      return () => {
        clearInterval(interval);
        connection.off("AnomalyAlert");
      };
    }

    start();
  }, []);

  return (
    <div className="bg-red-50 border border-red-200 rounded-xl p-4 max-h-80 overflow-y-auto">
      <h3 className="font-semibold text-red-700 mb-2">⚠️ Anomalies (&gt; 98.0) </h3>
      {alerts.length === 0 ? (
        <p className="text-sm text-gray-500">No anomalies detected</p>
      ) : (
        <div className="space-y-1">
          {alerts.map((a, i) => (
            <div key={i} className="text-sm border-b border-red-100 py-1">
              <span className="font-medium text-gray-700">
                {new Date(a.timestampUtc).toLocaleTimeString()}:
              </span>{" "}
              {a.sensorId} (<strong>{a.value.toFixed(2)}</strong>)
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
