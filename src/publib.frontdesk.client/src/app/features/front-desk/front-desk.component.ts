import { HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { BookProvisionDto } from 'app/interfaces/book-provision';
import { BookReservationDto } from 'app/interfaces/book-reservation.dto';
import { BookDto } from 'app/interfaces/book.dto';
import { PersonDto } from 'app/interfaces/person.dto';
import { ApiService } from 'app/services/rest-api.service';
import { SignalRService } from 'app/services/signalr.service';
import { Subscription } from 'rxjs';

@Component({
    selector: 'front-desk',
    templateUrl: './front-desk.component.html',
    styleUrl: './front-desk.component.css'
})
export class FrontDeskComponent implements OnInit
{
    private bookProvisionSubscription!: Subscription;
    userId: number = 0;
    title: string = '';
    year: number = 0;

    private headers = new HttpHeaders({
        'Content-Type': 'application/json'
    });

    constructor(
            private apiService: ApiService,
            private signalRService: SignalRService) { }

    ngOnInit()
    {
        this.bookProvisionSubscription = this.signalRService.bookProvisionReceived.subscribe(
            ({ bookProvision }) =>
            {
                this.onBookProvisionReceived(bookProvision);
            }
        );
    }

    applyMembership(lastName: string, firstName: string, city: string)
    {
        const person: PersonDto = {
            firstName: firstName,
            lastName: lastName,
            city: city
        };

        this.apiService.post('frontdesk/memberships/apply', person).subscribe({
            next: (response: any) =>
            {
                console.log('Membership applied.');
            },
            error: (error) =>
            {
                console.error('Error when applying membership.');
            }
        });
    }

    sendReservation(userId: string, title: string, year: string)
    {
        const book: BookDto = {
            title: title,
            year: parseInt(year)
        };

        const bookReservation: BookReservationDto = {
            userId: parseInt(userId),
            book: book
        };

        this.apiService.post('frontdesk/reservations/create', bookReservation).subscribe({
            next: (response: any) =>
            {
                console.log('Book reservation sent.');
            },
            error: (error) =>
            {
                console.error('Error when sending reservation.');
            }
        });
    }

    onBookProvisionReceived(bookProvision: BookProvisionDto): void
    {
        this.userId = bookProvision.userId;
        this.title = bookProvision.book.title;
        this.year = bookProvision.book.year;
    }

    ngOnDestroy()
    {
        if (this.bookProvisionSubscription)
        {
            this.bookProvisionSubscription.unsubscribe();
        }
    }
}
