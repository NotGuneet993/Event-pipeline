import { useState, useEffect } from 'react';
import axios from 'axios';
import { EtwEvent } from './useWebSocket';

export function useSearch(query: string) {
    const [results, setResults] = useState<EtwEvent[]>([]);
    const [loading, setLoading] = useState<boolean>(false);

    useEffect(() => {
        if (!query.trim()) {
            setResults([]);
            return;
        }

        const debounce = setTimeout(async () => {
            setLoading(true);
            try {
                const response = await axios.get<EtwEvent[]>(`/api/search?q=${encodeURIComponent(query)}`);
                setResults(response.data);
            } catch (err) {
                console.error('Search failed', err);
            } finally {
                setLoading(false);
            }
        }, 300);

        return () => clearTimeout(debounce);
    }, [query]);

    return { results, loading };
}