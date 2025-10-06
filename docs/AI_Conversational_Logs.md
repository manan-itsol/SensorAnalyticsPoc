# AI Conversations Log

## Raw Conversations
Here is the shareable link to my ChatGPT conversations:
[View Conversation Log](https://chatgpt.com/share/68e39f34-40d4-8010-8021-987ba634c862)

---

## Disagreements & Annotations

### Disagreement 1 — Use of In-Memory Storage
- **AI said**: AI suggested using in-memory storage, which 
- **I disagreed because**: of following drawbacks:

  1. In-memory storage is temporary and relies on RAM.
  2. Handling a very large dataset (1000 × 60 × 60 × 24 = 86,400,000 readings per day) would quickly overwhelm server memory.    
- **My solution**: Use Redis data store with persisted enabled which provide high-throuhput in our case. 
- **Evidence of performance issue**: It is evident that using Redis reduced the memory overload on server because server no more stores larger dataset in-memory (i.e., in RAM).  

### Disagreement 2 — Redis Storage Strategy
- **AI said**: AI proposed storing every single reading in Redis individually  
- **I disagreed because**: I found this inefficient, as each operation incurs a round-trip cost.  
- **My solution**: I suggested using batch writes to maximize throughput.  

### Disagreement 3 — Single Hosted Service for All Operations
- **AI said**: AI recommended consolidating reading generation, storage, purging, and SignalR broadcasting into a single `HostedService`.  
- **I disagreed because**: I opposed this approach because it risked slowing down reading generation and broadcasting.  
- **My solution**: My design separates concerns by introducing two additional hosted services—one dedicated to storage and another to purging.  
