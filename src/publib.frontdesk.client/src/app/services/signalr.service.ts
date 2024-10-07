import { EventEmitter, Injectable, Injector, OnDestroy } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { APP_CONSTANTS } from 'app/constants/app.constants';
import { environment } from 'environments/environment';
import { BookProvisionDto } from '../interfaces/book-provision';

@Injectable({
    providedIn: 'root'
})
export class SignalRService implements OnDestroy
{
    public apiUrl: string = "";
    private hubConnection: signalR.HubConnection;
    public bookProvisionReceived = new EventEmitter<{ bookProvision: BookProvisionDto }>();

    constructor(private injector: Injector)
    {
        this.apiUrl = environment.apiUrl;
        const messagingHubUrl: string = `${this.apiUrl}messageHub`;

        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(messagingHubUrl)
            .configureLogging(signalR.LogLevel.Warning)
            // optional: auto reconnect
            .withAutomaticReconnect([0, 2000, 10000, 30000])
            .build();
    }

    public isDisconnected(): boolean
    {
        return !this.hubConnection || this.hubConnection.state === signalR.HubConnectionState.Disconnected;
    }

    /**
     * Establishes the connection and sets up the message listener.
     * @returns A Promise that resolves when the connection is established, or rejects on error.
     */
    public initializeConnection(): Promise<void>
    {
        return this.startConnection()
            .then(() =>
            {
                this.addReceiveMessageListener();
                console.log('SignalR initialized successfully');
            })
            .catch((error) =>
            {
                console.error('Failed to initialize SignalR', error);
                throw error;
            });
    }

    public async stopConnection(): Promise<void>
    {
        if (this.hubConnection)
        {
            try
            {
                await this.hubConnection.stop();
                console.log('Connection stopped');
            } catch (err)
            {
                console.error('Error while stopping connection:', err);
                throw err;
            }
        }
    }

    public removeListeners(): void
    {
        if (this.hubConnection)
        {
            this.hubConnection.off('BookReadyForPickup');
            this.hubConnection.off('ErrorNotification');
        }
    }

    public addReceiveMessageListener(): void
    {
        this.hubConnection.on('BookReadyForPickup', (bookProvision: BookProvisionDto) =>
        {
            console.log(`Received book provision: ${bookProvision.book.title}`);
            this.bookProvisionReceived.emit({ bookProvision });
        });

        this.hubConnection.on("ErrorNotification", (errorMessage: string) =>
        {
            console.error("Received error from server:", errorMessage);
        });
    }

    private joinGroup(groupName: string): void
    {
        this.hubConnection.invoke('JoinGroup', groupName)
            .then(() => console.log(`Joined group: ${groupName}`))
            .catch(err => console.error('Error while joining group: ' + err));
    }

    private startConnection(): Promise<void>
    {
        console.log("Starting SignalR Connection.");
        return this.hubConnection.start()
            .then(() =>
            {
                console.log('SignalR connection started');
                this.joinGroup(APP_CONSTANTS.SIGNALR_GROUP_FRONTDESK);
            })
            .catch(err =>
            {
                console.log('Error while starting connection: ' + err);
                throw err;
            });
    }

    ngOnDestroy(): void
    {
        this.stopConnection();
        this.removeListeners();
    }
}
