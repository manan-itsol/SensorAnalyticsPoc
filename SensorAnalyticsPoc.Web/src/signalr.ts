import * as signalR from "@microsoft/signalr";

export const connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:44327/hubs/sensor")
  .withAutomaticReconnect()
  .configureLogging(signalR.LogLevel.Debug)
  .build();

export async function ensureConnectionStarted() {
  if (connection.state === signalR.HubConnectionState.Disconnected) {
    await connection.start();
    console.log("SignalR connected");
  }
}