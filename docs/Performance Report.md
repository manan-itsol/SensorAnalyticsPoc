# Performance Report — Real-Time Analytics Dashboard (POC)

## Goal
Verify system can:
- Ingest **1,000 sensor readings/sec**
- Maintain **100,000 points in memory**
- Persist data for **24 hours** with auto-purge
- Provide real-time chart & alerts without UI degradation

---

## Test Environment
- Backend: .NET 8 Web API + SignalR
- Frontend: React + Vite + Chart.js
- Persistence: Redis (local)
- Hardware: 8-core CPU, 16GB RAM

---

## Methodology
1. **Load Generator**  
   - BackgroundService produced 1,000 readings/sec (100x10 batches).  

2. **Backend Metrics**  
   - Measured throughput (events/sec), CPU%, memory usage.  
   - Observed Redis TTL cleanup after 24h. 
   - Observed Task manager to see the utilized resources (10% - 20% CPU load) 

3. **Frontend Metrics** 
   - Measured chart FPS, memory growth, latency from server → UI. 
   - Observed Task manager to see the utilized resources (10% - 20% CPU load)

---

## Results

| Metric                          | Observed Value |
|---------------------------------|----------------|
| Throughput (events/sec)         | ~1,000 steady  |
| Batch delivery latency           | 100–200 ms     |
| Memory (100k points in buffer)  | ~15–20 MB      |
| CPU load (backend)              | ~12–18%        |
| Alerts detected                 | Events flagged (threshold-based) |