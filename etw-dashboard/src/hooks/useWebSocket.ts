import { useEffect, useRef, useState } from 'react'

export interface EtwEvent {
    id: number;
    eventName: string;
    time: string;
    level: number;
    processID: number;
    processName: string;
    providerGuid: string;
    providerName: string;
    machineName: string;
}

export function useWebSocket(url: string, maxEvents: number = 500): EtwEvent[] {
    const [events, setEvents] = useState<EtwEvent[]>([]);
    const wsRef = useRef<WebSocket | null>(null);

    useEffect(() => {
        const ws = new WebSocket(url);
        wsRef.current = ws;

        ws.onmessage = (msg: MessageEvent) => {
            const evt: EtwEvent = JSON.parse(msg.data);
            setEvents(prev => [evt, ...prev].slice(0, maxEvents));
        };

        ws.onclose = () => console.log('WebSocket disconnected');
        ws.onerror = (err) => console.error('WebSocket error', err);

        return () => ws.close();
    }, [url]);

    return events;
}