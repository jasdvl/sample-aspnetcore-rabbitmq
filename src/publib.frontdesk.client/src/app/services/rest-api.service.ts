import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class ApiService
{
    private apiUrl = `${environment.apiUrl}api`;

    // Standard-Header für JSON-Anfragen
    private headers = new HttpHeaders({
        'Content-Type': 'application/json'
    });

    constructor(private http: HttpClient) { }

    // GET-Methode
    get<T>(endpoint: string): Observable<T>
    {
        return this.http.get<T>(`${this.apiUrl}/${endpoint}`, { headers: this.headers });
    }

    // POST-Methode
    post<T>(endpoint: string, body: any): Observable<T>
    {
        return this.http.post<T>(`${this.apiUrl}/${endpoint}`, body, { headers: this.headers });
    }

    // PUT-Methode
    put<T>(endpoint: string, body: any): Observable<T>
    {
        return this.http.put<T>(`${this.apiUrl}/${endpoint}`, body, { headers: this.headers });
    }

    // PATCH-Methode
    patch<T>(endpoint: string, body: any): Observable<T>
    {
        return this.http.patch<T>(`${this.apiUrl}/${endpoint}`, body, { headers: this.headers });
    }

    // DELETE-Methode
    delete<T>(endpoint: string): Observable<T>
    {
        return this.http.delete<T>(`${this.apiUrl}/${endpoint}`, { headers: this.headers });
    }
}
