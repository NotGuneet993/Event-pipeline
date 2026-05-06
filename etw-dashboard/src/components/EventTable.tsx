import {
    DataGrid,
    DataGridHeader,
    DataGridHeaderCell,
    DataGridBody,
    DataGridRow,
    DataGridCell,
    TableColumnDefinition,
    createTableColumn,
    Badge,
} from '@fluentui/react-components';
import { EtwEvent } from '../hooks/useWebSocket';

const columns: TableColumnDefinition<EtwEvent>[] = [
    createTableColumn<EtwEvent>({
        columnId: 'time',
        renderHeaderCell: () => 'Time',
        renderCell: (evt) => new Date(evt.time).toLocaleString(),
    }),
    createTableColumn<EtwEvent>({
        columnId: 'eventName',
        renderHeaderCell: () => 'Event',
        renderCell: (evt) => evt.eventName,
    }),
    createTableColumn<EtwEvent>({
        columnId: 'processName',
        renderHeaderCell: () => 'Process',
        renderCell: (evt) => evt.processName,
    }),
    createTableColumn<EtwEvent>({
        columnId: 'processID',
        renderHeaderCell: () => 'PID',
        renderCell: (evt) => evt.processID.toString(),
    }),
    createTableColumn<EtwEvent>({
        columnId: 'providerName',
        renderHeaderCell: () => 'Provider',
        renderCell: (evt) => evt.providerName,
    }),
    createTableColumn<EtwEvent>({
        columnId: 'providerGuid',
        renderHeaderCell: () => 'Provider GUID',
        renderCell: (evt) => evt.providerGuid,
    }),
    createTableColumn<EtwEvent>({
        columnId: 'level',
        renderHeaderCell: () => 'Level',
        renderCell: (evt) => (
            <Badge color={evt.level <= 2 ? 'danger' : evt.level === 3 ? 'warning' : 'success'}>
                {evt.level}
            </Badge>
        ),
    }),
    createTableColumn<EtwEvent>({
        columnId: 'machineName',
        renderHeaderCell: () => 'Machine',
        renderCell: (evt) => evt.machineName,
    }),
    createTableColumn<EtwEvent>({
        columnId: 'id',
        renderHeaderCell: () => 'ID',
        renderCell: (evt) => evt.id.toString(),
    }),
];

interface EventTableProps {
    events: EtwEvent[];
}

export function EventTable({ events }: EventTableProps) {
    return (
        <DataGrid
            items={events}
            columns={columns}
            getRowId={(evt) => `${evt.id}-${evt.time}`}
        >
            <DataGridHeader>
                <DataGridRow>
                    {({ renderHeaderCell }) => (
                        <DataGridHeaderCell>{renderHeaderCell()}</DataGridHeaderCell>
                    )}
                </DataGridRow>
            </DataGridHeader>
            <DataGridBody<EtwEvent>>
                {({ item, rowId }) => (
                    <DataGridRow<EtwEvent> key={rowId}>
                        {({ renderCell }) => (
                            <DataGridCell>{renderCell(item)}</DataGridCell>
                        )}
                    </DataGridRow>
                )}
            </DataGridBody>
        </DataGrid>
    );
}