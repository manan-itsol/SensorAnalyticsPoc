# Decision Document — Real-Time Analytics Dashboard (POC)

## Overview

This proof of concept demonstrates a **real-time analytics dashboard** designed to handle **1,000 sensor readings per second**, maintain **100,000 points in memory**, and retain data for **24 hours** with automatic purging.

The backend is built with **.NET 9 + SignalR**, the frontend with **React + TypeScript + uPlot**, and **Redis** is used for temporary persistence and scheduled data purging.

---

## Architecture Decisions & Trade-Offs

### Data Ingestion

* **Option considered**: Push each sensor reading directly to clients.
* **Decision**: Batch readings (100 every 100ms) to reduce network overhead and provide smoother UI updates.
* **Trade-off**: Introduces slight latency (~100–200ms), which is acceptable for analytics scenarios.

### Storage

* **Option considered**: Store all readings in memory.
* **Decision**: Use Redis as an in-memory data store with a 24-hour rolling purge.
* **Trade-off**: Provides high throughput and simplifies retention management.

### Separation of Concerns

* **Option considered**: Use a single background `HostedService` for reading generation, storage, purging, and SignalR broadcasting.
* **Decision**: Implement separate `HostedService` components for each responsibility and connect them with a channel/queue mechanism.
* **Trade-off**: Improves scalability, enforces separation of concerns, and enables switching data stores without affecting other components.

### Frontend Updates

* **Option considered**: Update aggregated stats and anomaly detection in real-time as data arrives.
* **Decision**: Batch UI updates every 200–500ms.
* **Trade-off**: Introduces ~200ms delay in UI updates but ensures smooth performance under heavy load.

---

## Performance Optimizations

1. **Message batching**: 100 readings per 100ms.
2. **Service separation**: Dedicated hosted services for reading, storage, purging, and broadcasting.
3. **Rolling statistics**: Constant-time updates for efficiency.
4. **SignalR + WebSockets**: Reduced overhead compared to HTTP polling.
5. **UI downsampling**: Aggregated stats and alerts are rendered at 200ms intervals for efficiency.

---

## AI Suggestions Rejected

1. **Pure in-memory storage** → Would exhaust server memory at scale.
2. **Single insert per Redis reading** → Inefficient for 1,000 inserts per second.
3. **Single hosted service for all operations** → Reduces maintainability and causes performance bottlenecks.

---

## Validation Approach

* Benchmarked with the **.NET Benchmark plugin**.
* Confirmed that the buffer capped at **100,000 points**.
* Validated that the UI remained responsive during sustained high-load tests.
