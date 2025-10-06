export default function StatsPanel({ data }: { data: { count: number; min: number; max: number; avg: number } }) {
  return (
    <div className="bg-white rounded-xl shadow-sm border p-4">
      <h3 className="font-semibold text-gray-700 mb-2">Aggregated Statistics</h3>
      <ul className="text-sm text-gray-600 space-y-1">
        <li><span className="font-medium text-gray-800">Count:</span> {data.count}</li>
        <li><span className="font-medium text-gray-800">Min:</span> {data.min.toFixed(2)}</li>
        <li><span className="font-medium text-gray-800">Max:</span> {data.max.toFixed(2)}</li>
        <li><span className="font-medium text-gray-800">Avg:</span> {data.avg.toFixed(2)}</li>
      </ul>
    </div>
  );
}
