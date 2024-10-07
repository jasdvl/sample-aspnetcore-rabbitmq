import { HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { SignalRService } from './services/signalr.service';

@Component({
    selector: 'app-root',
    template: '<app-layout></app-layout>',
    styles: [],
})
export class AppComponent implements OnInit
{
    constructor(private signalRService: SignalRService) { }

    ngOnInit()
    {
        this.checkAndRestartSignalR();
    }

    private checkAndRestartSignalR()
    {
        if (this.signalRService.isDisconnected())
        {
            this.signalRService.initializeConnection()
                .catch(error => console.error('Failed to reestablish SignalR connection', error));
        }
    }
}
