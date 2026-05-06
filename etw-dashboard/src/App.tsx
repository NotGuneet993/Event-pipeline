import { useState } from 'react';
import {
    FluentProvider,
    webLightTheme,
    Input,
    Spinner,
    Title1,
    Tab,
    TabList,
    SelectTabEvent,
    SelectTabData,
} from '@fluentui/react-components';
import { Search24Regular } from '@fluentui/react-icons';
import { EventTable } from './components/EventTable';
import { useWebSocket } from './hooks/useWebSocket';
import { useSearch } from './hooks/useSearch';

export default function App() {
    const [query, setQuery] = useState('');
    const [activeTab, setActiveTab] = useState('live');

    const liveEvents = useWebSocket('ws://localhost:5234/ws');
    const { results, loading } = useSearch(query);

    const onTabSelect = (_: SelectTabEvent, data: SelectTabData) => {
        setActiveTab(data.value as string);
    };

    return (
        <FluentProvider theme={webLightTheme}>
            <div style={{ padding: '24px' }}>
                <Title1 style={{ marginBottom: '16px' }}>ETW Telemetry Dashboard</Title1>

                <TabList selectedValue={activeTab} onTabSelect={onTabSelect}>
                    <Tab value="live">Live Feed</Tab>
                    <Tab value="search">Search</Tab>
                </TabList>

                {activeTab === 'search' && (
                    <div style={{ margin: '16px 0' }}>
                        <Input
                            contentBefore={<Search24Regular />}
                            placeholder="Search by event, process, provider..."
                            value={query}
                            onChange={(_, data) => setQuery(data.value)}
                            style={{ width: '400px' }}
                        />
                    </div>
                )}

                <div style={{ marginTop: '16px' }}>
                    {activeTab === 'live' && <EventTable events={liveEvents} />}
                    {activeTab === 'search' && (
                        loading
                            ? <Spinner label="Searching..." />
                            : <EventTable events={results} />
                    )}
                </div>
            </div>
        </FluentProvider>
    );
}
