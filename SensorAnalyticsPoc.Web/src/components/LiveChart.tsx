import { useEffect, useRef, useState } from "react";
import uPlot from "uplot";
import "uplot/dist/uPlot.min.css";
import { connection, ensureConnectionStarted } from "../signalr";
import StatsPanel from "./StatsPanel";
import Alerts from "./Alerts";
import type { ISensorReading } from "../models/SensorReading";

export default function LiveChart() {
  const chartRef = useRef<HTMLDivElement>(null);
  const plotRef = useRef<uPlot | null>(null);
  const bufferRef = useRef<ISensorReading[]>([]);

  const [aggregatedStats, setAggregatedStats] = useState({
    count: 0,
    min: 0,
    max: 0,
    avg: 0,
  });

  useEffect(() => {
    let raf: number;
    async function start() {
      try {
        await ensureConnectionStarted();

        connection.on("SensorBatch", (batch: ISensorReading[]) => {
          bufferRef.current.push(...batch);
          if (bufferRef.current.length > 100_000) {
            bufferRef.current.splice(0, bufferRef.current.length - 100_000);
          }
        });

        const opts: uPlot.Options = {
          title: "Real-Time Analytics Demo",
          width: chartRef.current?.clientWidth || 1000,
          height: 500,
          scales: { x: { time: true } },
          series: [
            {},
            {
              label: "Sensor Value",
              stroke: "#2563eb",
              width: 1,
            },
          ],
        };

        const data: uPlot.AlignedData = [[], []];
        if (!plotRef.current) {
          plotRef.current = new uPlot(opts, data, chartRef.current!);
        }

        const update = () => {
          const points = bufferRef.current;
          if (points.length) {
            const xs = points.map((p) => p.timestampUtc / 1000);
            const ys = points.map((p) => p.value);
            plotRef.current!.setData([xs, ys]);
          }
          raf = requestAnimationFrame(update);
        };

        raf = requestAnimationFrame(update);

        return () => {
          cancelAnimationFrame(raf);
          plotRef.current?.destroy();
          plotRef.current = null;
          connection.off("SensorBatch");
        };
      } catch (err) {
        console.error(err);
      }
    }

    start();
  }, []);

  useEffect(() => {
    const aggregatedInterval = setInterval(() => {
      const points = bufferRef.current;
      if (points.length === 0) return;

      const count = points.length;
      let min = Number.POSITIVE_INFINITY;
      let max = Number.NEGATIVE_INFINITY;
      let sum = 0;

      for (let i = 0; i < points.length; i++) {
        const val = points[i].value;
        if (val < min) min = val;
        if (val > max) max = val;
        sum += val;
      }

      const avg = sum / points.length;

      setAggregatedStats((prev) => {
        if (
          prev.min !== min ||
          prev.max !== max ||
          Math.abs(prev.avg - avg) > 0.001
        ) {
          return { count, min, max, avg };
        }
        return prev;
      });
    }, 500);

    return () => clearInterval(aggregatedInterval);
  }, []);

  return (
    <div className="grid grid-cols-[3fr_1fr] gap-4 w-full h-full">
      <div className="bg-white rounded-xl shadow p-4 border border-gray-200">
        <h2 className="font-semibold mb-3 text-gray-700">Live Sensor Data</h2>
        <div ref={chartRef} className="overflow-hidden rounded-lg" />
      </div>

      <div className="flex flex-col gap-4">
        <StatsPanel data={aggregatedStats} />
        <Alerts />
      </div>
    </div>
  );
}
